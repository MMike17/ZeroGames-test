using System.IO;
using UnityEngine;

/// <summary>File saving and loading class</summary>
public static class FileManager
{
	static string localPath => Application.persistentDataPath;
	static string debugTag => "<b>[FileManager] : </b>";

	public static void SaveFile<T>(T objectToSave, string fileName)
	{
		string filePath = Path.Combine(localPath, fileName);

		if(Directory.Exists(localPath))
			Directory.CreateDirectory(localPath);

		string jsonData = JsonUtility.ToJson(objectToSave, true);
		File.WriteAllText(filePath, jsonData);

		Debug.Log(debugTag + "File fo type " + objectToSave.GetType() + " has been saved as Json to " + filePath);
	}

	public static T LoadFile<T>(string fileName)
	{
		string filePath = Path.Combine(localPath, fileName);
		string jsonData = null;

		if(File.Exists(filePath))
			jsonData = File.ReadAllText(filePath);

		if(jsonData == null)
		{
			Debug.LogError(debugTag + "File with name \"" + fileName + "\" was not found");
			return default(T);
		}

		T loadedObject = JsonUtility.FromJson<T>(jsonData);
		return loadedObject;
	}
}