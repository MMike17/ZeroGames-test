using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerBehaviour : BaseBehaviour
{
	[Header("Settings")]
	public float maxNavDistanceThreshold = 1;

	NavMeshAgent aiAgent;

	public void Init()
	{
		aiAgent = GetComponent<NavMeshAgent>();

		InitInternal();
	}

	public void SetPlayerDestination(Vector3 targetPos)
	{
		if(!CheckInitialized())
			return;

		NavMeshHit navMeshHit;

		if(NavMesh.SamplePosition(targetPos, out navMeshHit, maxNavDistanceThreshold, 1))
			aiAgent.SetDestination(navMeshHit.position);
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
}