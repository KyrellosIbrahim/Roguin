using UnityEngine;
using TMPro;

public class MoveCounter : MonoBehaviour
{
    public int moveCount = 0;
    public TextMeshProUGUI movesText;

    void Start()
    {
        UpdateUI();
    }

    public void AddMove()
    {
        moveCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        movesText.text = "Moves: " + moveCount;
    }
}