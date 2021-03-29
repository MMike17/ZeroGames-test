using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used to controll the player interface and transitions between panels</summary>
public class PlayerInterfaceManager : BaseBehaviour
{
	[Header("Settings")]
	public string showMenuAnimationFormat;
	public string hideMenuAnimationFormat;

	[Header("Scene references - UI")]
	public Animator canvasAnimator;
	public TextMeshProUGUI zoneTitleText, zonePromptText;
	public Button zonePromptButton;

	Action FocusCamera;

	public void Init(Action focusCamera)
	{
		FocusCamera = focusCamera;

		InitInternal();
	}

	public void ShowZoneTitle(string title)
	{
		if(!CheckInitialized())
			return;

		zoneTitleText.text = title;
		canvasAnimator.Play("ShowTitle", 0);
	}

	public void HideZoneTitle()
	{
		if(!CheckInitialized())
			return;

		canvasAnimator.Play("HideTitle", 0);
	}

	public void ShowZonePrompt(string zoneAnimationTag, string buttonPrompt)
	{
		if(!CheckInitialized())
			return;

		zonePromptText.text = buttonPrompt;

		zonePromptButton.onClick.AddListener(() =>
		{
			canvasAnimator.Play(string.Format(showMenuAnimationFormat, zoneAnimationTag), 2);

			if(zoneAnimationTag == "Cust")
				FocusCamera();
		});

		canvasAnimator.Play("ShowPrompt", 1);
	}

	public void HideZonePrompt()
	{
		if(!CheckInitialized())
			return;

		zonePromptButton.onClick.RemoveAllListeners();

		canvasAnimator.Play("HidePrompt", 1);
	}
}