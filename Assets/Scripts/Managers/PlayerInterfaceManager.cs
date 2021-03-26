using TMPro;
using UnityEngine;

public class PlayerInterfaceManager : BaseBehaviour
{
	[Header("Scene references - UI")]
	public Animator canvasAnimator;
	public TextMeshProUGUI zoneTitleUI;

	public void Init()
	{
		InitInternal();
	}

	public void ShowZoneTitle(string title)
	{
		if(!CheckInitialized())
			return;

		zoneTitleUI.text = title;
		canvasAnimator.Play("ShowTitle");
	}

	public void HideZoneTitle()
	{
		if(!CheckInitialized())
			return;

		canvasAnimator.Play("HideTitle");
	}

	public void ShowZoneMenu(string zoneAnimationTag)
	{
		if(!CheckInitialized())
			return;
	}

	public void HideZoneMenu(string zoneAnimationTag)
	{
		if(!CheckInitialized())
			return;
	}
}