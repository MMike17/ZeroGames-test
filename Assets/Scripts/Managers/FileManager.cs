using System.IO;
using UnityEngine;

/// <summary>File saving and loading class</summary>
public static class FileManager
{
	static string localPath => Application.persistentDataPath;
	static string debugTag => "<b>[FileManager] : </b>";

	public static void SaveFile<T>(T objectToSave, string fileName, string folderName = null)
	{
		string folderPath = folderName != null ? Path.Combine(localPath, folderName) : localPath;
		string filePath = GetFilePath(fileName, folderName);

		if(!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

		string jsonData = JsonUtility.ToJson(objectToSave, true);
		File.WriteAllText(filePath, jsonData);

		Debug.Log(debugTag + "File of type " + objectToSave.GetType() + " has been saved as Json to " + filePath);
	}

	public static T LoadFile<T>(string fileName, string folderName = null)
	{
		string filePath = GetFilePath(fileName, folderName);
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

	public static void DeleteFile(string fileName, string folderName = null)
	{
		string filePath = GetFilePath(fileName, folderName);

		if(File.Exists(filePath))
		{
			File.Delete(filePath);
			Debug.Log(debugTag + "File with name \"" + fileName + "\" was deleted");
		}
		else
			Debug.Log(debugTag + "File with name \"" + fileName + "\" was not found");
	}

	static string GetFilePath(string fileName, string folderName = null)
	{
		if(folderName != null)
			return Path.Combine(localPath, folderName, fileName);
		else
			return Path.Combine(localPath, fileName);
	}
}