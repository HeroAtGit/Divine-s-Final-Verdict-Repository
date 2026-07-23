using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TransferToMainMenu());
    }

    IEnumerator TransferToMainMenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
