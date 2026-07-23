using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject TextTip;
    [SerializeField] GameObject TitleCard;

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
        yield return new WaitForSeconds(55);
        TextTip.SetActive(true);
        TitleCard.SetActive(true);
    }

    IEnumerator TransferToMainMenu()
    {
        yield return null;
        SceneManager.LoadScene(1);
    }
}
