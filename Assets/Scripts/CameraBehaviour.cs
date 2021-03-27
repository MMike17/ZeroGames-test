using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehaviour : BaseBehaviour
{
	[Header("Settings")]
	public float cameraSpeed;
	public float fallOffDistanceThreshold;

	Camera mainCamera;
	Transform target;
	Action<Vector3> SetPlayerDestination;
	Vector3 targetOffset;

	public void Init(Transform cameraTarget, Action<Vector3> setPlayerDestination)
	{
		mainCamera = GetComponent<Camera>();

		target = cameraTarget;
		SetPlayerDestination = setPlayerDestination;

		targetOffset = transform.position - target.position;

		InitInternal();
	}

	void Update()
	{
		if(!CheckInitialized())
			return;

		FollowTarget();
		PickPlayerDestination();
	}

	void FollowTarget()
	{
		Vector3 targetPos = target.position + targetOffset;
		float targetDistance = Vector3.Distance(transform.position, targetPos);
		float currentCameraSpeed = cameraSpeed;

		if(targetDistance <= fallOffDistanceThreshold)
			currentCameraSpeed *= targetDistance / fallOffDistanceThreshold;

		transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, currentCameraSpeed * Time.deltaTime);
	}

	void PickPlayerDestination()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
			{
				// check if there is an object hit by raycast that is UI
				GameObject[] hoveredObjects = ActuallyUsefulInputModule.GetPointerEventData().hovered.ToArray();
				bool hitsUI = false;

				foreach (GameObject hovered in hoveredObjects)
				{
					if(hovered.activeSelf && hovered.GetComponent<RectTransform>() != null)
					{
						hitsUI = true;
						break;
					}
				}

				if(!hitsUI)
					SetPlayerDestination(hit.point);
			}
			else
				Debug.LogWarning(debugTag + "Mouse raycast didn't hit anything");
		}
	}
}