using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ResultUiView : MonoBehaviour
{
	[SerializeField] 
	private Canvas resultCanvas;

	[Header("User score")]
	[SerializeField] 
	private TextMeshProUGUI userScoreText;
	
	[SerializeField] 
	private Button retryButton;
	[SerializeField] 
	private RectTransform retryButtonFinalPosition;
	[SerializeField] 
	private RectTransform retryButtonRectTransform;
	
	[Header("Animate rect transforms (Tooltip)")]
	[Tooltip("The elements appear in the same order as in the array")]
	[SerializeField] 
	private RectTransform[] resultItemsToAnimate;
	
	public void Init(Action retryAction)
	{
		retryButton.onClick.AddListener(()=>
		{
			TriggerExitResultAnimation(retryAction);
		});
	}
	
	/// <summary>
	/// Trigger enter items in order
	/// </summary>
	private void TriggerEnterResultAnimation()
	{
		float delay = 0.1f;
		foreach (RectTransform itemToAnimate in resultItemsToAnimate)
		{
			itemToAnimate.DOLocalMoveX(0, 0.2f).From(-itemToAnimate.rect.width * 2).SetDelay(delay);
			delay += 0.05f;
		}
		retryButtonRectTransform.DOLocalMoveY(retryButtonFinalPosition.localPosition.y, 0.2f).From(-Screen.width * 2)
			.SetDelay(delay);
		
	}
	
	/// <summary>
	/// Trigger exit items in order
	/// </summary>
	private void TriggerExitResultAnimation(Action finishAction)
	{
		float delay = 0.01f;
		retryButtonRectTransform.DOLocalMoveY(-Screen.width * 2, 0.25f)
			.From(retryButtonFinalPosition.localPosition.y)
			.SetDelay(delay);

		int count = resultItemsToAnimate.Length - 1;

		for (var index = 0; index < resultItemsToAnimate.Length; index++)
		{
			RectTransform itemToAnimate = resultItemsToAnimate[index];

			if (index == count)
			{
				itemToAnimate.DOLocalMoveX(itemToAnimate.rect.width * 2, 0.2f)
					.From(0)
					.SetDelay(delay)
					.OnComplete(() =>
					{
						ToggleScreen(false);
						finishAction?.Invoke();
					});
			}
			else
			{
				itemToAnimate.DOLocalMoveX(itemToAnimate.rect.width * 2, 0.2f)
					.From(0)
					.SetDelay(delay);
			}

			delay += 0.05f;
		}
	}

	public void SetScore(int score)
	{
		userScoreText.text = $"Score: {score}";
	}
	

	public void ToggleScreen(bool enable)
	{
		resultCanvas.enabled = enable;
	}

	public void GameOver(int score)
	{
		ToggleScreen(true);
		TriggerEnterResultAnimation();
		SetScore(score);
	}
}
