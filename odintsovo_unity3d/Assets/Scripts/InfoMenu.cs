using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
	void Start()
	{
		for (int i = 0; i < _list.Length; i++)
		{
			_list[i].ClickEvent += Show;
		}
	}

	void OnDestroy()
	{
		for (int i = 0; i < _list.Length; i++)
		{
			_list[i].ClickEvent -= Show;
		}
	}

	public void Show(Base.Apartament apart)
	{
		/*if (_plan.sprite != null)
		{
			Resources.UnloadAsset(_plan.sprite);
		}

		if (_planSection.sprite != null)
		{
			Resources.UnloadAsset(_planSection.sprite);
		}*/

		_parent.SetActive(true);
		HideSection();

		_plan.sprite = apart.GetIcon(true);
		_plan.SetNativeSize();
		_planSection.sprite = apart.GetIcon(false);
		_planSection.SetNativeSize();

		_houseText.text = apart.house;
		_sectionText.text = apart.section.ToString();
		_numberText.text = apart.number.ToString();
		_floorText.text = apart.floor.ToString();
		_roomText.text = apart.room == 0 ? "СТУДИЯ" : apart.room.ToString();
		_squareText.text = apart.square.ToString();
		_mpriceText.text = apart.mprice.ToString("### ###.##");
		_priceText.text = apart.price.ToString("### ### ###");
	}

	public void ShowSection()
	{
		_sectionParent.SetActive(true);
		_showSectionButton.SetActive(false);
	}

	public void HideSection()
	{
		_sectionParent.SetActive(false);
		_showSectionButton.SetActive(true);
	}

	public void Hide()
	{
		HideSection();
		_parent.SetActive(false);
	}

	[SerializeField] ListController[]	_list;

	[SerializeField] Image		_plan;
	[SerializeField] Image		_planSection;
	[SerializeField] Text		_houseText;
	[SerializeField] Text		_sectionText;
	[SerializeField] Text		_numberText;
	[SerializeField] Text		_floorText;
	[SerializeField] Text		_roomText;
	[SerializeField] Text		_squareText;
	[SerializeField] Text		_mpriceText;
	[SerializeField] Text		_priceText;

	[SerializeField] GameObject	_parent;
	[SerializeField] GameObject	_sectionParent;
	[SerializeField] GameObject	_showSectionButton;
}
