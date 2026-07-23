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

    [SerializeField] private DialougeManager dialougeManager;
    [SerializeField] private MoralityManager moralityManager;

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

        // Subscribe to warning events
        if (moralityManager != null)
        {
            moralityManager.onWarningTriggered += HandleWarningTriggered;
            moralityManager.onWarningFaded += HandleWarningFaded;
        }
        StartCoroutine(E01());
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
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
            if (heavensLamentDialogue != null) heavensLamentDialogue.TriggerDialouge();
            Debug.Log("[WARNING] Heaven's Lament Triggered!");
        }
        else if (warningType == MoralityManager.WarningType.HellsGrumble)
        {
            if (hellWarningBG != null) hellWarningBG.SetActive(true);
            if (hellsGrumbleDialogue != null) hellsGrumbleDialogue.TriggerDialouge();
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

    #region events
    public IEnumerator E01()
    {
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
        StartCoroutine(E02());
    }

    public IEnumerator E02()
    {
        yield return new WaitForSeconds(1);
        FadeIn.SetActive(false);
        NPC.SetActive(true);

        yield return new WaitForSeconds(1.5f);
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
        StartCoroutine(E03());
    }

    public IEnumerator E03()
    {
        yield return new WaitForSeconds(1);
        NPC.SetActive(true);

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
        StartCoroutine(E04());
    }

    public IEnumerator E04()
    {
        yield return new WaitForSeconds(1);
        FadeIn.SetActive(false);
        NPC.SetActive(true);

        yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1);
        FadeIn.SetActive(false);
        NPC.SetActive(true);

        yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1);
        FadeIn.SetActive(false);
        NPC.SetActive(true);

        yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1);
        FadeIn.SetActive(false);
        NPC.SetActive(true);

        yield return new WaitForSeconds(1.5f);
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

        // --- TRIGGER ENDING AFTER E07 COMPLETES ---
        TriggerGameEnding();
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