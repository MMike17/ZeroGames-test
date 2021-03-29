/// <summary>Convenience class used to store names of the profiles saved on disk</summary>
public class CustomizationProfilesList
{
	public const string FILE_TYPE = ".data";

	public string[] profilesNames;

	public CustomizationProfilesList(CustomizationProfile[] profileList)
	{
		profilesNames = new string[profileList.Length];

		for (int i = 0; i < profileList.Length; i++)
			profilesNames[i] = profileList[i].name + FILE_TYPE;
	}
}