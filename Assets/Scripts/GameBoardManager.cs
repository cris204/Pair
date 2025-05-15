using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform gridParent;
    public Sprite[] cardFaces;
    public Sprite cardBack;
    public Vector2Int gridSize = new(4, 3);

    public List<CardController> cards = new();

    private void Start()
    {
        CreateBoard();
    }

    public void CreateBoard()
    {
        ClearBoard();

        List<int> ids = GenerateCardIds();
        Shuffle(ids);

        for (int i = 0; i < ids.Count; i++)
        {
            GameObject go = Instantiate(cardPrefab, gridParent);
            var card = go.GetComponent<CardController>();
            card.SetCard(ids[i], cardFaces[ids[i]]);
            card.backSprite = cardBack;
            cards.Add(card);
        }
    }

    private void ClearBoard()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);
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
