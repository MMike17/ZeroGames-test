using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Zone : BaseBehaviour, IInterractableZone
{
	[Header("Settings")]
	public string zoneTitle;
	public string zoneAnimationTag;

	Action<string> ShowZoneTitle, ShowZoneMenu, HideZoneMenu;
	Action HideZoneTitle;
	bool hasVisitor;

	public void Init(Action<string> showZoneTitle, Action hideZoneTitle, Action<string> showZoneMenu, Action<string> hideZoneMenu)
	{
		ShowZoneTitle = showZoneTitle;
		HideZoneTitle = hideZoneTitle;
		ShowZoneMenu = showZoneMenu;
		HideZoneMenu = hideZoneMenu;

		hasVisitor = false;

		InitInternal();
	}

	public void OnZoneEntered()
	{
		if(!CheckInitialized())
			return;

		ShowZoneTitle(zoneTitle);
		hasVisitor = true;
	}

	public void OnZoneExit()
	{
		if(!CheckInitialized())
			return;

		HideZoneTitle();
		hasVisitor = false;
	}

	public void StartInterraction()
	{
		ShowZoneMenu(zoneAnimationTag);
	}

	public void StopInterraction()
	{
		HideZoneMenu(zoneAnimationTag);
	}
}