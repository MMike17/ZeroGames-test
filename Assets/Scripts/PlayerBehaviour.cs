using UnityEngine;
using UnityEngine.AI;

/// <summary>Class used to move player around</summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerBehaviour : BaseBehaviour
{
	[Header("Settings")]
	public float maxNavDistanceThreshold = 1;

	[Header("Scene references")]
	public MeshRenderer playerRenderer;
	public Transform customizationCameraTarget;
	public TargetIndicator indicator;

	NavMeshAgent aiAgent;
	GameObject selectedHat, selectedColor, selectedGadget;
	bool blockDestination;

	void OnDrawGizmos()
	{
		if(customizationCameraTarget != null)
		{
			Gizmos.color = new UnityEngine.Color(1, 0.5f, 0, 0.5f);
			Gizmos.DrawSphere(customizationCameraTarget.position, 0.3f);
		}
	}

	public void Init(Transform mainCamera)
	{
		aiAgent = GetComponent<NavMeshAgent>();
		indicator.Init(mainCamera, transform);

		InitInternal();
	}

	public void SetPlayerDestination(Vector3 targetPos)
	{
		if(!CheckInitialized() || blockDestination)
			return;

		NavMeshHit navMeshHit;

		if(NavMesh.SamplePosition(targetPos, out navMeshHit, maxNavDistanceThreshold, 1))
		{
			aiAgent.SetDestination(navMeshHit.position);
			indicator.transform.position = targetPos;
		}
		else
			Debug.LogWarning(debugTag + "Couldn't find NavMesh point close to target position within distance");
	}

	void OnTriggerEnter(Collider collider)
	{
		IInterractableZone detectedZone = collider.GetComponent<IInterractableZone>();

		if(detectedZone != null)
			detectedZone.OnZoneEntered();
	}

	void OnTriggerExit(Collider collider)
	{
		IInterractableZone detectedZone = collider.GetComponent<IInterractableZone>();

		if(detectedZone != null)
			detectedZone.OnZoneExit();
	}

	public void GiveHat(GameObject prefab)
	{
		Destroy(selectedHat);

		selectedHat = Instantiate(prefab, transform.position, transform.rotation, transform);
	}

	public void GiveColor(UnityEngine.Color color)
	{
		playerRenderer.material.color = color;
	}

	public void GiveGadget(GameObject prefab)
	{
		Destroy(selectedGadget);

		selectedGadget = Instantiate(prefab, transform.position, transform.rotation, transform);
	}

	public void BlockDestination()
	{
		blockDestination = true;
	}

	public void AllowDestination()
	{
		blockDestination = false;
	}
}