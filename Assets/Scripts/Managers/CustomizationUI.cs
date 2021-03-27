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
	public Button exitPanelButton, newProfileCreateButton;
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

	public void Init(CustomizationProfile[] loadedProfiles, Hat[] selectableHats, Color[] selectableColors, Gadget[] selectedGadgets)
	{
		this.selectableHats = selectableHats;
		this.selectableColors = selectableColors;
		this.selectableGadgets = selectedGadgets;

		customizationProfiles = loadedProfiles != null ? new List<CustomizationProfile>(loadedProfiles) : new List<CustomizationProfile>();

		spawnedProfileTickets = new List<CustomizationProfileTicket>();
		selectedProfile = 0;

		customizationProfiles.ForEach(item => SpawnProfileTicket(item));

		newProfileCreateButton.onClick.AddListener(() =>
		{
			CreateCustomizationProfile();
			newProfileNameInputField.SetTextWithoutNotify("");
		});

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

				SelectProfile(selectedProfile);
			},
			(profile, ticket) =>
			{
				spawnedProfileTickets.Remove(ticket);
				customizationProfiles.Remove(profile);
			}
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
	}

	void SelectProfile(int index)
	{
		CustomizationProfile profile = customizationProfiles[index];

		selectedProfileNameText.text = profile.name;
		selectedProfileHatText.text = selectableHats[profile.hatIndex].hatName;
		selectedProfileColorImage.color = selectableColors[profile.colorIndex].color;
		selectedProfileGadgetText.text = selectableGadgets[profile.gadgetIndex].gadgetName;

		selectedProfileHatPositionText.text = string.Format(positionFormat, profile.hatIndex, selectableColors.Length);
		selectedProfileColorPositionText.text = string.Format(positionFormat, profile.colorIndex, selectableHats.Length);
		selectedProfileGadgetPositionText.text = string.Format(positionFormat, profile.gadgetIndex, selectableGadgets.Length);

		selectedProfile = index;
	}
}