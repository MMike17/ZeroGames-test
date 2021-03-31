using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInterfaceManager.GameMenu;

/// <summary>Class used to controll the player interface and transitions between panels</summary>
public class PlayerInterfaceManager : BaseBehaviour
{
	[Header("Settings")]
	public List<GameMenu> gameMenus;

	[Header("Scene references - UI")]
	public Animator canvasAnimator;
	public TextMeshProUGUI zoneTitleText, zonePromptText;
	public Button zonePromptButton;

	Action BlockPlayer, AllowPlayer;

	public void Init(Action blockPlayer, Action allowPlayer)
	{
		BlockPlayer = blockPlayer;
		AllowPlayer = allowPlayer;

		InitInternal();
	}

	public void SubscribeOpenMenuEvent(MenuTag tag, Action callback)
	{
		if(!CheckInitialized())
			return;

		GameMenu selectedMenu = SelectGameMenu(tag);

		if(selectedMenu == null)
			return;

		int index = gameMenus.IndexOf(selectedMenu);
		gameMenus[index].SubscribeOnOpenEvent(callback);
	}

	public void SubscribeCloseMenuEvent(MenuTag tag, Action callback)
	{
		if(!CheckInitialized())
			return;

		GameMenu selectedMenu = SelectGameMenu(tag);

		if(selectedMenu == null)
			return;

		int index = gameMenus.IndexOf(selectedMenu);
		gameMenus[index].SubscribeOnCloseEvent(callback);
	}

	public void SetZoneData(MenuTag tag)
	{
		if(!CheckInitialized())
			return;

		GameMenu selectedMenu = SelectGameMenu(tag);

		if(selectedMenu == null)
			return;

		zoneTitleText.text = selectedMenu.title;
		zonePromptText.text = selectedMenu.buttonPrompt;

		zonePromptButton.onClick.RemoveAllListeners();
		zonePromptButton.onClick.AddListener(() => ShowMenu(tag));
	}

	public void ShowZonePrompt()
	{
		if(!CheckInitialized())
			return;

		canvasAnimator.Play("ShowPrompt", 0);
	}

	public void HideZonePrompt()
	{
		if(!CheckInitialized())
			return;

		canvasAnimator.Play("HidePrompt", 0);
	}

	public void HideMenu(MenuTag menuTag)
	{
		if(!CheckInitialized())
			return;

		GameMenu selectedMenu = SelectGameMenu(menuTag);

		if(selectedMenu == null)
			return;

		canvasAnimator.Play("ShowPrompt", 0);
		canvasAnimator.Play(selectedMenu.closePanelAnimName, 1);

		selectedMenu.OnPanelClose();
		AllowPlayer();
	}

	void ShowMenu(MenuTag menuTag)
	{
		if(!CheckInitialized())
			return;

		GameMenu selectedMenu = SelectGameMenu(menuTag);

		if(selectedMenu == null)
			return;

		canvasAnimator.Play("HidePrompt", 0);
		canvasAnimator.Play(selectedMenu.openPanelAnimName, 1);

		selectedMenu.OnPanelOpen();
		BlockPlayer();
	}

	GameMenu SelectGameMenu(MenuTag menuTag)
	{
		GameMenu selectedMenu = gameMenus.Find(item => item.menuTag == menuTag);

		if(selectedMenu == null)
		{
			Debug.LogError(debugTag + "Couldn't find game menu with tag " + menuTag);
			return null;
		}

		return selectedMenu;
	}

	[Serializable]
	public class GameMenu
	{
		public MenuTag menuTag;
		public string title, buttonPrompt, openPanelAnimName, closePanelAnimName;
		public Action onPanelOpen, onPanelClose;

		public enum MenuTag
		{
			CUSTOMIZATION,
			RECIPE_SEARCH
		}

		public void SubscribeOnOpenEvent(Action callback)
		{
			if(callback != null)
				onPanelOpen += callback;
		}

		public void SubscribeOnCloseEvent(Action callback)
		{
			if(callback != null)
				onPanelClose += callback;
		}

		public void OnPanelOpen()
		{
			if(onPanelOpen != null)
				onPanelOpen();
		}

		public void OnPanelClose()
		{
			if(onPanelClose != null)
				onPanelClose();
		}
	}
}