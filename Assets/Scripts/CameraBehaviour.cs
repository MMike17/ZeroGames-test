using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehaviour : BaseBehaviour
{
	[Header("Settings")]
	public float cameraSpeed;

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
		transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, cameraSpeed * Time.deltaTime);
	}
}