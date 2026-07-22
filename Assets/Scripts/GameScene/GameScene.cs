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


    /*#region"OldTextSystem"
    IEnumerator E01()
    {
        nextButton.SetActive(false);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "The Soul";
        textToSpeak = "Uhm.. Helloe.. >_< Nice to see u, I'd like to tell my story!! I-if you don't mind..";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.05f);
        nextButton.SetActive(true);
        eventPos = 2;
        
    }

    IEnumerator E02()
    {
        nextButton.SetActive(false);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "The MM.";
        textToSpeak = "Go on.";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.05f);
        nextButton.SetActive(true);
        eventPos = 3;
    }

    IEnumerator E03()
    {
        nextButton.SetActive(false);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "The Soul";
        textToSpeak = "It all began in 1987, It was quite the year to be honest..,";
        textToSpeak = "Id hate it to happen again to me all of those things";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.05f);
        nextButton.SetActive(true);
        eventPos = 4;
    }

    IEnumerator E04()
    {
        nextButton.SetActive(false);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "The Soul";
        textToSpeak = "Blah-blablah! Blah, Blah blah. Blablablah. Blabla? BLALBBLABLBALBLABLA!, Blabla. Blablabalbala. blablabla.";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.05f);
        nextButton.SetActive(true);
        eventPos = 5;
    }
    IEnumerator E05()
    {
        nextButton.SetActive(false);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "The MM.";
        textToSpeak = "Ive heard enough, it is time for your Trial..";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.05f);
        nextButton.SetActive(true);
        eventPos = 6;
    }

    IEnumerator E06()
    {
        nextButton.SetActive(false);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "???";
        textToSpeak = "The time is now for the trials.. You will choose either the path of Justice, or.. the path of Judgement..";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.05f);
        nextButton.SetActive(true);
        eventPos = 7;
    }
    #endregion*/

    /*public void EventsSystem()
    {
        if (eventPos == 2)
        {
            StartCoroutine(E02());
        }
        if (eventPos == 3)
        {
            StartCoroutine(E03());
        }
       
    }*/
}
