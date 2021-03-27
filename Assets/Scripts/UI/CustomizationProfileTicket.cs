using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationProfileTicket : BaseBehaviour
{
	[Header("Scene references - UI")]
	public TextMeshProUGUI profileNameText;
	public Button loadProfileButton, deleteProfileButton;
	public Image selectedStateImage;

	public void Init(Action LoadProfile, Action DeleteProfile)
	{
		selectedStateImage.enabled = false;
		// profileNameText.text = ;

		loadProfileButton.onClick.AddListener(() =>
		{
			selectedStateImage.enabled = true;
			LoadProfile();
		});

		deleteProfileButton.onClick.AddListener(() =>
		{
			DeleteProfile();
			Destroy(gameObject);
		});

		InitInternal();
	}

	public void UnloadProfile()
	{
		selectedStateImage.enabled = false;
	}
}