using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    /// <summary>
    /// Method that changes game settings.
    /// </summary>
    /// <param name="index">Game difficulty index.</param>
    public void SetDifficulty(int index)
    {
        switch (index)
        {
            case 0:
                Settings.MinesCounter = 10;
                Settings.Width = 9;
                Settings.Height = 9;
                break;
            case 1:
                Settings.MinesCounter = 40;
                Settings.Width = 16;
                Settings.Height = 16;
                break;
            case 2:
                Settings.MinesCounter = 99;
                Settings.Width = 16;
                Settings.Height = 30;
                break;
            default:
                Settings.MinesCounter = 10;
                Settings.Width = 9;
                Settings.Height = 9;
                break;
        }
    }
}
