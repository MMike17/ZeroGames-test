using System;
using System.Collections.Generic;
using static PlayerInterfaceManager.GameMenu;

/// <summary>Class used to initialize and manage Zones objects</summary>
public class ZonesManager : BaseBehaviour
{
	List<Zone> zones;

	public void Init(Action<MenuTag> setZonePrompt, Action showZonePrompt, Action hideZonePrompt)
	{
		zones = new List<Zone>(FindObjectsOfType<Zone>());
		zones.ForEach(zone => zone.Init(setZonePrompt, showZonePrompt, hideZonePrompt));

		InitInternal();
	}
}