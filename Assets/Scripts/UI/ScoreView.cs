using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;
    
    public void UpdateScore(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }
    
    public void UpdateMoves(int newMoves)
    {
        movesText.text = $"Turns: {newMoves}";
    }
}