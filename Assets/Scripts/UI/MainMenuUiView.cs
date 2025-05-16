using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiView : MonoBehaviour
{
    [SerializeField] 
    private Canvas mainMenuCanvas;
    
    [SerializeField]
    private CanvasGroup menuCanvasGroup;
    
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private RectTransform playButtonRectTransform;
    
    [SerializeField]
    private Button loadButton;
    [SerializeField]
    private RectTransform loadButtonRectTransform;
    [SerializeField]
    private RectTransform buttonsFinalPosition;
    
    [Header("Animate rect transforms (Tooltip)")]
    [Tooltip("The elements appear in the same order as in the array")]
    [SerializeField] 
    private RectTransform[] menuItemsToAnimate;
    
    public void Init()
    {
        playButton.onClick.AddListener(()=>
        {
            TriggerExitMenuAnimation(()=>
            {
                StartGameplay();
                GameManager.Instance.NewGame();
            });
       
        });

        if (GameManager.Instance.hasGameInProgress)
        {
            loadButton.interactable = true;
            loadButton.onClick.AddListener(()=>
            {
                TriggerExitMenuAnimation(()=>
                {
                    StartGameplay();
                    GameManager.Instance.StartFromLoadState();
                });
           
            });
        }
        else
        {
            loadButton.interactable = false;
        }

        TriggerEnterMenuAnimation();
    }
    
    public void ToggleScreen(bool enable)
    {
        mainMenuCanvas.enabled = enable;
    }
    
        
    public void StartGameplay()
    {
        ToggleScreen(false);
        menuCanvasGroup.DOFade(0, 0.5f).From(1);
    }

    private void TriggerEnterMenuAnimation()
    {
        float delay = 0.15f;
        foreach (RectTransform itemToAnimate in menuItemsToAnimate)
        {
            itemToAnimate.DOLocalMoveX(0, 0.25f).From(-itemToAnimate.rect.width * 2).SetDelay(delay);
            delay += 0.15f;
        }

        playButtonRectTransform.DOLocalMoveY(buttonsFinalPosition.localPosition.y, 0.2f).From(-Screen.width * 2)
            .SetDelay(delay);
        
        loadButtonRectTransform.DOLocalMoveY(buttonsFinalPosition.localPosition.y, 0.2f).From(-Screen.width * 2)
            .SetDelay(delay);
    }
    
    private void TriggerExitMenuAnimation(Action finishAction)
    {
        float delay = 0.01f;
        playButtonRectTransform.DOLocalMoveY(-Screen.width * 2, 0.25f).From(buttonsFinalPosition.localPosition.y).SetDelay(delay);
        loadButtonRectTransform.DOLocalMoveY(-Screen.width * 2, 0.25f).From(buttonsFinalPosition.localPosition.y).SetDelay(delay);
        
        int count = menuItemsToAnimate.Length - 1;
        
        for (var index = 0; index < menuItemsToAnimate.Length; index++)
        {
            RectTransform itemToAnimate = menuItemsToAnimate[index];

            if (index == count)
            {
                itemToAnimate.DOLocalMoveX(itemToAnimate.rect.width * 2, 0.2f).From(0).SetDelay(delay).OnComplete(() =>
                {
                    finishAction?.Invoke();
                });
                continue;
            }
            
            itemToAnimate.DOLocalMoveX(itemToAnimate.rect.width * 2, 0.2f).From(0).SetDelay(delay);
            delay += 0.05f;
        }
    }
}
