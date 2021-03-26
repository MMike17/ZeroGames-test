using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Class used to connect to the RecipePuppy API at http://www.recipepuppy.com/about/api</summary>
public class RecipePuppyConnector : BaseBehaviour
{
	[Header("Settings")]
	public string baseUrl;
	public string keywordPrefix, ingredientsPrefix, pagePrefix;

	Action<Recipe[]> OnResultsParsed;

	public void Init(Action<Recipe[]> onResultsParsed)
	{
		OnResultsParsed = onResultsParsed;

		InitInternal();
	}

	public void StartRecipeRequest(string keyword, string[] ingredients, int pageNumber)
	{
		if(!CheckInitialized())
			return;

		string completeUrl = baseUrl;

		// add ingredients
		if(ingredients != null && !string.IsNullOrEmpty(ingredients[0]))
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

		Debug.Log(debugTag + "Generated url : " + completeUrl);

		StartCoroutine(ExecuteRecipeRequest(request));
	}

	IEnumerator ExecuteRecipeRequest(UnityWebRequest request)
	{
		yield return request.SendWebRequest();
		ParseResults(request.downloadHandler.text);

		yield break;
	}

	void ParseResults(string json)
	{
		Debug.Log(debugTag + "Received data from api\n" + json);
		// OnResultsParsed()
	}
}