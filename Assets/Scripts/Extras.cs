using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Extras : MonoBehaviour
{
    //private int eventPos = 0;
    public GameObject Button;
    void Start()
    {
        //StartCoroutine(P01());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(TransferToMainMenu());
        }
    }
    IEnumerator TransferToMainMenu()
    {
        yield return null;
        SceneManager.LoadScene(1);
    }

    /*public void PageButton(int eventPos)
    {
        buttonSfx.Play();
        fadeOut.SetActive(true);
    }

    IEnumerator P01()
    {

    }

    public void EventsSystem()
    {
        if (pagePos == 2)
        {
            StartCoroutine(P02());
        }
        if (pagePos == 3)
        {
            StartCoroutine(E03());
        }

    }*/
}
