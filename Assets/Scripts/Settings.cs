using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    void Start()
    {

    }

    public void StartGame()
    {
        fadeOut.SetActive(true);
        StartCoroutine(TransferToClassScene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TransferToClassScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(4);
    }
}
