public class CustomizationProfile
{
	public string name;
	public int hatIndex;
	public int colorIndex;
	public int gadgetIndex;

	public CustomizationProfile(string name)
	{
		this.name = name;

		hatIndex = 0;
		colorIndex = 0;
		gadgetIndex = 0;
	}
}