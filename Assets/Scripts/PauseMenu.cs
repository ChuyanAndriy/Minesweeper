using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused = false;

    public GameObject pauseMenuUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void QuitGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }
}
