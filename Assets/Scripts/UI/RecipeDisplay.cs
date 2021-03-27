using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used to display recipes</summary>
public class RecipeDisplay : BaseBehaviour
{
	[Header("Scene references - UI")]
	public RawImage thumbnail;
	public TextMeshProUGUI title;
	public Button openWebSite;

	public void Init(Recipe recipe)
	{
		thumbnail.texture = recipe.thumbnailTexture;
		title.text = recipe.title;

		openWebSite.onClick.AddListener(() => Application.OpenURL(recipe.recipeUrl));

		InitInternal();
	}
}