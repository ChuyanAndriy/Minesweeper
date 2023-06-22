using UnityEngine;

public class SettingsMenu : MonoBehaviour 
{
    public void SetDifficulty(int sizeIndex) 
    {
        switch (sizeIndex) 
        {
            case 0: 
                Settings.mineCounter = 10;
                Settings.width = 9;
                Settings.height = 9;
                break;
            case 1:
                Settings.mineCounter = 40;
                Settings.width = 16;
                Settings.height = 16;
                break;
            case 2:
                Settings.mineCounter = 99;
                Settings.width = 16;
                Settings.height = 30;
                break;
            default:
                Settings.mineCounter = 10;
                Settings.width = 9;
                Settings.height = 9;
                break;
        }
    }
}
