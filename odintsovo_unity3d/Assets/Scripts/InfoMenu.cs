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
			_list[i].ClickEvent += ShowWithParams;
		}
	}

	void OnDestroy()
	{
		for (int i = 0; i < _list.Length; i++)
		{
			_list[i].ClickEvent -= ShowWithParams;
		}
	}

	public void ShowWithParams(Base.Apartament apart)
	{
		Show(apart, true);
	}

	public void Show(Base.Apartament apart, bool chooseParam)
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

		_apart = apart;
		_isParam = chooseParam;

		_chooseParam.SetActive(_isParam);
		_choose3d.SetActive(! _isParam);
		if (_isParam)
		{
			_sliceController = _choose3dMenu.GetSlice(apart.house);

			if (_apart.color != null)
			{
				_apart.color.SetActive();
				_oldRoomsParent = _apart.color.gameObject.transform.parent;
				_apart.color.gameObject.transform.parent = _parentRooms;
				_apart.color.gameObject.transform.localScale = Vector3.one;
				_infoPosition.SetPosition(apart);
			}
		}

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
		if (_isParam)
		{
			SliceUpFloor();
			if (_apart.color != null)
			{
				_apart.color.SetDisable();
				_apart.color.gameObject.transform.parent = _oldRoomsParent;
				_apart.color.gameObject.transform.localScale = Vector3.one;
			}
		}
	}

	public void SliceFloor()
	{
		if (_sliceController != null)
		{
			_sliceController.ClickFloor(_apart.floor);
			_slice.SetActive(false);
			_sliceUp.SetActive(true);
		}
	}

	public void SliceUpFloor()
	{
		if (_sliceController != null)
		{
			_sliceController.ClickUpFloor();
			_slice.SetActive(true);
			_sliceUp.SetActive(false);
		}
	}

	[SerializeField] ListController[]	_list;

	[SerializeField] Image				_plan;
	[SerializeField] Image				_planSection;
	[SerializeField] Text				_houseText;
	[SerializeField] Text				_sectionText;
	[SerializeField] Text				_numberText;
	[SerializeField] Text				_floorText;
	[SerializeField] Text				_roomText;
	[SerializeField] Text				_squareText;
	[SerializeField] Text				_mpriceText;
	[SerializeField] Text				_priceText;

	[SerializeField] GameObject			_parent;
	[SerializeField] GameObject			_sectionParent;
	[SerializeField] GameObject			_showSectionButton;

	[SerializeField] GameObject			_chooseParam;
	[SerializeField] GameObject 		_choose3d;

	[SerializeField] Choose3dMenu		_choose3dMenu;
	[SerializeField] GameObject			_slice;
	[SerializeField] GameObject			_sliceUp;

	[SerializeField] InfoCameraPosition	_infoPosition;

	[SerializeField] Transform			_parentRooms;

	bool 								_isParam;
	Base.Apartament						_apart;
	SliceController						_sliceController;
	Transform							_oldRoomsParent;
}
