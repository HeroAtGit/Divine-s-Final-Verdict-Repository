using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class DialougeManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialougeText;
    public GameObject TextBox;
    [SerializeField] int textLength;
    public Queue<string> names;
    public Queue<string> sentences;


    void Start()
    {
        names = new Queue<string>();
        sentences = new Queue<string>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialouge (Dialouge dialouge)
    {
        names.Clear();
        sentences.Clear();

        foreach (string name in dialouge.names)
        {
            names.Enqueue(name);
        }

        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialouge();
            return;
        }

        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeName(name));
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeName(string name)
    {
        nameText.text = "";
        foreach (char letter in name)
        {
            nameText.text += letter;
            yield return null;
        }
    }
    //Text Roll System for dialouge
    IEnumerator TypeSentence (string sentence)
    {
        dialougeText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialougeText.text += letter;
            yield return null;
        }
    }

    public void EndDialouge()
    {
        TextBox.SetActive(false);
        StopAllCoroutines();
        Debug.Log("End of Dialouge");
    }

}
