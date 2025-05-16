using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Managers")]
    public GameBoardManager board;

    public bool canInteract = true;
    private CardController firstCard, secondCard;
    
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
        board.CreateBoard();
        SubscribeToCards();
        totalPairs = (board.gridSize.x * board.gridSize.y) / 2;
        matchedPairs = 0;
    }

    private void SubscribeToCards()
    {
        foreach (var card in board.cards)
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
            firstCard.isMatched = true;
            secondCard.isMatched = true;
            SoundManager.Instance.PlaySound(Env.SOUND_MATCH);
            matchedPairs++;

            firstCard.Hide();
            secondCard.Hide();

            if (matchedPairs == totalPairs)
            {
                GameWon();
            }
        }
        else
        {
            firstCard.isFlipped = false;
            secondCard.isFlipped = false;
            firstCard.ShowBack();
            secondCard.ShowBack();
            SoundManager.Instance.PlaySound(Env.SOUND_FAIL);
        }

        firstCard = null;
        secondCard = null;
        canInteract = true;
    }
    
    private void GameWon()
    {
        SoundManager.Instance.PlaySound(Env.SOUND_WIN);
    }
}
