using UnityEngine;

/// <summary>Scriptable object used to integrate customization gadgets</summary>
[CreateAssetMenu(fileName = "Gadget", menuName = "ZeroGamesTest/Gadget")]
public class Gadget : ScriptableObject
{
	public string gadgetName;
	public GameObject gadgetPrefab;
}