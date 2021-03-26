using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerBehaviour : BaseBehaviour
{
	NavMeshAgent aiAgent;

	public void Init()
	{
		aiAgent = GetComponent<NavMeshAgent>();

		InitInternal();
	}
}