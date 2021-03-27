using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used for interractions with the customization panel</summary>
public class CustomizationUI : BaseBehaviour
{
	[Header("Scene references - UI")]
	public TMP_InputField newProfileNameInputField;
	public Button exitPanelButton, newProfileCreateButton;
	public Image selectedProfileColorImage;
	public TextMeshProUGUI selectedProfileNameText, selectedProfileHatText, selectedProfileGadgetText, selectedProfileHatPositionText, selectedProfileGadgetPositionText, selectedProfileColorPositionText;
	public Transform profilesList;
	public CustomizationProfileTicket profileTicketprefab;

	List<CustomizationProfileTicket> spawnedProfileTickets;
	int selectedProfile;

	public void Init()
	{
		spawnedProfileTickets = new List<CustomizationProfileTicket>();
		selectedProfile = 0;

		newProfileCreateButton.onClick.AddListener(() =>
		{
			CreateCustomizationProfile();
			newProfileNameInputField.SetTextWithoutNotify("");
		});

		InitInternal();
	}

	void SpawnProfileTicket()
	{
		CustomizationProfileTicket profileTicket = Instantiate(profileTicketprefab, profilesList);
		// profileTicket.Init();

		spawnedProfileTickets.Add(profileTicket);
	}

	void CreateCustomizationProfile()
	{
		if(string.IsNullOrEmpty(newProfileNameInputField.text))
		{
			Debug.LogWarning(debugTag + "Profile name was empty, therefore profile was not created");
			return;
		}
	}

	void SelectProfile(int index)
	{
		// selectedProfileNameText.text = ;
		// selectedProfileColorImage.color = ;
		// selectedProfileHatText.text = ;
		// selectedProfileGadgetText.text = ;

		// selectedProfileHatPositionText.text = ;
		// selectedProfileColorPositionText.text = ;
		// selectedProfileGadgetPositionText.text = ;

		selectedProfile = index;
	}
}