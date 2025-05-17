using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Controllers")]
    public GameBoardController board;
    public ScoreController scoreController;
    public MainMenuUiView mainMenuUi;
    public ResultUiView resultUiView;

    public bool canInteract = true;
    public bool hasGameInProgress = false;
    
    private CardController firstCard, secondCard;
    
    private bool isRunning = false;
    private int totalPairs = 0;
    private int matchedPairs = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        hasGameInProgress = LoadGame();
        mainMenuUi.Init();
        resultUiView.Init(NewGame);
    }

    public void NewGame()
    {
        SaveSystem.DeleteSave();
        board.CreateBoard();
        SubscribeToCards();
        totalPairs = board.cards.Count / 2;
        matchedPairs = 0;
        scoreController.Reset();
        isRunning = true;
    }
    
    public void StartFromLoadState()
    {
        SavedGameState state = SaveSystem.currentState;
        board.gridSize = new Vector2Int(state.gridWidth, state.gridHeight);
        totalPairs = state.totalPairs;
        matchedPairs = state.matchedPairs;
        board.CreateBoardFromSavedState(state);
        scoreController.AddMatchPoints(state.score);
        SubscribeToCards();
        matchedPairs = state.matchedPairs;
        isRunning = true;
    }

    private void SubscribeToCards()
    {
        foreach (CardController card in board.cards)
        {
            card.OnCardFlipped += HandleCardFlipped;
        }
    }

    private void HandleCardFlipped(CardController card)
    {
        if (!canInteract || card == firstCard)
        {
            return;
        }

        if (firstCard == null)
        {
            firstCard = card;
            SoundManager.Instance.PlaySound(Env.SOUND_FLIP);
        }
        else
        {
            secondCard = card;
            canInteract = false;
            SoundManager.Instance.PlaySound(Env.SOUND_FLIP);
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.25f);

        if (firstCard.cardId == secondCard.cardId)
        {
            firstCard.MarkMatched();
            secondCard.MarkMatched();
            SoundManager.Instance.PlaySound(Env.SOUND_MATCH);
            scoreController.AddMatchPoints(10);
            matchedPairs++;

            if (matchedPairs == totalPairs)
            {
                GameWon();
            }
        }
        else
        {
            firstCard.isFlipped = false;
            secondCard.isFlipped = false;
            firstCard.FlipBack();
            secondCard.FlipBack();
            SoundManager.Instance.PlaySound(Env.SOUND_FAIL);
        }

        firstCard = null;
        secondCard = null;
        canInteract = true;
    }
    
    private void GameWon()
    {
        SoundManager.Instance.PlaySound(Env.SOUND_WIN);
        SaveGame();
        resultUiView.GameOver(scoreController.Score);
    }

    private void OnApplicationQuit()
    {
        if (isRunning)
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        SavedGameState state = new SavedGameState
        {
            score = scoreController.Score,
            gridWidth = board.gridSize.x,
            gridHeight = board.gridSize.y,
            matchedPairs = matchedPairs,
            totalPairs = totalPairs
        };

        foreach (var card in board.cards)
        {
            state.cards.Add(new SavedCard
            {
                cardId = card.cardId,
                isMatched = card.isMatched
            });
        }

        SaveSystem.SaveGame(state);
    }
    
    private bool LoadGame()
    {
        SavedGameState state = SaveSystem.LoadGame();
        if (state == null || state.matchedPairs == state.totalPairs)
        {
            return false;
        }

        return true;
    }


    [ContextMenu("DELETE")]
    public void delete()
    {
        SaveSystem.DeleteSave();
    }
}
