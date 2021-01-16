using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public CanvasGroup settings, controls, menu;
    public UnityEngine.Audio.AudioMixer mixer;
    public Image NoMusic;
    public Image NoSound;
    void Start()
    {
        OpenMenu();
    }
    public void PlayGame()
    {
        SceneChanger.LoadScene(Scenes.Game);
    }
    public void OpenSettings()
    {
        menu.SetEnabled(false);
        controls.SetEnabled(false);
        settings.SetEnabled(true);
    }
    public void OpenControls()
    {
        menu.SetEnabled(false);
        controls.SetEnabled(true);
        settings.SetEnabled(false);
    }
    public void OpenMenu()
    {
        menu.SetEnabled(true);
        controls.SetEnabled(false);
        settings.SetEnabled(false);
    }
    public void MuteMusic()
    {
        float volume;
        mixer.GetFloat("MusicVolume", out volume);
        if (volume < -70)
        {
            NoMusic.gameObject.SetActive(false);
            mixer.SetFloat("MusicVolume", 0);
        }
        else
        {
            NoMusic.gameObject.SetActive(true);
            mixer.SetFloat("MusicVolume", -80);
        }
    }
    public void MuteSound()
    {
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        if (volume < -70)
        {
            NoSound.gameObject.SetActive(false);
            mixer.SetFloat("MasterVolume", 0);
        }
        else
        {
            NoSound.gameObject.SetActive(true);
            mixer.SetFloat("MasterVolume", -80);
        }
    }
}