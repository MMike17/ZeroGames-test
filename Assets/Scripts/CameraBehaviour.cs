using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class managing interractive camera movement</summary>
[RequireComponent(typeof(Animator))]
public class CameraBehaviour : BaseBehaviour
{
	[Header("Settings")]
	public float fallOffDistanceThreshold;
	public float cameraFocusTransitionDuration;

	[Header("Scene references")]
	public Camera mainCamera;

	Animator animator;
	Transform target, customizationTarget;
	List<MeshRenderer> hidenObjects;
	Action<Vector3> SetPlayerDestination;
	Quaternion initialRotation;
	Vector3 targetOffset;
	float cameraSpeed;
	bool isInCustomization, startedCustomization;

	public void Init(float playerSpeed, Transform cameraTarget, Transform cameraCustomizationTarget, Action<Vector3> setPlayerDestination)
	{
		animator = GetComponent<Animator>();

		cameraSpeed = playerSpeed;
		target = cameraTarget;
		customizationTarget = cameraCustomizationTarget;
		SetPlayerDestination = setPlayerDestination;

		targetOffset = transform.position - target.position;
		initialRotation = transform.rotation;

		InitInternal();
	}

	void Update()
	{
		if(!CheckInitialized())
			return;

		if(isInCustomization)
			FocusForCustomization();
		else
			FollowTarget();

		PickPlayerDestination();
	}

	void FocusForCustomization()
	{
		if(!startedCustomization)
		{
			float distanceToTarget = Vector3.Distance(customizationTarget.position, transform.position);
			float angleToTarget = Quaternion.Angle(customizationTarget.rotation, transform.rotation);

			float positionStep = distanceToTarget / cameraFocusTransitionDuration;
			float rotationStep = angleToTarget / cameraFocusTransitionDuration;

			animator.Play("Customization");

			startedCustomization = true;

			StartCoroutine(FocusCoroutine(positionStep, rotationStep));
		}
	}

	void FollowTarget()
	{
		Vector3 targetPos = target.position + targetOffset;
		float targetDistance = Vector3.Distance(transform.position, targetPos);
		float currentCameraSpeed = cameraSpeed;

		if(targetDistance <= fallOffDistanceThreshold)
			currentCameraSpeed *= targetDistance / fallOffDistanceThreshold;

		transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, currentCameraSpeed * Time.deltaTime);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, cameraSpeed * 10 * Time.deltaTime);
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

	void DetectCameraObstruction()
	{
		// hides objects between camera and player
		Vector3 castDirection = customizationTarget.position + target.position - customizationTarget.position;
		float castDistance = Vector3.Distance(customizationTarget.position, target.position);

		RaycastHit[] hits = Physics.BoxCastAll(
			customizationTarget.position + (target.position - customizationTarget.position) / 2,
			new Vector3(1, 2, castDistance) / 2,
			castDirection,
			Quaternion.LookRotation(castDirection),
			castDistance
		);

		foreach (RaycastHit hit in hits)
		{
			MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();

			if(renderer != null)
				HideObject(renderer);
		}
	}

	void HideObject(MeshRenderer meshRenderer)
	{
		if(!hidenObjects.Contains(meshRenderer))
			hidenObjects.Add(meshRenderer);

		meshRenderer.enabled = false;
	}

	void RevealHidenObjects()
	{
		foreach (MeshRenderer renderer in hidenObjects)
			renderer.enabled = true;
	}

	IEnumerator FocusCoroutine(float positionStep, float rotationStep)
	{
		DetectCameraObstruction();

		while (transform.position != customizationTarget.position && transform.rotation != customizationTarget.rotation)
		{
			if(!startedCustomization)
				yield break;

			transform.position = Vector3.MoveTowards(transform.position, customizationTarget.position, positionStep * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, customizationTarget.rotation, rotationStep * Time.deltaTime);
			yield return null;
		}

		transform.position = customizationTarget.position;
		transform.rotation = customizationTarget.rotation;
		yield break;
	}

	public void StartCustomization()
	{
		isInCustomization = true;

		hidenObjects = new List<MeshRenderer>();
	}

	public void StopCustomization()
	{
		isInCustomization = false;
		startedCustomization = false;

		animator.Play("Idle");
		RevealHidenObjects();
	}
}