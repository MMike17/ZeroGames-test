using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Zone : BaseBehaviour
{
	bool hasVisitor;

	public void Init()
	{
		hasVisitor = false;

		InitInternal();
	}

	public void OnZoneEntered()
	{
		if(!CheckInitialized())
			return;

		Debug.Log(debugTag + "Player entered zone");

		hasVisitor = true;
	}

	public void OnZoneExit()
	{
		if(!CheckInitialized())
			return;

		Debug.Log(debugTag + "Player exited zone");

		hasVisitor = false;
	}
}