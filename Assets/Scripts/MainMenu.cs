using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    void Start()
    {
        
    }

    public void StartGame()
    {
        fadeOut.SetActive(true);
        StartCoroutine(TransferToGame());
    }

    public void OpenSettings()
    {
        fadeOut.SetActive(true);
        StartCoroutine(TransferToSettings());
    }
    public void OpenCredits()
    {
        fadeOut.SetActive(true);
        StartCoroutine(TransferToCredits());
    }
    public void OpenExtras()
    {
        fadeOut.SetActive(true);
        StartCoroutine(TransferToExtras());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TransferToGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(5);
    }
    IEnumerator TransferToSettings()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }
    IEnumerator TransferToCredits()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(3);
    }

    IEnumerator TransferToExtras()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(4);
    }
}
