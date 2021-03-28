using UnityEngine;

/// <summary>Class used to setup execution pipeline and controll other managers and uniques</summary>
public class GameManager : MonoBehaviour
{
	[Header("Scene references - Managers")]
	public ZonesManager zonesManager;
	public PlayerInterfaceManager playerInterface;
	public RecipePuppyConnector recipePuppy;
	public CustomizableProfilesManager profilesManager;
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
		playerInterface.Init(mainCamera.StartCustomization);
		recipePuppy.Init(recipeSearchUI.SetRecipes);
		recipeSearchUI.Init(
			recipePuppy.StartRecipeRequest,
			() => playerInterface.canvasAnimator.Play(string.Format(playerInterface.hideMenuAnimationFormat, "Form"), 2)
		);
		customizationUI.Init(
			() =>
			{
				playerInterface.canvasAnimator.Play(string.Format(playerInterface.hideMenuAnimationFormat, "Cust"), 2);
				mainCamera.StopCustomization();
			},
			profilesManager.GetLocalProfiles(),
			profilesManager.hats,
			profilesManager.colors,
			profilesManager.gadgets,
			0
		);
		profilesManager.Init();

		// Uniques
		mainCamera.Init(player.transform, player.customizationCameraTarget, player.SetPlayerDestination);
		player.Init();
	}
}