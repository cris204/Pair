using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int Score { get; private set; }
    public int Turns { get; private set; }
    
    [SerializeField]
    private ScoreView view;

    public event System.Action<int> OnScoreChanged;
    public event System.Action<int> OnMove;

    private void Awake()
    {
        OnScoreChanged += view.UpdateScore;
        OnMove += view.UpdateMoves;
    }
    
    public void AddMatchPoints(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }
    
    public void AddMoves(int turns)
    {
        Turns += turns;
        OnMove?.Invoke(Turns);
    }

    public void Reset()
    {
        Score = 0;
        Turns = 0;
        OnScoreChanged?.Invoke(Score);
        OnMove?.Invoke(Turns);
    }

    private void OnDestroy()
    {
        OnScoreChanged -= view.UpdateScore;
        OnMove -= view.UpdateMoves;
    }
}