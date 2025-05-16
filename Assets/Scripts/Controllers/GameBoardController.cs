using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameBoardController : MonoBehaviour
{
    public Transform gridParent;
    public GridLayoutGroup gridLayout;
    public Sprite[] cardFaces;
    public Vector2Int gridSize = new(4, 3);

    public List<CardController> cards = new();

    public void CreateBoard()
    {
        ClearBoard();

        List<int> ids = GenerateCardIds();
        Shuffle(ids);

        StartCoroutine(CreateCardsCoroutine(ids));
    }
    
    public IEnumerator CreateBoardFromSavedState(SavedGameState state)
    {
        ClearBoard();

        gridLayout.enabled = true;

        List<CardController> matchCards = new List<CardController>();

        for (int i = 0; i < state.cards.Count; i++)
        {
            var savedCard = state.cards[i];
            GameObject cardGO = PoolManager.Instance.GetObject(Env.CARD_PATH);
            cardGO.transform.SetParent(gridParent, false);
            cardGO.transform.localScale = Vector3.one;

            CardController card = cardGO.GetComponent<CardController>();
            card.SetCard(savedCard.cardId, cardFaces[savedCard.cardId]);
            card.isMatched = savedCard.isMatched;

            if (savedCard.isMatched)
            {
                matchCards.Add(card);
            }
            else
            {
                card.ShowBack();
            }

            cards.Add(card);
        }

        yield return null; 

        foreach (CardController card in matchCards)
        {
            card.Hide();
        }

        gridLayout.enabled = false;
    }
    
    private IEnumerator CreateCardsCoroutine(List<int> ids)
    {
        gridLayout.enabled = true;

        for (int i = 0; i < ids.Count; i++)
        {
            GameObject cardGO = PoolManager.Instance.GetObject(Env.CARD_PATH);
            cardGO.transform.SetParent(gridParent, false);
            cardGO.transform.localScale = Vector3.one;

            CardController card = cardGO.GetComponent<CardController>();
            card.SetCard(ids[i], cardFaces[ids[i]]);
            cards.Add(card);
        }

        yield return null;

        gridLayout.enabled = false;
    }

    private void ClearBoard()
    {
        foreach (CardController card in cards)
        {
            PoolManager.Instance.ReleaseObject(Env.CARD_PATH, card.gameObject);
            Destroy(card.gameObject);
        }

        cards.Clear();
    }

    private List<int> GenerateCardIds()
    {
        int pairs = (gridSize.x * gridSize.y) / 2;
        var ids = new List<int>();
        for (int i = 0; i < pairs; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }
        return ids;
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
