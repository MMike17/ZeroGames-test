using System;
using System.Collections.Generic;

/// <summary>Class used to initialize and manage Zones objects</summary>
public class ZonesManager : BaseBehaviour
{
	List<Zone> zones;

	public void Init(Action<string> showZoneTitle, Action hideZoneTitle, Action<string, string> showZonePrompt, Action hideZonePrompt)
	{
		zones = new List<Zone>(FindObjectsOfType<Zone>());
		zones.ForEach(zone => zone.Init(showZoneTitle, hideZoneTitle, showZonePrompt, hideZonePrompt));

		InitInternal();
	}
}