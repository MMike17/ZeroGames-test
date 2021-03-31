using System;
using UnityEngine;
using static PlayerInterfaceManager.GameMenu;

/// <summary>Class used to setup zones and link them to their respective interface panels</summary>
[RequireComponent(typeof(BoxCollider))]
public class Zone : BaseBehaviour, IInterractableZone
{
	[Header("Settings")]
	public MenuTag menuTag;

	Action<MenuTag> SetZonePrompt;
	Action ShowZonePrompt, HideZonePrompt;

	public void Init(Action<MenuTag> setZonePrompt, Action showZonePrompt, Action hideZonePrompt)
	{
		SetZonePrompt = setZonePrompt;
		ShowZonePrompt = showZonePrompt;
		HideZonePrompt = hideZonePrompt;

		InitInternal();
	}

	public void OnZoneEntered()
	{
		if(!CheckInitialized())
			return;

		SetZonePrompt(menuTag);
		ShowZonePrompt();
	}

	public void OnZoneExit()
	{
		if(!CheckInitialized())
			return;

		HideZonePrompt();
	}
}