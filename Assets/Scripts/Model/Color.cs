using UnityEngine;

/// <summary>Scriptable object used to integrate customization colors</summary>
[CreateAssetMenu(fileName = "Color", menuName = "ZeroGamesTest/Color")]
public class Color : ScriptableObject
{
	public UnityEngine.Color color = UnityEngine.Color.white;
}