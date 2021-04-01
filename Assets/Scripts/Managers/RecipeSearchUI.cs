using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used for interractions with the recipe search panel</summary>
public class RecipeSearchUI : BaseBehaviour
{
	[Header("Settings")]
	public float spinnerSpeed;

	[Header("Scene references - UI")]
	public TMP_InputField titleKeywordInputField;
	public TMP_InputField ingredientInputField;
	public TextMeshProUGUI pageNumberText, errorMessageText;
	public Button addIngredientButton, startSearchButton, previousPageButton, nextPageButton, exitSearchButton;
	public Transform ingredientsList, spinner;
	public GameObject spinnerPanel, pageNavigationHolder;
	public IngredientBubble ingredientPrefab;
	public List<RecipeDisplay> recipeDisplays;

	List<string> providedIngredients;
	Action<string, string[], int> StartWebSearch;
	int currentPage;

	public void Init(Action<string, string[], int> startSearch, Action ClosePanel)
	{
		StartWebSearch = startSearch;

		providedIngredients = new List<string>();
		currentPage = 1;

		recipeDisplays.ForEach(item => item.gameObject.SetActive(false));
		previousPageButton.interactable = false;

		errorMessageText.enabled = false;
		pageNavigationHolder.SetActive(false);

		addIngredientButton.onClick.AddListener(() => SpawnIngredient(ingredientInputField.text));

		previousPageButton.onClick.AddListener(() =>
		{
			currentPage--;

			StartSearch();
			UpdateArrows();
		});

		nextPageButton.onClick.AddListener(() =>
		{
			currentPage++;

			StartSearch();
			UpdateArrows();
		});

		exitSearchButton.onClick.AddListener(() => ClosePanel());

		ingredientInputField.onSubmit.AddListener((ingredient) => SpawnIngredient(ingredient));
		startSearchButton.onClick.AddListener(() =>
		{
			currentPage = 1;
			UpdateArrows();
			StartSearch();
		});

		InitInternal();
	}

	public void SetRecipes(Recipe[] recipes)
	{
		if(!CheckInitialized())
			return;

		// did not receive any info from server (error)
		if(recipes == null)
		{
			errorMessageText.enabled = true;
			recipeDisplays.ForEach(item => item.gameObject.SetActive(false));
			nextPageButton.interactable = false;
		}
		else
		{
			nextPageButton.interactable = true;
			errorMessageText.enabled = false;

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
		}

		ShowPageNavigation();
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
		StartSpinner();
		StartWebSearch(titleKeywordInputField.text, providedIngredients.ToArray(), currentPage);
	}

	void UpdateArrows()
	{
		previousPageButton.interactable = currentPage > 1;

		pageNumberText.text = currentPage.ToString();
	}

	void StartSpinner()
	{
		spinnerPanel.SetActive(true);
	}

	void StopSpinner()
	{
		spinnerPanel.SetActive(false);
	}

	void ShowPageNavigation()
	{
		pageNavigationHolder.SetActive(true);
		pageNumberText.text = currentPage.ToString();
	}
}