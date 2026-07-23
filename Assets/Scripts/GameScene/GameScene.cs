using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public List<DialougeTrigger> events = new List<DialougeTrigger>();

    //Game Objects
    public GameObject FadeIn;
    public GameObject NPC;
    public GameObject StatueBG;
    public GameObject TextBox;
    [SerializeField] GameObject mainTextObject;

    //Events
    [SerializeField] private DialougeTrigger SelectE1;
    [SerializeField] private DialougeTrigger SelectE2;
    [SerializeField] private DialougeTrigger SelectE3;
    [SerializeField] private DialougeTrigger SelectE4;
    [SerializeField] private DialougeTrigger SelectE5;

    [SerializeField] private DialougeManager dialougeManager;

    void Start()
    {
        if (dialougeManager == null)
        {
            dialougeManager = FindAnyObjectByType<DialougeManager>(FindObjectsInactive.Include);
        }
    
        StartCoroutine(E01());
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
        SelectE4.gameObject.SetActive(false);
        //StartCoroutine(E06());//
    }


    #endregion

}

