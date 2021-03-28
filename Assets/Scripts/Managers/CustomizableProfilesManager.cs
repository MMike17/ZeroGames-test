using System.Collections.Generic;
using UnityEngine;

public class CustomizableProfilesManager : BaseBehaviour
{
	const string FILE_LIST_NAME = "ProfileNames";
	const string PLAYER_PREFERENCES_FILE_NAME = "PlayerPreferences";

	[Header("Settings")]
	public Hat[] hats;
	public Color[] colors;
	public Gadget[] gadgets;

	CustomizationProfile[] loadedProfiles;
	PlayerBehaviour player;
	PlayerPreferences preferences;

	public void Init(PlayerBehaviour player)
	{
		this.player = player;

		CustomizationProfilesList profileList = FileManager.LoadFile<CustomizationProfilesList>(FILE_LIST_NAME + CustomizationProfilesList.FILE_TYPE);

		// not the first time we play
		if(profileList != null)
		{
			loadedProfiles = new CustomizationProfile[profileList.profilesNames.Length];

			for (int i = 0; i < profileList.profilesNames.Length; i++)
				loadedProfiles[i] = FileManager.LoadFile<CustomizationProfile>(profileList.profilesNames[i]);
		}
		else // first time we play
		{
			Debug.Log(debugTag + "No profile names found, generating default profile");

			loadedProfiles = new CustomizationProfile[1];
			loadedProfiles[0] = new CustomizationProfile("Default");
		}

		preferences = FileManager.LoadFile<PlayerPreferences>(PLAYER_PREFERENCES_FILE_NAME + CustomizationProfilesList.FILE_TYPE);

		if(preferences == null)
			preferences = new PlayerPreferences();

		InitInternal();
	}

	public CustomizationProfile[] GetLocalProfiles()
	{
		return loadedProfiles;
	}

	public int GetSelectedProfile()
	{
		return preferences.selectedProfileIndex;
	}

	public Hat ApplyHatToPlayer(int index)
	{
		player.GiveHat(hats[index].hatPrefab);
		return hats[index];
	}

	public Color ApplyColorToPlayer(int index)
	{
		player.GiveColor(colors[index].color);
		return colors[index];
	}

	public Gadget ApplyGadgetToPlayer(int index)
	{
		player.GiveGadget(gadgets[index].gadgetPrefab);
		return gadgets[index];
	}

	public void SaveCustomizationprofiles(CustomizationProfile[] profiles, int selectedProfileIndex)
	{
		DeleteLocalSave(profiles);

		CustomizationProfilesList profileList = new CustomizationProfilesList(profiles);
		FileManager.SaveFile(profileList, FILE_LIST_NAME + CustomizationProfilesList.FILE_TYPE);

		for (int i = 0; i < profiles.Length; i++)
			FileManager.SaveFile(profiles[i], profileList.profilesNames[i]);

		preferences.selectedProfileIndex = selectedProfileIndex;
		FileManager.SaveFile(preferences, PLAYER_PREFERENCES_FILE_NAME + CustomizationProfilesList.FILE_TYPE);

		Debug.Log(debugTag + "All profiles have been saved localy");
	}

	void DeleteLocalSave(CustomizationProfile[] profiles)
	{
		List<CustomizationProfile> toDelete = new List<CustomizationProfile>();
		List<CustomizationProfile> currentProfiles = new List<CustomizationProfile>(profiles);

		// searches for profiles that still exists and that were delete in game
		foreach (CustomizationProfile profile in loadedProfiles)
		{
			if(!currentProfiles.Contains(profile))
				toDelete.Add(profile);
		}

		// delete profiles that do not exist anymore
		CustomizationProfilesList toDeleteNames = new CustomizationProfilesList(toDelete.ToArray());

		foreach (string fileName in toDeleteNames.profilesNames)
			FileManager.DeleteFile(fileName);
	}
}