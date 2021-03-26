using UnityEngine;

/// <summary>Class used to setup execution pipeline and controll other managers and uniques</summary>
public class GameManager : MonoBehaviour
{
	[Header("Scene references - Managers")]
	public ZonesManager zonesManager;
	public PlayerInterfaceManager playerInterface;

	[Header("Scene references - Uniques")]
	public CameraBehaviour mainCamera;
	public PlayerBehaviour player;

	void Awake()
	{
		// Managers
		zonesManager.Init(
			playerInterface.ShowZoneTitle,
			playerInterface.HideZoneTitle,
			playerInterface.ShowZoneMenu,
			playerInterface.HideZoneMenu
		);
		playerInterface.Init();

		// Uniques
		mainCamera.Init(player.transform, player.SetPlayerDestination);
		player.Init();
	}
}