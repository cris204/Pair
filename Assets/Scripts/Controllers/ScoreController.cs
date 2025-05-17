using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int Score { get; private set; }
    
    [SerializeField]
    private ScoreView view;

    public event System.Action<int> OnScoreChanged;

    private void Awake()
    {
        OnScoreChanged += view.UpdateScore;
    }
    
    public void AddMatchPoints(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
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