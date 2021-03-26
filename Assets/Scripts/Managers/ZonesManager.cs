using System.Collections.Generic;
using UnityEngine;

public class ZonesManager : MonoBehaviour
{
	List<Zone> zones;

	public void Init()
	{
		zones = new List<Zone>(FindObjectsOfType<Zone>());

		zones.ForEach(zone => zone.Init());
	}
}