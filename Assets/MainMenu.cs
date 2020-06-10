using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource sfxSource;

    public void StartGame()
    {
        sfxSource.Play();
        Invoke("LoadLevel", 1f);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
