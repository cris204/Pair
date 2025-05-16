using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    
    public void UpdateScore(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }
}