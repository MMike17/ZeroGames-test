using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Class used to display searched ingredients</summary>
public class IngredientBubble : BaseBehaviour
{
	[Header("Scene references - UI")]
	public TextMeshProUGUI ingredientName;
	public Button deleteButton;

	public void Init(Action OnIngredientDelete, string ingredient)
	{
		ingredientName.text = ingredient;

		deleteButton.onClick.AddListener(() =>
		{
			OnIngredientDelete();
			Destroy(gameObject);
		});

		InitInternal();
	}
}