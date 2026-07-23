using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Extras : MonoBehaviour
{
    public int pagePosi = 0;
    public int pagePrev = 0;
    public GameObject button;
    public AudioSource buttonSfx;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;

    void Start()
    {
        StartCoroutine(P01());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttonSfx.Play();   
            StartCoroutine(TransferToMainMenu());
        }
    }
    IEnumerator TransferToMainMenu()
    {
        yield return null;
        SceneManager.LoadScene(1);
    }

    IEnumerator P01()
    {
        yield return null;
        p1.SetActive(true);
        p2.SetActive(false);
        p3.SetActive(false);
        pagePosi = 1;
        pagePrev = 1;

    }
    IEnumerator P02()
    {
        yield return null;
        p1.SetActive(false);
        p2.SetActive(true);
        p3.SetActive(false);
        pagePosi = 2;
        pagePrev = 2;
    }
    IEnumerator P03()
    {
        yield return null;
        p1.SetActive(false);
        p2.SetActive(false);
        p3.SetActive(true);
        pagePrev = 3;
    }
    public void NextButton()
    {
        buttonSfx.Play();

        if (pagePosi == 1)
        {
            StartCoroutine(P02());
        }
        if (pagePosi == 2)
        {
            StartCoroutine(P03());
        }
    }
    public void PrevButton()
    {
        buttonSfx.Play();

        if (pagePrev == 2)
        {
            StartCoroutine(P01());
        }
        if (pagePrev == 3)
        {
            StartCoroutine(P02());
        }
    }
}
