using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used to load or delete customization profiles</summary>
public class CustomizationProfileTicket : BaseBehaviour
{
	[Header("Scene references - UI")]
	public TextMeshProUGUI profileNameText;
	public Button loadProfileButton, deleteProfileButton;
	public Image selectedStateImage;

	public bool isSelected => selectedStateImage.enabled;

	public void Init(CustomizationProfile profile, Action<CustomizationProfile> LoadProfile, Action<CustomizationProfile, CustomizationProfileTicket> DeleteProfile)
	{
		selectedStateImage.enabled = false;
		profileNameText.text = profile.name;

		loadProfileButton.onClick.AddListener(() =>
		{
			selectedStateImage.enabled = true;
			LoadProfile(profile);
		});

		deleteProfileButton.onClick.AddListener(() =>
		{
			DeleteProfile(profile, this);
			Destroy(gameObject);
		});

		InitInternal();
	}

	public void UnloadProfile()
	{
		selectedStateImage.enabled = false;
	}
}