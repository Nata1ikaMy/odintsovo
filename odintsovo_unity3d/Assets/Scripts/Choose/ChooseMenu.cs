using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMenu : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(HideCoroutine());
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

	[SerializeField] GameObject	_chooseTab;
	[SerializeField] GameObject	_favoriteTab;
}
