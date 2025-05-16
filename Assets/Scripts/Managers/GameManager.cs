using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameBoardManager board;

    private CardController firstCard, secondCard;
    private bool canInteract = true;

    void Start()
    {
        board.CreateBoard();
        SubscribeToCards();
    }

    void SubscribeToCards()
    {
        foreach (var card in board.GetComponentsInChildren<CardController>())
            card.OnCardFlipped += HandleCardFlipped;
    }

    void HandleCardFlipped(CardController card)
    {
        if (!canInteract || card == firstCard) return;

        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            canInteract = false;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstCard.cardId == secondCard.cardId)
        {
            firstCard.isMatched = true;
            secondCard.isMatched = true;
        }
        else
        {
            firstCard.isFlipped = false;
            secondCard.isFlipped = false;
            firstCard.ShowBack();
            secondCard.ShowBack();
        }

        firstCard = null;
        secondCard = null;
        canInteract = true;
    }
}
