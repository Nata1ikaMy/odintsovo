using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMenu : MonoBehaviour
{
	void Start()
	{
		for (int i = 0; i < _list.Length; i++)
		{
			_list[i].ClickEvent += Hide;
		}
		StartCoroutine(HideCoroutine());
	}

	void OnDestroy()
	{
		for (int i = 0; i < _list.Length; i++)
		{
			_list[i].ClickEvent -= Hide;
		}
	}

	IEnumerator HideCoroutine()
	{
		yield return null;
		yield return null;
		yield return null;
		ShowChoose();
		this.gameObject.SetActive(false);
	}

	public void ShowChoose()
	{
		_chooseTab.SetActive(true);
		_favoriteTab.SetActive(false);
	}

	public void ShowFavorite()
	{
		_chooseTab.SetActive(false);
		_favoriteTab.SetActive(true);
	}

	public void Show()
	{
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	void Hide(Base.Apartament apart)
	{
		Hide();
	}

	[SerializeField] ListController[]	_list;
	[SerializeField] GameObject			_chooseTab;
	[SerializeField] GameObject			_favoriteTab;
}
