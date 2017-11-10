using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public System.Action		ClickEvent;
	public System.Action<bool>	SetActiveEvent;

	[System.Serializable]
	public class Menu
	{
		[SerializeField] string			_id;
		[SerializeField] Button			_button;
		[SerializeField] GameObject		_panel;
		[SerializeField] Button[] 		_closeButton;

		MainMenu	_controller;

		public void Activate(MainMenu controller)
		{
			_controller = controller;
			_button.onClick.AddListener(Click);
			for (int i = 0; i < _closeButton.Length; i++)
			{
				_closeButton[i].onClick.AddListener(Hide);
			}
		}

		public void Deactivate()
		{
			_button.onClick.RemoveListener(Click);
			for (int i = 0; i < _closeButton.Length; i++)
			{
				_closeButton[i].onClick.RemoveListener(Hide);
			}
		}

		void Show()
		{			
			_panel.SetActive(true);
		}

		void Hide()
		{
			_panel.SetActive(false);
		}

		void Click()
		{
			Show();
			_controller.Click();
		}
	}

	void Start()
	{
		for (int i = 0; i < _menu.Length; i++)
		{
			_menu[i].Activate(this);
		}
	}
	
	void OnDestroy()
	{
		for (int i = 0; i < _menu.Length; i++)
		{
			_menu[i].Deactivate();
		}
	}

	public void Click()
	{
		if (ClickEvent != null)
		{
			ClickEvent();
		}
	}

	public void Show()
	{
		if (SetActiveEvent != null)
		{
			SetActiveEvent(true);
		}
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		if (SetActiveEvent != null)
		{
			SetActiveEvent(false);
		}
		this.gameObject.SetActive(false);
	}

	public void ChangeMenuPosition()
	{
		_isShow = !_isShow;
		_menuPanel.parent = _isShow ? _menuShowPosition : _menuHidePosition;
		_menuPanel.localPosition = Vector3.zero;
		_menuPanel.localScale = Vector3.one;
	}

	[SerializeField] Menu[] 		_menu;
	[SerializeField] Transform		_menuPanel;
	[SerializeField] Transform		_menuShowPosition;
	[SerializeField] Transform		_menuHidePosition;

	bool _isShow = true;
}
