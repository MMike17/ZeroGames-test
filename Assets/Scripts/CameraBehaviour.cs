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
	Vector3 targetOffset;

	public void Init(Transform cameraTarget)
	{
		mainCamera = GetComponent<Camera>();

		target = cameraTarget;
		targetOffset = transform.position - target.position;

		InitInternal();
	}

	void Update()
	{
		if(!CheckInitialized())
			return;

		FollowTarget();
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
}