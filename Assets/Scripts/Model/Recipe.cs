using System;
using UnityEngine;

/// <summary>Model of recipes used for display</summary>
public class Recipe
{
	public string title;
	public string recipeUrl;
	public string[] ingredients;
	public Texture2D thumbnailTexture;

	public Recipe(RecipeMap receivedData, Texture2D thumbnail)
	{
		title = receivedData.title;
		recipeUrl = receivedData.href;

		ingredients = receivedData.ingredients.Split(new string[1] { ", " }, StringSplitOptions.None);

		thumbnailTexture = thumbnail;
	}
}