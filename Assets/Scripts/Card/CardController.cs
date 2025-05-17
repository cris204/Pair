using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

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
        AnimateFlip(true);
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

    public void FlipBack()
    {
        isFlipped = false;
        AnimateFlip(false);
    }
    
    private void AnimateFlip(bool showFront)
    {
        float duration = 0.25f;
        
        Sequence flip = DOTween.Sequence();
        
        flip.Append(transform.DOScaleX(0f, duration / 2).SetEase(Ease.InQuad));
        
        flip.AppendCallback(() =>
        {
            if (showFront)
            {
                ShowFront();
            }
            else
            {
                ShowBack();
            }
        });
        
        flip.Append(transform.DOScaleX(1f, duration / 2).SetEase(Ease.OutQuad));
    }
}
