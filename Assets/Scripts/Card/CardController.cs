using UnityEngine;
using UnityEngine.UI;
using System;

public class CardController : MonoBehaviour
{
    public int cardId;
    public bool isFlipped = false;
    public bool isMatched = false;
    
    [SerializeField]
    private Button cardButton;
    [SerializeField]
    private Image cardImage;
    [SerializeField]
    private Sprite frontSprite;
    [SerializeField]
    private Sprite backSprite;

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
        Hide();
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
