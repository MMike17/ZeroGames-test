using System.IO;
using UnityEngine;

/// <summary>File saving and loading class</summary>
public static class FileManager
{
	static string localPath => Application.persistentDataPath;
	static string debugTag => "<b>[FileManager] : </b>";

	public static void SaveFile<T>(T objectToSave, string fileName)
	{
		string filePath = GetFilePath(fileName);

		if(Directory.Exists(localPath))
			Directory.CreateDirectory(localPath);

		string jsonData = JsonUtility.ToJson(objectToSave, true);
		File.WriteAllText(filePath, jsonData);

		Debug.Log(debugTag + "File of type " + objectToSave.GetType() + " has been saved as Json to " + filePath);
	}

	public static T LoadFile<T>(string fileName)
	{
		string filePath = GetFilePath(fileName);
		string jsonData = null;

		if(File.Exists(filePath))
			jsonData = File.ReadAllText(filePath);

		if(jsonData == null)
		{
			Debug.LogWarning(debugTag + "File with name \"" + fileName + "\" was not found");
			return default(T);
		}

		T loadedObject = JsonUtility.FromJson<T>(jsonData);
		return loadedObject;
	}

	public static void DeleteFile(string fileName)
	{
		string filePath = GetFilePath(fileName);

		if(File.Exists(filePath))
		{
			File.Delete(filePath);
			Debug.Log(debugTag + "File with name \"" + fileName + "\" was deleted");
		}
		else
			Debug.Log(debugTag + "File with name \"" + fileName + "\" was not found");
	}

	static string GetFilePath(string fileName)
	{
		return Path.Combine(localPath, fileName);
	}
}