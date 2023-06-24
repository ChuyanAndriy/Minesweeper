using UnityEngine;
using UnityEngine.UI;

public class MinesCounterScript : MonoBehaviour
{
    public static MinesCounterScript instance;

    public Text minesCounterText;

    private void Awake()
    {
        instance = this;
    }

    public void SetMinesCounterText(int minesCounter)
    {
        minesCounterText.text = "Mines: " + minesCounter.ToString();
    }
}
