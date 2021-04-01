using UnityEngine;

/// <summary>Scriptable object used to integrate customization hat</summary>
[CreateAssetMenu(fileName = "Hat", menuName = "ZeroGamesTest/Hat")]
public class Hat : ScriptableObject
{
	public string hatName;
	public GameObject hatPrefab;
}