using UnityEngine;
using UnityEngine.UI;

public class MinesCounterScript : MonoBehaviour
{
    public Text minesCounterText;

    /// <summary>
    /// Method that displays number of mines.
    /// </summary>
    /// <param name="minesCounter">Number of mines.</param>
    public void SetMinesCounterText(int minesCounter)
    {
        minesCounterText.text = "Mines: " + minesCounter.ToString();
    }
}
