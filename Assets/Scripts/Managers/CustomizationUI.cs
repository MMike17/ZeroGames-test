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
	Func<int, Hat> GiveHatToPlayer;
	Func<int, Color> GiveColorToPlayer;
	Func<int, Gadget> GiveGadgetToPlayer;
	int selectedProfile, selectableHatsLength, selectableColorsLength, selectableGadgetsLength;

	public void Init(Action ClosePanel, Func<int, Hat> giveHatToPlayer, Func<int, Color> giveColorToPlayer, Func<int, Gadget> giveGadgetToPlayer, CustomizationProfile[] loadedProfiles, int lastSelectedProfile, int selectableHatsLength, int selectableColorsLength, int selectableGadgetsLength)
	{
		GiveHatToPlayer = giveHatToPlayer;
		GiveColorToPlayer = giveColorToPlayer;
		GiveGadgetToPlayer = giveGadgetToPlayer;

		this.selectableHatsLength = selectableHatsLength;
		this.selectableColorsLength = selectableColorsLength;
		this.selectableGadgetsLength = selectableGadgetsLength;

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
			CustomizationProfile profile = customizationProfiles[selectedProfile];

			profile.hatIndex--;
			SetHat(profile.hatIndex);

			CheckHatArrowsState();
		});
		nextHatButton.onClick.AddListener(() =>
		{
			CustomizationProfile profile = customizationProfiles[selectedProfile];

			profile.hatIndex++;
			SetHat(profile.hatIndex);

			CheckHatArrowsState();
		});

		previousColorButton.onClick.AddListener(() =>
		{
			CustomizationProfile profile = customizationProfiles[selectedProfile];

			profile.colorIndex--;
			SetColor(profile.colorIndex);

			CheckColorArrowsState();
		});
		nextColorButton.onClick.AddListener(() =>
		{
			CustomizationProfile profile = customizationProfiles[selectedProfile];

			profile.colorIndex++;
			SetColor(profile.colorIndex);

			CheckColorArrowsState();
		});

		previousGadgetButton.onClick.AddListener(() =>
		{
			CustomizationProfile profile = customizationProfiles[selectedProfile];

			profile.gadgetIndex--;
			SetGadget(profile.gadgetIndex);

			CheckGadgetArrowsState();
		});
		nextGadgetButton.onClick.AddListener(() =>
		{
			CustomizationProfile profile = customizationProfiles[selectedProfile];

			profile.gadgetIndex++;
			SetGadget(profile.gadgetIndex);

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
		SetHat(profile.hatIndex);
		SetColor(profile.colorIndex);
		SetGadget(profile.gadgetIndex);

		spawnedProfileTickets.ForEach(item =>
		{
			if(!item.HasThisProfile(profile))
				item.UnloadProfile();
		});

		selectedProfile = index;
	}

	void SetHat(int index)
	{
		Hat given = GiveHatToPlayer(index);
		selectedProfileHatText.text = given.hatName;

		customizationProfiles[selectedProfile].hatIndex = index;
		selectedProfileHatPositionText.text = string.Format(positionFormat, index + 1, selectableColorsLength);
	}

	void SetColor(int index)
	{
		Color given = GiveColorToPlayer(index);
		selectedProfileColorImage.color = given.color;

		customizationProfiles[selectedProfile].colorIndex = index;
		selectedProfileColorPositionText.text = string.Format(positionFormat, index + 1, selectableHatsLength);
	}

	void SetGadget(int index)
	{
		Gadget given = GiveGadgetToPlayer(index);
		selectedProfileGadgetText.text = given.gadgetName;

		customizationProfiles[selectedProfile].gadgetIndex = index;
		selectedProfileGadgetPositionText.text = string.Format(positionFormat, index + 1, selectableGadgetsLength);
	}

	void CheckHatArrowsState()
	{
		CustomizationProfile modifiableProfile = customizationProfiles[selectedProfile];

		previousHatButton.interactable = modifiableProfile.hatIndex > 0;
		nextHatButton.interactable = modifiableProfile.hatIndex < selectableHatsLength - 1;
	}

	void CheckColorArrowsState()
	{
		CustomizationProfile modifiableProfile = customizationProfiles[selectedProfile];

		previousColorButton.interactable = modifiableProfile.colorIndex > 0;
		nextColorButton.interactable = modifiableProfile.colorIndex < selectableColorsLength - 1;
	}

	void CheckGadgetArrowsState()
	{
		CustomizationProfile modifiableProfile = customizationProfiles[selectedProfile];

		previousGadgetButton.interactable = modifiableProfile.gadgetIndex > 0;
		nextGadgetButton.interactable = modifiableProfile.gadgetIndex < selectableGadgetsLength - 1;
	}
}