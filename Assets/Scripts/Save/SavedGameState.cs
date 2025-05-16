using System.Collections.Generic;

[System.Serializable]
public class SavedGameState
{
    public int score;
    public int gridWidth;
    public int gridHeight;
    public int matchedPairs;
    public int totalPairs;
    public List<SavedCard> cards = new();
}

[System.Serializable]
public class SavedCard
{
    public int cardId;
    public bool isMatched;
}
