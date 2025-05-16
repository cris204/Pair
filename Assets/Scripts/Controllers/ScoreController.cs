using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public ScoreView view;

    public int Score { get; private set; }
    
    public event System.Action<int> OnScoreChanged;

    private void Awake()
    {
        OnScoreChanged += view.UpdateScore;
    }

    private void Start()
    {
        Reset();
    }

    public void AddMatchPoints(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }

    public int GetScore()
    {
        return Score;
    }
    
    public void Reset()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    private void OnDestroy()
    {
        OnScoreChanged -= view.UpdateScore;
    }
}