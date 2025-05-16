using UnityEngine;
using UnityEngine.UI;
using System;

public class CardController : MonoBehaviour
{
    public Button cardButton;
    public Image cardImage;
    public Sprite frontSprite;
    public Sprite backSprite;

    public int cardId;
    public bool isFlipped = false;
    public bool isMatched = false;

    public event Action<CardController> OnCardFlipped;

    void Start()
    {
        cardButton.onClick.AddListener(FlipCard);
        ShowBack();
    }

    public void SetCard(int id, Sprite front)
    {
        cardId = id;
        frontSprite = front;
        ShowBack();
    }

    public void FlipCard()
    {
        if ((isFlipped || isMatched) || !GameManager.Instance.canInteract)
        {
            return;
        }

        isFlipped = true;
        ShowFront();
        OnCardFlipped?.Invoke(this);
    }

    public void ShowFront()
    {
        cardImage.sprite = frontSprite;
    }

    public void ShowBack()
    {
        isFlipped = false;
        cardImage.sprite = backSprite;
    }

    public void MarkMatched()
    {
        isMatched = true;
        cardButton.interactable = false;
    }

    public void ResetCard()
    {
        isFlipped = false;
        isMatched = false;
        cardButton.interactable = true;
        ShowBack();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
