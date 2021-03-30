using UnityEngine;

/// <summary>Class used to setup execution pipeline and controll other managers and uniques</summary>
public class GameManager : MonoBehaviour
{
	[Header("Scene references - Managers")]
	public ZonesManager zonesManager;
	public PlayerInterfaceManager playerInterface;
	public RecipePuppyConnector recipePuppy;
	public CustomizableProfilesManager profilesManager;
	[Space]
	public RecipeSearchUI recipeSearchUI;
	public CustomizationUI customizationUI;

	[Header("Scene references - Uniques")]
	public CameraBehaviour mainCamera;
	public PlayerBehaviour player;

	void Awake()
	{
		// Managers
		zonesManager.Init(
			playerInterface.ShowZoneTitle,
			playerInterface.HideZoneTitle,
			playerInterface.ShowZonePrompt,
			playerInterface.HideZonePrompt
		);
		playerInterface.Init(mainCamera.StartCustomization, player.BlockDestination);
		recipePuppy.Init(recipeSearchUI.SetRecipes);
		recipeSearchUI.Init(
			recipePuppy.StartRecipeRequest,
			() =>
			{
				player.AllowDestination();
				playerInterface.canvasAnimator.Play(string.Format(playerInterface.hideMenuAnimationFormat, "Form"), 2);
			}
		);
		profilesManager.Init(player);
		customizationUI.Init(
			() =>
			{
				playerInterface.canvasAnimator.Play(string.Format(playerInterface.hideMenuAnimationFormat, "Cust"), 2);
				mainCamera.StopCustomization();
				player.AllowDestination();
			},
			profilesManager.ApplyHatToPlayer,
			profilesManager.ApplyColorToPlayer,
			profilesManager.ApplyGadgetToPlayer,
			profilesManager.GetLocalProfiles(),
			profilesManager.GetSelectedProfile(),
			profilesManager.hats.Length,
			profilesManager.colors.Length,
			profilesManager.gadgets.Length
		);

		// Uniques
		mainCamera.Init(player.transform, player.customizationCameraTarget, player.SetPlayerDestination);
		player.Init();
	}

	void OnApplicationQuit()
	{
		profilesManager.SaveCustomizationprofiles(customizationUI.GetCurrentProfiles(), customizationUI.GetSelectedProfile());
	}
}