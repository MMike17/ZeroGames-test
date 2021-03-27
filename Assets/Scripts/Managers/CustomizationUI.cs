using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used for interractions with the customization panel</summary>
public class CustomizationUI : BaseBehaviour
{
	[Header("Scene references - UI")]
	public TMP_InputField newProfileNameInputField;
	public Button exitPanelButton, newProfileCreateButton;

	public void Init()
	{
		newProfileCreateButton.onClick.AddListener(() =>
		{
			CreateCustomizationProfile();
			newProfileNameInputField.SetTextWithoutNotify("");
		});

		InitInternal();
	}

	void CreateCustomizationProfile()
	{
		if(string.IsNullOrEmpty(newProfileNameInputField.text))
		{
			Debug.LogWarning(debugTag + "Profile name was empty, therefore profile was not created");
			return;
		}
	}
}