using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSearchUI : BaseBehaviour
{
	[Header("Scene references - UI")]
	public TMP_InputField titleKeywordInputField;
	public TMP_InputField ingredientInputField;
	public Button startSearchButton;
	public Transform ingredientsList, recipesList;
	public IngredientBubble ingredientPrefab;
	public RecipeDisplay recipePrefab;

	List<string> providedIngredients;
	Action<string, string[], int> StartWebSearch;
	int currentPage;

	public void Init(Action<string, string[], int> startSearch)
	{
		StartWebSearch = startSearch;

		providedIngredients = new List<string>();
		currentPage = 0;

		ingredientInputField.onSubmit.AddListener((ingredient) => SpawnIngredient(ingredient));
		startSearchButton.onClick.AddListener(() => StartSearch());

		InitInternal();
	}

	public void SpawnRecipe(Recipe recipe)
	{
		RecipeDisplay recipeUI = Instantiate(recipePrefab, recipesList);
		recipeUI.Init(recipe);
	}

	void SpawnIngredient(string ingredient)
	{
		providedIngredients.Add(ingredient);

		IngredientBubble bubble = Instantiate(ingredientPrefab, ingredientsList);
		bubble.Init(() => providedIngredients.Remove(ingredient), ingredient);

		ingredientInputField.SetTextWithoutNotify("");
	}

	void StartSearch()
	{
		if(!CheckInitialized())
			return;

		StartWebSearch(titleKeywordInputField.text, providedIngredients.ToArray(), currentPage);
	}
}