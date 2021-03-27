using System;
using UnityEngine;

/// <summary>Class used to setup zones and link them to their respective interface panels</summary>
[RequireComponent(typeof(BoxCollider))]
public class Zone : BaseBehaviour, IInterractableZone
{
	[Header("Settings")]
	public string zoneTitle;
	public string zoneAnimationTag, zoneInterractionPrompt;

	Action<string, string> ShowZonePrompt;
	Action<string> ShowZoneTitle;
	Action HideZoneTitle, HideZonePrompt;
	bool hasVisitor;

	public void Init(Action<string> showZoneTitle, Action hideZoneTitle, Action<string, string> showZonePrompt, Action hideZonePrompt)
	{
		ShowZoneTitle = showZoneTitle;
		HideZoneTitle = hideZoneTitle;
		ShowZonePrompt = showZonePrompt;
		HideZonePrompt = hideZonePrompt;

		hasVisitor = false;

		InitInternal();
	}

	public void OnZoneEntered()
	{
		if(!CheckInitialized())
			return;

		ShowZoneTitle(zoneTitle);
		ShowZonePrompt(zoneAnimationTag, zoneInterractionPrompt);
		hasVisitor = true;
	}

	public void OnZoneExit()
	{
		if(!CheckInitialized())
			return;

		HideZoneTitle();
		HideZonePrompt();
		hasVisitor = false;
	}
}