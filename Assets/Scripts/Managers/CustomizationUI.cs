using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used for interractions with the customization panel</summary>
public class CustomizationUI : BaseBehaviour
{
	[Header("Settings")]
	public string positionFormat;

	[Header("Scene references - UI")]
	public TMP_InputField newProfileNameInputField;
	public Button exitPanelButton, newProfileCreateButton, previousHatButton, nextHatButton, previousColorButton, nextColorButton, previousGadgetButton, nextGadgetButton;
	public Image selectedProfileColorImage;
	public TextMeshProUGUI selectedProfileNameText, selectedProfileHatText, selectedProfileGadgetText, selectedProfileHatPositionText, selectedProfileGadgetPositionText, selectedProfileColorPositionText;
	public Transform profilesList;
	public CustomizationProfileTicket profileTicketprefab;

	List<CustomizationProfileTicket> spawnedProfileTickets;
	List<CustomizationProfile> customizationProfiles;
	Hat[] selectableHats;
	Color[] selectableColors;
	Gadget[] selectableGadgets;
	int selectedProfile;

	public void Init(Action ClosePanel, CustomizationProfile[] loadedProfiles, Hat[] selectableHats, Color[] selectableColors, Gadget[] selectedGadgets, int lastSelectedProfile)
	{
		this.selectableHats = selectableHats;
		this.selectableColors = selectableColors;
		this.selectableGadgets = selectedGadgets;

		if(loadedProfiles != null)
			customizationProfiles = new List<CustomizationProfile>(loadedProfiles);
		else
		{
			customizationProfiles = new List<CustomizationProfile>();
			customizationProfiles.Add(new CustomizationProfile("Default"));
		}

		spawnedProfileTickets = new List<CustomizationProfileTicket>();
		selectedProfile = 0;

		customizationProfiles.ForEach(item => SpawnProfileTicket(item));

		exitPanelButton.onClick.AddListener(() => ClosePanel());

		newProfileCreateButton.onClick.AddListener(() =>
		{
			CreateCustomizationProfile();
			newProfileNameInputField.SetTextWithoutNotify("");
		});

		previousHatButton.onClick.AddListener(() =>
		{
			customizationProfiles[selectedProfile].hatIndex--;
			// apply modification here
			CheckHatArrowsState();
		});
		nextHatButton.onClick.AddListener(() =>
		{
			customizationProfiles[selectedProfile].hatIndex++;
			// apply modification here
			CheckHatArrowsState();
		});

		previousColorButton.onClick.AddListener(() =>
		{
			customizationProfiles[selectedProfile].colorIndex--;
			// apply modification here
			CheckColorArrowsState();
		});
		nextColorButton.onClick.AddListener(() =>
		{
			customizationProfiles[selectedProfile].colorIndex++;
			// apply modification here
			CheckColorArrowsState();
		});

		previousGadgetButton.onClick.AddListener(() =>
		{
			customizationProfiles[selectedProfile].gadgetIndex--;
			// apply modification here
			CheckGadgetArrowsState();
		});
		nextGadgetButton.onClick.AddListener(() =>
		{
			customizationProfiles[selectedProfile].gadgetIndex++;
			// apply modification here
			CheckGadgetArrowsState();
		});

		CheckHatArrowsState();
		CheckColorArrowsState();
		CheckGadgetArrowsState();

		SelectProfile(lastSelectedProfile);

		InitInternal();
	}

	void SpawnProfileTicket(CustomizationProfile profile)
	{
		CustomizationProfileTicket profileTicket = Instantiate(profileTicketprefab, profilesList);
		profileTicket.Init(
			profile,
			profile =>
			{
				int index = customizationProfiles.IndexOf(profile);

				if(index == -1)
				{
					Debug.LogError(debugTag + "Profile not found, this should not happen...like ever");
					return;
				}

				SelectProfile(index);
			},
			(profile, ticket) =>
			{
				spawnedProfileTickets.Remove(ticket);
				customizationProfiles.Remove(profile);
			},
			selectedProfile == profileTicket.transform.GetSiblingIndex()
		);

		spawnedProfileTickets.Add(profileTicket);
	}

	void CreateCustomizationProfile()
	{
		if(string.IsNullOrEmpty(newProfileNameInputField.text))
		{
			Debug.LogWarning(debugTag + "Profile name was empty, therefore profile was not created");
			return;
		}

		CustomizationProfile createdProfile = new CustomizationProfile(newProfileNameInputField.text);
		customizationProfiles.Add(createdProfile);

		SpawnProfileTicket(createdProfile);
	}

	void SelectProfile(int index)
	{
		CustomizationProfile profile = customizationProfiles[index];

		selectedProfileNameText.text = profile.name;
		selectedProfileHatText.text = selectableHats[profile.hatIndex].hatName;
		selectedProfileColorImage.color = selectableColors[profile.colorIndex].color;
		selectedProfileGadgetText.text = selectableGadgets[profile.gadgetIndex].gadgetName;

		selectedProfileHatPositionText.text = string.Format(positionFormat, profile.hatIndex + 1, selectableColors.Length);
		selectedProfileColorPositionText.text = string.Format(positionFormat, profile.colorIndex + 1, selectableHats.Length);
		selectedProfileGadgetPositionText.text = string.Format(positionFormat, profile.gadgetIndex + 1, selectableGadgets.Length);

		spawnedProfileTickets.ForEach(item =>
		{
			if(!item.HasThisProfile(profile))
				item.UnloadProfile();
		});

		selectedProfile = index;
	}

	void CheckHatArrowsState()
	{
		CustomizationProfile modifiableProfile = customizationProfiles[selectedProfile];

		previousHatButton.interactable = modifiableProfile.hatIndex > 0;
		nextHatButton.interactable = modifiableProfile.hatIndex < selectableHats.Length - 1;
	}

	void CheckColorArrowsState()
	{
		CustomizationProfile modifiableProfile = customizationProfiles[selectedProfile];

		previousColorButton.interactable = modifiableProfile.colorIndex > 0;
		nextColorButton.interactable = modifiableProfile.colorIndex < selectableColors.Length - 1;
	}

	void CheckGadgetArrowsState()
	{
		CustomizationProfile modifiableProfile = customizationProfiles[selectedProfile];

		previousGadgetButton.interactable = modifiableProfile.gadgetIndex > 0;
		nextGadgetButton.interactable = modifiableProfile.gadgetIndex < selectableGadgets.Length - 1;
	}
}