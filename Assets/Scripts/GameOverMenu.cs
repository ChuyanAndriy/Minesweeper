using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public static GameOverMenu instance;

    public GameObject gameOverMenuUI;

    public Text gameResult;

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

    public void GameOverHide()
    {
        gameOverMenuUI.SetActive(false);
    }

    public void ExitGame()
    {
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(1);
    }

    private void Awake()
    {
        instance = this;
    }
}
