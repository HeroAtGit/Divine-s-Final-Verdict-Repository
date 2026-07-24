using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public List<DialougeTrigger> events = new List<DialougeTrigger>();

    //Game Objects
    public GameObject bg;
    public GameObject TextBox;
    public GameObject fadeIn;
    public GameObject NPCTable;
    public GameObject MMPodium;
    public GameObject NPC;
    public GameObject statueBG;
    public GameObject blackBG;
    public GameObject middleMan;
    public GameObject MMJustice;
    public GameObject MMJudgement;
    public GameObject JusticeMask;
    public GameObject JudgementMask;
    public GameObject maskButtons;
    public GameObject destinationButtons;
    public GameObject endBG;
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
    [SerializeField] private DialougeTrigger SelectE8;
    [SerializeField] private DialougeTrigger SelectE9;
    [SerializeField] private DialougeTrigger SelectE10;
    [SerializeField] private DialougeTrigger SelectE11;
    [SerializeField] private DialougeTrigger SelectE12;
    [SerializeField] private DialougeTrigger SelectE13;
    [SerializeField] private DialougeTrigger SelectE14;
    [SerializeField] private DialougeTrigger SelectE15;
    [SerializeField] private DialougeTrigger SelectE16;
    [SerializeField] private DialougeTrigger SelectE17;
    [SerializeField] private DialougeTrigger SelectE18;
    [SerializeField] private DialougeTrigger SelectE19;
    [SerializeField] private DialougeTrigger SelectE20;

    #region Systems
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
                
            }
        }

        if (moralityManager != null)
        {
            moralityManager.onWarningTriggered += HandleWarningTriggered;
            moralityManager.onWarningFaded += HandleWarningFaded;
        }

        if (maskButtons != null) maskButtons.SetActive(false);
        if (destinationButtons != null) destinationButtons.SetActive(false);

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
        maskButtons.SetActive(true);
        Debug.Log("Waiting for mask choice...");
        yield return new WaitUntil(() => chosenMask != null);
        Debug.Log($"Mask choice received: {chosenMask.Value}");
        maskButtons.SetActive(false);
    }

    IEnumerator WaitForDestinationChoice()
    {
        chosenDestination = null;
        destinationButtons.SetActive(true);
        Debug.Log("Waiting for destination choice...");
        yield return new WaitUntil(() => chosenDestination != null);
        Debug.Log($"Destination choice received: {chosenDestination.Value}");
        destinationButtons.SetActive(false);
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
    #endregion

    #region events
    public IEnumerator E01()
    {
        yield return null;
        statueBG.SetActive(true);
        blackBG.SetActive(true);
        yield return new WaitForSeconds(2);
        fadeIn.SetActive(false);

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
        StartCoroutine(E02());
    }

    public IEnumerator E02()
    {
        yield return null;
        fadeIn.SetActive(true);
        statueBG.SetActive(false);
        blackBG.SetActive(false);
        bg.SetActive(true);
        NPCTable.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        fadeIn.SetActive(false);
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
        StartCoroutine( E03());
    }

    public IEnumerator E03()
    {
        yield return new WaitForSeconds(0);
        JusticeMask.SetActive(true);

        yield return new WaitForSeconds(1);
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
        StartCoroutine(E04());
    }

    public IEnumerator E04()
    {
        yield return new WaitForSeconds(0);
        JusticeMask.SetActive(false);
        JudgementMask.SetActive(true);

        yield return new WaitForSeconds(0.5f);
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
        StartCoroutine(E05());
    }

    public IEnumerator E05()
    {
        yield return null;
        JudgementMask.SetActive(false);

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
        StartCoroutine(E06());
    }

    public IEnumerator E06()
    {
        //Case
        yield return null;
        NPC.SetActive(true);

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
        StartCoroutine(E07());
    }

    public IEnumerator E07()
    {
        yield return null;
        NPC.SetActive(false);
        bg.SetActive(false);
        NPCTable.SetActive(false);
        blackBG.SetActive(true);
        middleMan.SetActive(true);
        maskButtons.SetActive(true);
        MMPodium.SetActive(true);

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
        StartCoroutine(E08());
    }

    public IEnumerator E08()
    {
        //Case
        yield return null;
        blackBG.SetActive(false);
        middleMan.SetActive(false);
        maskButtons.SetActive(false);
        MMPodium.SetActive(false);
        NPC.SetActive(true);
        bg.SetActive(true);
        NPCTable.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE8.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE08Finished;

        SelectE8.TriggerDialouge();
    }
    void OnE08Finished()
    {
        dialougeManager.onDialougeEnd -= OnE08Finished;
        SelectE8.gameObject.SetActive(false);
        StartCoroutine(E09());
    }

    public IEnumerator E09()
    {
        yield return null;
        NPC.SetActive(false);
        bg.SetActive(false);
        NPCTable.SetActive(false);
        blackBG.SetActive(true);
        middleMan.SetActive(true);
        maskButtons.SetActive(true);
        MMPodium.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE9.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE09Finished;

        SelectE9.TriggerDialouge();
    }
    void OnE09Finished()
    {
        dialougeManager.onDialougeEnd -= OnE09Finished;
        SelectE9.gameObject.SetActive(false);
        StartCoroutine(E10());
    }

    public IEnumerator E10()
    {
        //Case
        yield return null;
        blackBG.SetActive(false);
        middleMan.SetActive(false);
        maskButtons.SetActive(false);
        MMPodium.SetActive(false);
        NPC.SetActive(true);
        bg.SetActive(true);
        NPCTable.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE10.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE10Finished;

        SelectE10.TriggerDialouge();
    }
    void OnE10Finished()
    {
        dialougeManager.onDialougeEnd -= OnE10Finished;
        SelectE10.gameObject.SetActive(false);
        StartCoroutine(E11());
    }

    public IEnumerator E11()
    {
        //Masks
        yield return null;
        NPC.SetActive(false);
        bg.SetActive(false);
        NPCTable.SetActive(false);
        blackBG.SetActive(true);
        middleMan.SetActive(true);
        maskButtons.SetActive(true);
        MMPodium.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE11.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE11Finished;

        SelectE11.TriggerDialouge();
    }
    void OnE11Finished()
    {
        dialougeManager.onDialougeEnd -= OnE11Finished;
        SelectE11.gameObject.SetActive(false);
        StartCoroutine(E12());
    }

    public IEnumerator E12()
    {
        //Case
        yield return null;
        blackBG.SetActive(false);
        middleMan.SetActive(false);
        maskButtons.SetActive(false);
        MMPodium.SetActive(false);
        NPC.SetActive(true);
        bg.SetActive(true);
        NPCTable.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE12.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE12Finished;

        SelectE12.TriggerDialouge();
    }
    void OnE12Finished()
    {
        dialougeManager.onDialougeEnd -= OnE12Finished;
        SelectE12.gameObject.SetActive(false);
        StartCoroutine(E13());
    }

    public IEnumerator E13()
    {
        //Masks
        yield return null;
        NPC.SetActive(false);
        bg.SetActive(false);
        NPCTable.SetActive(false);
        blackBG.SetActive(true);
        middleMan.SetActive(true);
        maskButtons.SetActive(true);
        MMPodium.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE13.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE13Finished;

        SelectE13.TriggerDialouge();
    }
    void OnE13Finished()
    {
        dialougeManager.onDialougeEnd -= OnE13Finished;
        SelectE13.gameObject.SetActive(false);
        StartCoroutine(E14());
    }

    public IEnumerator E14()
    {
        //Case
        yield return null;
        blackBG.SetActive(false);
        middleMan.SetActive(false);
        maskButtons.SetActive(false);
        MMPodium.SetActive(false);
        NPC.SetActive(true);
        bg.SetActive(true);
        NPCTable.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE14.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE14Finished;

        SelectE14.TriggerDialouge();
    }
    void OnE14Finished()
    {
        dialougeManager.onDialougeEnd -= OnE14Finished;
        SelectE14.gameObject.SetActive(false);
        StartCoroutine(E15());
    }

    public IEnumerator E15()
    {
        //Masks
        yield return null;
        NPC.SetActive(false);
        bg.SetActive(false);
        NPCTable.SetActive(false);
        blackBG.SetActive(true);
        middleMan.SetActive(true);
        maskButtons.SetActive(true);
        MMPodium.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE15.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE15Finished;

        SelectE15.TriggerDialouge();
    }
    void OnE15Finished()
    {
        dialougeManager.onDialougeEnd -= OnE15Finished;
        SelectE15.gameObject.SetActive(false);
        StartCoroutine(E16());
    }

    public IEnumerator E16()
    {
        //Case
        yield return null;
        blackBG.SetActive(false);
        middleMan.SetActive(false);
        maskButtons.SetActive(false);
        MMPodium.SetActive(false);
        NPC.SetActive(true);
        bg.SetActive(true);
        NPCTable.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE16.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE16Finished;

        SelectE16.TriggerDialouge();
    }
    void OnE16Finished()
    {
        dialougeManager.onDialougeEnd -= OnE16Finished;
        SelectE16.gameObject.SetActive(false);
        StartCoroutine(E17());
    }

    public IEnumerator E17()
    {
        //Masks
        yield return null;
        NPC.SetActive(false);
        bg.SetActive(false);
        NPCTable.SetActive(false);
        blackBG.SetActive(true);
        middleMan.SetActive(true);
        maskButtons.SetActive(true);
        MMPodium.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE17.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE17Finished;

        SelectE17.TriggerDialouge();
    }
    void OnE17Finished()
    {
        dialougeManager.onDialougeEnd -= OnE17Finished;
        SelectE17.gameObject.SetActive(false);
        StartCoroutine(E18());
    }

    public IEnumerator E18()
    {
        //Case
        yield return null;
        blackBG.SetActive(false);
        middleMan.SetActive(false);
        maskButtons.SetActive(false);
        MMPodium.SetActive(false);
        NPC.SetActive(true);
        bg.SetActive(true);
        NPCTable.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE18.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE18Finished;

        SelectE18.TriggerDialouge();
    }
    void OnE18Finished()
    {
        dialougeManager.onDialougeEnd -= OnE18Finished;
        SelectE18.gameObject.SetActive(false);
        StartCoroutine(E19());
    }

    public IEnumerator E19()
    {
        //Masks
        yield return null;
        NPC.SetActive(false);
        bg.SetActive(false);
        NPCTable.SetActive(false);
        blackBG.SetActive(true);
        middleMan.SetActive(true);
        maskButtons.SetActive(true);
        MMPodium.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE19.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE19Finished;

        SelectE19.TriggerDialouge();
    }
    void OnE19Finished()
    {
        dialougeManager.onDialougeEnd -= OnE19Finished;
        SelectE19.gameObject.SetActive(false);
        StartCoroutine(E20());
    }
    public IEnumerator E20()
    {
        yield return null;
        middleMan.SetActive(false);
        maskButtons.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        mainTextObject.SetActive(true);
        TextBox.SetActive(true);
        SelectE20.gameObject.SetActive(true);
        dialougeManager.onDialougeEnd += OnE20Finished;

        SelectE20.TriggerDialouge();
    }
    void OnE20Finished()
    {
        dialougeManager.onDialougeEnd -= OnE20Finished;
        SelectE20.gameObject.SetActive(false);
        StartCoroutine(TransferToCredits());
    }
    #endregion
    IEnumerator TransferToCredits()
    {
        yield return null;
        SceneManager.LoadScene(3);
    }


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