using UnityEngine;

/// <summary>Class used to derive other objects from, which need initialization before their execution</summary>
public class BaseBehaviour : MonoBehaviour
{
	protected string debugTag => "<b>[" + GetType() + "] : </b>";
	bool initialized;

	protected void InitInternal()
	{
		initialized = true;
		Debug.Log(debugTag + "Initialized");
	}

	protected bool CheckInitialized()
	{
		if(!enabled)
			return false;

		if(!initialized)
			Debug.LogError(debugTag + "Not initialized");

		return initialized;
	}

	// just here so that all objects derived from BaseBehaviour can be deactivated
	void Update() { }
}