using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Preloader,
    MainMenu,
    Game
}

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public static void LoadScene(Scenes scene)
    {
        Instance.loadScene(scene);
    }

    public UnityEngine.UI.Image blackout;
    public void Start()
    {
        Instance = this;
        loadScene(Scenes.MainMenu);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(DoLoadIn());
    }
    bool isLoading = false;

    public void loadScene(Scenes scene)
    {
        if (!isLoading)
        {
            isLoading = true;
            StartCoroutine(DoLoadScene(scene));
        }
    }
    IEnumerator DoLoadIn()
    {
        while (blackout.color.a > 0)
        {
            Color c = blackout.color;
            c.a -= 0.02f;
            blackout.color = c;
            yield return null;
        }
        blackout.gameObject.SetActive(false);
        isLoading = false;
    }
    IEnumerator DoLoadScene(Scenes scene)
    {
        blackout.gameObject.SetActive(true);
        float startTime = Time.time;
        while (blackout.color.a < 1)
        {
            Color c = blackout.color;
            c.a += 0.02f;
            blackout.color = c;
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene((int) scene);
    }

}