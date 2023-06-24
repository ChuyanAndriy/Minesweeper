using UnityEngine;
using UnityEngine.UI;

public class MinesCounterScript : MonoBehaviour
{
    public static MinesCounterScript instance;

    public Text minesCounterText;

    public void SetMinesCounterText(int minesCounter)
    {
        minesCounterText.text = "Mines: " + minesCounter.ToString();
    }

    private void Awake()
    {
        instance = this;
    }
}
