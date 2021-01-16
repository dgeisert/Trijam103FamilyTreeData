using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private ScoreScreen scoreScreen;
    [SerializeField] private InGameUI inGameUI;
    public bool active = true;
    public static float Score
    {
        get
        {
            if (Instance)
            {
                return Instance.score;
            }
            return -1f;
        }
        set
        {
            if (Instance)
            {
                Instance.score = value;
                Instance.inGameUI.UpdateScore(value);
            }
        }
    }
    private float score;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        scoreScreen.gameObject.SetActive(false);
        pauseMenu.canvas.enabled = false;
        inGameUI.canvas.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Controls.Shake)
            {
                Camera.main.transform.Shake(0.5f, 3);
                Flytext.CreateFlytext(Vector3.up, score.ToString("#"), Color.white, 6, 1.5f, 1);
                GameObject.FindObjectOfType<FillBar>().UpdateValue(100 * (Random.value - 0.6f));
            }
            if (Controls.Next)
            {
                Victory();
            }
            if (Controls.Pause)
            {
                Pause();
            }
            Score += Time.deltaTime * Random.value * 100;
        }
    }

    public void Pause()
    {
        pauseMenu.canvas.enabled = !pauseMenu.canvas.enabled;
        pauseMenu.PauseGame(pauseMenu.canvas.enabled);
    }

    public void Victory()
    {
        inGameUI.EndGame(true);
        scoreScreen.EndGame(true);
        pauseMenu.canvas.enabled = false;
    }
}