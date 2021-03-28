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

	CustomizationProfile profile;

	public void Init(CustomizationProfile profile, Action<CustomizationProfile> LoadProfile, Func<CustomizationProfile, CustomizationProfileTicket, bool> DeleteProfile, bool isInitialSelected)
	{
		selectedStateImage.enabled = isInitialSelected;
		profileNameText.text = profile.name;
		this.profile = profile;

		loadProfileButton.onClick.AddListener(() =>
		{
			LoadProfile(profile);
			selectedStateImage.enabled = true;
		});

		deleteProfileButton.onClick.AddListener(() =>
		{
			bool shouldDestroy = DeleteProfile(profile, this);

			if(shouldDestroy)
				Destroy(gameObject);
		});

		InitInternal();
	}

	public void UnloadProfile()
	{
		selectedStateImage.enabled = false;
	}

	public bool HasThisProfile(CustomizationProfile profile)
	{
		return this.profile == profile;
	}
}