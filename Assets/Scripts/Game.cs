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
        Score = 0;
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
            if (Controls.Pause)
            {
                Pause();
            }
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