using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSearchUI : BaseBehaviour
{
	[Header("Settings")]
	public float spinnerSpeed;

	[Header("Scene references - UI")]
	public TMP_InputField titleKeywordInputField;
	public TMP_InputField ingredientInputField;
	public TextMeshProUGUI pageNumberText;
	public Button startSearchButton, previousPageButton, nextPageButton;
	public Transform ingredientsList, spinner;
	public GameObject spinnerPanel;
	public IngredientBubble ingredientPrefab;
	public List<RecipeDisplay> recipeDisplays;

	List<string> providedIngredients;
	Action<string, string[], int> StartWebSearch;
	int currentPage;

	public void Init(Action<string, string[], int> startSearch)
	{
		StartWebSearch = startSearch;

		providedIngredients = new List<string>();
		currentPage = 1;

		recipeDisplays.ForEach(item => item.gameObject.SetActive(false));
		previousPageButton.interactable = false;

		previousPageButton.onClick.AddListener(() =>
		{
			currentPage--;
			StartSearch();

			previousPageButton.interactable = currentPage > 1;

			pageNumberText.text = currentPage.ToString();
			StartSpinner();
		});

		nextPageButton.onClick.AddListener(() =>
		{
			currentPage++;
			StartSearch();

			pageNumberText.text = currentPage.ToString();
			StartSpinner();
		});

		ingredientInputField.onSubmit.AddListener((ingredient) => SpawnIngredient(ingredient));
		startSearchButton.onClick.AddListener(() => StartSearch());

		InitInternal();
	}

	public void SetRecipes(Recipe[] recipes)
	{
		for (int i = 0; i < recipeDisplays.Count; i++)
		{
			if(i < recipes.Length)
			{
				recipeDisplays[i].gameObject.SetActive(true);
				recipeDisplays[i].Init(recipes[i]);
			}
			else
				recipeDisplays[i].gameObject.SetActive(false);
		}

		StopSpinner();
	}

	void Update()
	{
		if(spinnerPanel.activeInHierarchy)
			spinner.Rotate(0, 0, spinnerSpeed * Time.deltaTime);
	}

	void SpawnIngredient(string ingredient)
	{
		if(string.IsNullOrEmpty(ingredient))
			return;

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

	void StartSpinner()
	{
		spinnerPanel.SetActive(true);
	}

	void StopSpinner()
	{
		spinnerPanel.SetActive(false);
	}
}