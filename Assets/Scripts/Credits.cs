using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject TextTip;

    void Start()
    {
        StartCoroutine(Tip());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(TransferToMainMenu());
        }
    }
    IEnumerator Tip()
    {
        yield return new WaitForSeconds(5);
        TextTip.SetActive(true);
        yield return new WaitForSeconds(15);
        TextTip.SetActive(false);
    }

    IEnumerator TransferToMainMenu()
    {
        yield return null;
        SceneManager.LoadScene(1);
    }
}
