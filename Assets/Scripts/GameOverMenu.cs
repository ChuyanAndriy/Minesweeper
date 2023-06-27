using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;

    public Text gameResult;

    /// <summary>
    /// Method that shows game over menu.
    /// </summary>
    /// <param name="isGameWin">Is the game won?</param>
    public void GameOverShow(bool isGameWin)
    {
        if (isGameWin)
        {
            gameResult.text = "YOU WIN!";
        }
        else
        {
            gameResult.text = "YOU LOSE!";
        }

        gameOverMenuUI.SetActive(true);
    }

    /// <summary>
    /// Method that hides game over menu.
    /// </summary>
    public void GameOverHide()
    {
        gameOverMenuUI.SetActive(false);
    }

    /// <summary>
    /// Method for exiting game.
    /// </summary>
    public void ExitGame()
    {
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Method for restarting game.
    /// </summary>
    public void RestartGame()
    {
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
