using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainAudioMixer;
    void Start()
    {

    }

    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterParam", masterVol.value);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicParam", musicVol.value);
    }
    public void ChangeSfxVolume()
    {
        mainAudioMixer.SetFloat("SFXParam", sfxVol.value);
    }
    public void ExitButton()
    {
        StartCoroutine(TransferToMainMenu());
    }

    IEnumerator TransferToMainMenu()
    {
        yield return null;
        SceneManager.LoadScene(1);
    }
}
