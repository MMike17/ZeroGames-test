using UnityEngine;

/// <summary>Class used to setup execution pipeline and controll other managers and uniques</summary>
public class GameManager : MonoBehaviour
{
	[Header("Scene references - Uniques")]
	public CameraBehaviour mainCamera;
	public Transform player;

	void Awake()
	{
		mainCamera.Init(player);
	}
}