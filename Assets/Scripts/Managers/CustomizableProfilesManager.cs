using UnityEngine;

public class CustomizableProfilesManager : BaseBehaviour
{
	[Header("Settings")]
	public Hat[] hats;
	public Color[] colors;
	public Gadget[] gadgets;

	CustomizationProfile[] loadedProfiles;

	public void Init()
	{
		// load local files here
		InitInternal();
	}

	public CustomizationProfile[] GetLocalProfiles()
	{
		return loadedProfiles;
	}
}