using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    public void PauseGame(bool doPause)
    {
        Time.timeScale = doPause ? 0 : 1;
    }
    public void Resume()
    {
        Game.Instance.Pause();
    }
    public void Restart()
    {
        SceneChanger.LoadScene(Scenes.Game);
    }
    public void Menu()
    {
        SceneChanger.LoadScene(Scenes.MainMenu);
    }
}