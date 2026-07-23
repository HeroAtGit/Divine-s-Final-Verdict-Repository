using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public List<DialougeTrigger> events = new List<DialougeTrigger>();
    public GameObject FadeIn;
    public GameObject NPC;
    public GameObject TextBox;
    //[SerializeField] int eventPos = 0;
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject charName;

    [SerializeField] private DialougeTrigger SelectE1;
    [SerializeField] private DialougeTrigger SelectE2;
    [SerializeField] private DialougeTrigger SelectE3;
    [SerializeField] private DialougeTrigger SelectE4;

    void Update()
    {
       
    }

    void Start()

    {
        StartCoroutine(E01()); 
    }

    public IEnumerator E01()
    {

       yield return new WaitForSeconds(1);
       FadeIn.SetActive(false);
       NPC.SetActive(true);

        // Text function
       
       yield return new WaitForSeconds(1.5f);
       mainTextObject.SetActive(true);
       SelectE1.gameObject.SetActive(true);
       SelectE1.TriggerDialouge();
    }

    public IEnumerator E02()
    {
        // Text function

        yield return new WaitForSeconds(1);
        mainTextObject.SetActive(true);
        SelectE2.gameObject.SetActive(true);
        SelectE2.TriggerDialouge();
    }

}
