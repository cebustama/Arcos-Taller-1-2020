using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool gamePaused = false;

    public GameObject pauseMenu;

    public int score = 0;

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }

        public void PauseGame()
        {
            if (!gamePaused)
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                gamePaused = true;
            }
            else
            {
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
                gamePaused = false;
            }
        }

    private void Awake()
    {
        score = PlayerPrefs.GetInt("Score");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("Score", 0);
    }

    public void CheckScore(int newScore)
    {
        if (newScore > PlayerPrefs.GetInt("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", newScore);
        }
    }
}


