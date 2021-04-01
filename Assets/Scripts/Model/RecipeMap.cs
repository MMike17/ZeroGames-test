using System;

/// <summary>Model of recipes retrieved from RecipePuppy</summary>
[Serializable]
public class RecipeMap
{
	public string title;
	public string href;
	public string ingredients;
	public string thumbnail;
}