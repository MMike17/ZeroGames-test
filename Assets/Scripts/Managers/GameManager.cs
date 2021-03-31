using UnityEngine;
using static PlayerInterfaceManager.GameMenu;

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
		InitializeComponent();
		SubscribeEvents();
	}

	void InitializeComponent()
	{
		// Managers
		zonesManager.Init(
			playerInterface.SetZoneData,
			playerInterface.ShowZonePrompt,
			playerInterface.HideZonePrompt
		);
		playerInterface.Init(player.BlockDestination, player.AllowDestination);
		recipePuppy.Init(recipeSearchUI.SetRecipes);
		recipeSearchUI.Init(
			recipePuppy.StartRecipeRequest,
			() => playerInterface.HideMenu(MenuTag.RECIPE_SEARCH)
		);
		profilesManager.Init(player);
		customizationUI.Init(
			() => playerInterface.HideMenu(MenuTag.CUSTOMIZATION),
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
		player.Init(mainCamera.transform);
	}

	void SubscribeEvents()
	{
		playerInterface.SubscribeOpenMenuEvent(MenuTag.CUSTOMIZATION, () => mainCamera.StartCustomization());
		playerInterface.SubscribeCloseMenuEvent(MenuTag.CUSTOMIZATION, () => mainCamera.StopCustomization());
	}

	void OnApplicationQuit()
	{
		profilesManager.SaveCustomizationprofiles(customizationUI.GetCurrentProfiles(), customizationUI.GetSelectedProfile());
	}
}