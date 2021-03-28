using UnityEngine;

public class CustomizableProfilesManager : BaseBehaviour
{
	[Header("Settings")]
	public Hat[] hats;
	public Color[] colors;
	public Gadget[] gadgets;

	CustomizationProfile[] loadedProfiles;
	PlayerBehaviour player;

	public void Init(PlayerBehaviour player)
	{
		this.player = player;

		// load local files here

		InitInternal();
	}

	public CustomizationProfile[] GetLocalProfiles()
	{
		return loadedProfiles;
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
}