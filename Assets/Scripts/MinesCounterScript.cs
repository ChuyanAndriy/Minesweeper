using UnityEngine;
using UnityEngine.UI;

public class MinesCounterScript : MonoBehaviour
{
    public Text minesCounterText;

    public void SetMinesCounterText(int minesCounter)
    {
        minesCounterText.text = "Mines: " + minesCounter.ToString();
    }
}
