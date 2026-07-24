using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class DialougeManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialougeText;
    public GameObject TextBox;
    public Queue<string> names;
    public Queue<string> sentences;


    public event System.Action onDialougeEnd;
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

    public void StartDialouge(Dialouge dialouge)
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

    //Text Roll System for dialouge
    IEnumerator TypeName(string name)
    {
        nameText.text = "";
        foreach (char letter in name)
        {
            nameText.text += letter;
            yield return null;
        }
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialougeText.text = "";
        foreach (char letter in sentence)
        {
            dialougeText.text += letter;
            yield return null;
        }

    }
    public void EndDialouge()
    {
        TextBox.SetActive(false);
        Debug.Log("End of Dialouge");
        onDialougeEnd?.Invoke();
    }

}
