using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameBoardController : MonoBehaviour
{
    public Vector2Int gridSize = new(4, 3);
    public List<CardController> cards = new();
    
    [SerializeField]
    private Transform gridParent;
    [SerializeField]
    private GridLayoutGroup gridLayout;
    [SerializeField]
    private Sprite[] cardFaces;
    
    public void CreateBoard()
    {
        ClearBoard();

        List<int> ids = GenerateCardIds();
        Shuffle(ids);

        CreateCardsCoroutine(ids);
    }
    
    public void CreateBoardFromSavedState(SavedGameState state)
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

        foreach (CardController card in matchCards)
        {
            card.Hide();
        }

        StartAdjustingGridLayout();
    }
    
    private void CreateCardsCoroutine(List<int> ids)
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
        
        StartAdjustingGridLayout();
    }
    
    public IEnumerator RevealAllThenHide(float showTime = 0.5f, float flipDelay = 0.05f)
    {
        foreach (var card in cards)
        {
            card.ShowFront();
            card.isFlipped = false;
        }

        yield return new WaitForSeconds(showTime);

        foreach (var card in cards)
        {
            card.FlipBack();
            yield return new WaitForSeconds(flipDelay);
        }
    }

    private void ClearBoard()
    {
        foreach (CardController card in cards)
        {
            PoolManager.Instance.ReleaseObject(Env.CARD_PATH, card.gameObject);
            card.ResetCard();
        }

        cards.Clear();
    }

    private List<int> GenerateCardIds()
    {
        int totalCards = gridSize.x * gridSize.y;
        int requestedPairs = totalCards / 2;
        int availablePairs = Mathf.Min(requestedPairs, cardFaces.Length);

        if (requestedPairs > cardFaces.Length)
        {
            Debug.LogWarning($"Not enough card faces! Needed {requestedPairs}, but only {cardFaces.Length} provided. Using {availablePairs} pairs.");
        }

        var ids = new List<int>();
        for (int i = 0; i < availablePairs; i++)
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
    
    public void StartAdjustingGridLayout()
    {
        StartCoroutine(AdjustGridLayout());
    }

    private IEnumerator AdjustGridLayout()
    {
        RectTransform gridRect = gridLayout.GetComponent<RectTransform>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gridSize.x;

        float width = gridRect.rect.width;
        float height = gridRect.rect.height;

        int cols = gridSize.x;
        int rows = gridSize.y;

        float spacingX = gridLayout.spacing.x;
        float spacingY = gridLayout.spacing.y;

        float totalSpacingX = spacingX * (cols - 1);
        float totalSpacingY = spacingY * (rows - 1);

        float cellWidth = (width - totalSpacingX) / cols;
        float cellHeight = (height - totalSpacingY) / rows;

        float size = Mathf.Min(cellWidth, cellHeight);

        gridLayout.cellSize = new Vector2(size, size);

        yield return null;

        gridLayout.enabled = false;
    }
}
