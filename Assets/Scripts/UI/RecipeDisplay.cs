using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used to display recipes</summary>
public class RecipeDisplay : BaseBehaviour
{
	[Header("Settings")]
	public int recipeTitleCharacterLimit;

	[Header("Scene references - UI")]
	public RawImage thumbnail;
	public TextMeshProUGUI title;
	public Button openWebSite;

	public void Init(Recipe recipe)
	{
		thumbnail.texture = recipe.thumbnailTexture;

		string recipeTitle = recipe.title.Trim('\n', '\r', ' ').Replace("&nbsp", "");

		if(recipeTitle.Length > recipeTitleCharacterLimit)
			recipeTitle = recipeTitle.Substring(0, recipeTitleCharacterLimit - 4) + "...";

		title.text = recipeTitle;

		openWebSite.onClick.AddListener(() => Application.OpenURL(recipe.recipeUrl));

		InitInternal();
	}
}