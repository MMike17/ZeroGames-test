using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Class used to connect to the RecipePuppy API at http://www.recipepuppy.com/about/api</summary>
public class RecipePuppyConnector : BaseBehaviour
{
	[Header("Settings")]
	public string baseUrl;
	public string keywordPrefix, ingredientsPrefix, pagePrefix;

	Action<Recipe[]> OnResultsParsed;
	RecipePuppyResultMap cachedResults;

	public void Init(Action<Recipe[]> onResultsParsed)
	{
		OnResultsParsed = onResultsParsed;

		InitInternal();
	}

	public void StartRecipeRequest(string keyword, string[] ingredients, int pageNumber)
	{
		if(!CheckInitialized())
			return;

		if(Application.internetReachability == NetworkReachability.NotReachable)
		{
			Debug.LogError(debugTag + "No internet connexion");
			return;
		}

		string completeUrl = baseUrl;

		// add ingredients
		if(ingredients != null && ingredients.Length > 0 && !string.IsNullOrEmpty(ingredients[0]))
		{
			completeUrl += ingredientsPrefix;

			foreach (string ingredient in ingredients)
				completeUrl += ingredient + ",";

			completeUrl = completeUrl.Substring(0, completeUrl.Length - 1);
		}

		// add keyword
		if(!string.IsNullOrEmpty(keyword))
		{
			if(completeUrl[completeUrl.Length - 1] != '?')
				completeUrl += "&";

			completeUrl += keywordPrefix + keyword;
		}

		// cancel request if settings are empty
		if(completeUrl == baseUrl)
			OnResultsParsed(null);

		completeUrl += "&" + pagePrefix + pageNumber;

		UnityWebRequest request = UnityWebRequest.Get(completeUrl);
		request.downloadHandler = new DownloadHandlerBuffer();

		StartCoroutine(ExecuteRecipeRequest(request));
	}

	IEnumerator ExecuteRecipeRequest(UnityWebRequest request)
	{
		yield return request.SendWebRequest();

		if(request.result != UnityWebRequest.Result.Success)
		{
			RequestError(request.result);
			yield break;
		}

		ParseResults(request.downloadHandler.text);
		string[] thumbnailsUrls = GetThumbnailUrls();
		List<Recipe> completeRecipes = new List<Recipe>();

		for (int i = 0; i < thumbnailsUrls.Length; i++)
		{
			Texture2D downloadedTexture = null;

			if(!string.IsNullOrEmpty(thumbnailsUrls[i]))
			{
				UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(thumbnailsUrls[i]);
				yield return textureRequest.SendWebRequest();

				if(textureRequest.result != UnityWebRequest.Result.Success)
				{
					RequestError(textureRequest.result);
					yield break;
				}

				downloadedTexture = DownloadHandlerTexture.GetContent(textureRequest);
			}

			completeRecipes.Add(new Recipe(cachedResults.results[i], downloadedTexture));
		}

		OnResultsParsed(completeRecipes.ToArray());

		yield break;
	}

	void ParseResults(string json)
	{
		cachedResults = JsonUtility.FromJson<RecipePuppyResultMap>(json);
	}

	string[] GetThumbnailUrls()
	{
		List<string> urls = new List<string>();

		foreach (RecipeMap recipeMap in cachedResults.results)
			urls.Add(recipeMap.thumbnail);

		return urls.ToArray();
	}

	void RequestError(UnityWebRequest.Result code)
	{
		Debug.LogError(debugTag + "Error sending web request : " + code.ToString());
	}
}