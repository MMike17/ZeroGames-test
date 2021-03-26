using System;
using System.Collections.Generic;

public class ZonesManager : BaseBehaviour
{
	List<Zone> zones;

	public void Init(Action<string> showZoneTitle, Action hideZoneTitle, Action<string> showZoneMenu, Action<string> hideZoneMenu)
	{
		zones = new List<Zone>(FindObjectsOfType<Zone>());
		zones.ForEach(zone => zone.Init(showZoneTitle, hideZoneTitle, showZoneMenu, hideZoneMenu));

		InitInternal();
	}
}