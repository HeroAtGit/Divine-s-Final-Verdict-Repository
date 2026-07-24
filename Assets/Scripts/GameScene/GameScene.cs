using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public List<DialougeTrigger> events = new List<DialougeTrigger>();

    //Game Objects
    public GameObject BG;
    public GameObject TextBox;
    public GameObject FadeIn;
    public GameObject NPCTable;
    public GameObject MMPodium;
    public GameObject NPC;
    public GameObject StatueBG;
    public GameObject middleMan;
    public GameObject MMJustice;
    public GameObject MMJudgement;
    public GameObject MaskButtons;
    public GameObject DestinationButtons;
    [SerializeField] GameObject mainTextObject;

    //Warning Backgrounds
    [SerializeField] private GameObject heavenWarningBG;
    [SerializeField] private GameObject hellWarningBG;
    //Warning Dialogue
    [SerializeField] private DialougeTrigger heavensLamentDialogue;
    [SerializeField] private DialougeTrigger hellsGrumbleDialogue;

    //Events
    [SerializeField] private DialougeTrigger SelectE1;
    [SerializeField] private DialougeTrigger SelectE2;
    [SerializeField] private DialougeTrigger SelectE3;
    [SerializeField] private DialougeTrigger SelectE4;
    [SerializeField] private DialougeTrigger SelectE5;
    [SerializeField] private DialougeTrigger SelectE6;
    [SerializeField] private DialougeTrigger SelectE7;

    // Each event now judges its own soul - set Good/Bad per event here.
    // Index 0 = E01's soul, index 1 = E02's soul, ... index 6 = E07's soul.
    [Header("Per-Event Soul Types")]
    [SerializeField]
    private MoralityManager.SoulType[] soulTypes = new MoralityManager.SoulType[7];

    [SerializeField] private DialougeManager dialougeManager;
    [SerializeField] private MoralityManager moralityManager;

    // Optional - kept in sync with the current event's soul type so other
    // scripts reading NPC's SoulData component still see correct info.
    [SerializeField] private SoulData soulData;

    private MoralityManager.Mask? chosenMask;
    private MoralityManager.Destination? chosenDestination;

    void Start()
    {
        if (dialougeManager == null)
        {
            dialougeManager = FindAnyObjectByType<DialougeManager>(FindObjectsInactive.Include);
        }

        if (moralityManager == null)
        {
            moralityManager = FindAnyObjectByType<MoralityManager>();
        }

        if (soulData == null && NPC != null)
        {
            soulData = NPC.GetComponent<SoulData>();
            if (soulData == null)
            {
                Debug.LogWarning("No SoulData component found on NPC GameObject.");
            }
        }

        if (moralityManager != null)
        {
            moralityManager.onWarningTriggered += HandleWarningTriggered;
            moralityManager.onWarningFaded += HandleWarningFaded;
        }

        if (MaskButtons != null) MaskButtons.SetActive(false);
        if (DestinationButtons != null) DestinationButtons.SetActive(false);

        StartCoroutine(E01());
    }

    private void OnDestroy()
    {
        if (moralityManager != null)
        {
            moralityManager.onWarningTriggered -= HandleWarningTriggered;
            moralityManager.onWarningFaded -= HandleWarningFaded;
        }
    }

    private void HandleWarningTriggered(MoralityManager.WarningType warningType)
    {
        if (warningType == MoralityManager.WarningType.HeavensLament)
        {
            if (heavenWarningBG != null) heavenWarningBG.SetActive(true);
            Debug.Log("[WARNING] Heaven's Lament Triggered!");
        }
        else if (warningType == MoralityManager.WarningType.HellsGrumble)
        {
            if (hellWarningBG != null) hellWarningBG.SetActive(true);
            Debug.Log("[WARNING] Hell's Grumble Triggered!");
        }
    }

    private void HandleWarningFaded(MoralityManager.WarningType warningType)
    {
        if (warningType == MoralityManager.WarningType.HeavensLament)
        {
            if (heavenWarningBG != null) heavenWarningBG.SetActive(false);
            Debug.Log("[WARNING FADED] Heaven's Lament Fade Away.");
        }
        else if (warningType == MoralityManager.WarningType.HellsGrumble)
        {
            if (hellWarningBG != null) hellWarningBG.SetActive(false);
            Debug.Log("[WARNING FADED] Hell's Grumble Fade Away.");
        }
    }

    // --- Hooked to the four button OnClick() events in the Inspector ---
    public void SelectJustice()
    {
        chosenMask = MoralityManager.Mask.Justice;
        Debug.Log("Mask chosen: Justice");
    }

    public void SelectJudgement()
    {
        chosenMask = MoralityManager.Mask.Judgement;
        Debug.Log("Mask chosen: Judgement");
    }

    public void SelectHeaven()
    {
        chosenDestination = MoralityManager.Destination.Heaven;
        Debug.Log("Destination chosen: Heaven");
    }

    public void SelectHell()
    {
        chosenDestination = MoralityManager.Destination.Hell;
        Debug.Log("Destination chosen: Hell");
    }

    // --- Shared choice-gating coroutines (mask/destination panels only) ---
    IEnumerator WaitForMaskChoice()
    {
        chosenMask = null;
        MaskButtons.SetActive(true);
        Debug.Log("Waiting for mask choice...");
        yield return new WaitUntil(() => chosenMask != null);
        Debug.Log($"Mask choice received: {chosenMask.Value}");
        MaskButtons.SetActive(false);
    }

    IEnumerator WaitForDestinationChoice()
    {
        chosenDestination = null;
        DestinationButtons.SetActive(true);
        Debug.Log("Waiting for destination choice...");
        yield return new WaitUntil(() => chosenDestination != null);
        Debug.Log($"Destination choice received: {chosenDestination.Value}");
        DestinationButtons.SetActive(false);
    }

    // Blocks until the given warning dialogue actually finishes playing,
    // so it can never be overwritten by the next event's dialogue.
    private bool warningDialogueFinished;
    private void OnWarningDialogueFinished() { warningDialogueFinished = true; }

    IEnumerator PlayWarningDialogue(DialougeTrigger trigger)
    {
        if (trigger == null || dialougeManager == null) yield break;

        mainTextObject.SetActive(true);
        TextBox.SetActive(true);

        warningDialogueFinished = false;
        dialougeManager.onDialougeEnd += OnWarningDialogueFinished;
        trigger.TriggerDialouge();
        yield return new WaitUntil(() => warningDialogueFinished);
        dialougeManager.onDialougeEnd -= OnWarningDialogueFinished;
    }

    // Runs mask choice -> destination choice -> scores the verdict for the
    // given event index, then either starts the next event or ends the scene.
    IEnumerator RunVerdictThenContinue(int eventIndex, IEnumerator nextEvent)
    {
        yield return StartCoroutine(WaitForMaskChoice());
        yield return StartCoroutine(WaitForDestinationChoice());

        MoralityManager.SoulType soul = MoralityManager.SoulType.GoodPerson;
        if (soulTypes != null && eventIndex >= 0 && eventIndex < soulTypes.Length)
        {
            soul = soulTypes[eventIndex];
        }

        bool wasHeavenWarning = moralityManager != null && moralityManager.warningHeaven;
        bool wasHellWarning = moralityManager != null && moralityManager.warningHell;

        if (moralityManager != null && chosenMask.HasValue && chosenDestination.HasValue)
        {
            moralityManager.ProcessVerdict(chosenDestination.Value, chosenMask.Value, soul);
        }
        else
        {
            Debug.LogError($"Cannot process verdict for event index {eventIndex} - missing mask/destination.");
        }

        if (moralityManager != null)
        {
            bool heavenJustTriggered = !wasHeavenWarning && moralityManager.warningHeaven;
            bool hellJustTriggered = !wasHellWarning && moralityManager.warningHell;

            if (heavenJustTriggered)
            {
                yield return StartCoroutine(PlayWarningDialogue(heavensLamentDialogue));
            }

            if (hellJustTriggered)
            {
                yield return StartCoroutine(PlayWarningDialogue(hellsGrumbleDialogue));
            }
        }

        if (nextEvent != null)
        {
            StartCoroutine(nextEvent);
        }
        else
        {
            TriggerGameEnding();
        }
    }

    #region events
    public IEnumerator E01()
    {
        if (soulData != null && soulTypes.Length > 0) soulData.soulType = soulTypes[0];

        yield return new WaitForSeconds(2);
        FadeIn.SetActive(false);
        NPC.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE1.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE01Finished;

        SelectE1.TriggerDialouge();
    }
    void OnE01Finished()
    {
        dialougeManager.onDialougeEnd -= OnE01Finished;
        SelectE1.gameObject.SetActive(false);
        StartCoroutine(RunVerdictThenContinue(0, E02()));
    }

    public IEnumerator E02()
    {
        if (soulData != null && soulTypes.Length > 1) soulData.soulType = soulTypes[1];

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE2.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE02Finished;

        SelectE2.TriggerDialouge();
    }
    void OnE02Finished()
    {
        dialougeManager.onDialougeEnd -= OnE02Finished;
        SelectE2.gameObject.SetActive(false);
        StartCoroutine(RunVerdictThenContinue(1, E03()));
    }

    public IEnumerator E03()
    {
        if (soulData != null && soulTypes.Length > 2) soulData.soulType = soulTypes[2];

        yield return new WaitForSeconds(0.5f);
        NPC.SetActive(false);
        middleMan.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE3.gameObject.SetActive(true);

        dialougeManager.onDialougeEnd += OnE03Finished;
        SelectE3.TriggerDialouge();
    }
    void OnE03Finished()
    {
        dialougeManager.onDialougeEnd -= OnE03Finished;
        SelectE3.gameObject.SetActive(false);
        StartCoroutine(RunVerdictThenContinue(2, E04()));
    }

    public IEnumerator E04()
    {
        if (soulData != null && soulTypes.Length > 3) soulData.soulType = soulTypes[3];

        yield return new WaitForSeconds(0.5f);
        middleMan.SetActive(false);
        NPC.SetActive(true);

        yield return new WaitForSeconds(1);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE4.gameObject.SetActive(true);

        dialougeManager.onDialougeEnd += OnE04Finished;

        SelectE4.TriggerDialouge();
    }
    void OnE04Finished()
    {
        dialougeManager.onDialougeEnd -= OnE04Finished;
        SelectE4.gameObject.SetActive(false);
        StartCoroutine(RunVerdictThenContinue(3, E05()));
    }

    public IEnumerator E05()
    {
        if (soulData != null && soulTypes.Length > 4) soulData.soulType = soulTypes[4];

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE5.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE05Finished;

        SelectE5.TriggerDialouge();
    }
    void OnE05Finished()
    {
        dialougeManager.onDialougeEnd -= OnE05Finished;
        SelectE5.gameObject.SetActive(false);
        StartCoroutine(RunVerdictThenContinue(4, E06()));
    }

    public IEnumerator E06()
    {
        if (soulData != null && soulTypes.Length > 5) soulData.soulType = soulTypes[5];

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE6.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE06Finished;

        SelectE6.TriggerDialouge();
    }
    void OnE06Finished()
    {
        dialougeManager.onDialougeEnd -= OnE06Finished;
        SelectE6.gameObject.SetActive(false);
        StartCoroutine(RunVerdictThenContinue(5, E07()));
    }

    public IEnumerator E07()
    {
        if (soulData != null && soulTypes.Length > 6) soulData.soulType = soulTypes[6];

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE7.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE07Finished;

        SelectE7.TriggerDialouge();
    }
    void OnE07Finished()
    {
        dialougeManager.onDialougeEnd -= OnE07Finished;
        SelectE7.gameObject.SetActive(false);
        // No next event - RunVerdictThenContinue will call TriggerGameEnding()
        StartCoroutine(RunVerdictThenContinue(6, null));
    }

    #endregion

    private void TriggerGameEnding()
    {
        if (moralityManager == null) return;

        MoralityManager.GameEnding ending = moralityManager.EvaluateEnding();

        switch (ending)
        {
            case MoralityManager.GameEnding.DeathHeaven:
                Debug.Log("Ending Triggered: DeathHeaven");
                break;
            case MoralityManager.GameEnding.DeathHell:
                Debug.Log("Ending Triggered: DeathHell");
                break;
            case MoralityManager.GameEnding.IdealEnd:
                Debug.Log("Ending Triggered: IdealEnd");
                break;
            case MoralityManager.GameEnding.PerfectEnd:
                Debug.Log("Ending Triggered: PerfectEnd");
                break;
        }
    }
}