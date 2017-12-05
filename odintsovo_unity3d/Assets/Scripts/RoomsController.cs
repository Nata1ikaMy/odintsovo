using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomsController : MonoBehaviour
{
	void Start()
	{
		_clickController.ClickObjEvent += Click;
		_chooseController.ChangeHouseEvent += ChangeHouse;
		_chooseController.ChangeFloorEvent += ChangeFloor;
	}

	void OnDestroy()
	{
		_clickController.ClickObjEvent -= Click;
		_chooseController.ChangeHouseEvent -= ChangeHouse;
		_chooseController.ChangeFloorEvent -= ChangeFloor;
	}

	public void Show()
	{
		_isShow = true;
		_clickController.gameObject.SetActive(true);
	}

	public void Hide()
	{
		_isShow = false;
		_clickController.gameObject.SetActive(false);
		HideUI();
	}

	void Click(GameObject obj)
	{
		string[] infoString = obj.name.Split(new char[]{ '_' }, System.StringSplitOptions.RemoveEmptyEntries);
		if (infoString == null || infoString.Length != 5 || infoString[0] != "Rooms")
		{
			return;
		}

		Base.Apartament newApart = _base.GetApart(infoString[1], infoString[2], infoString[3], infoString[4]);
		if (newApart != null)
		{
			if (_apart != null)
			{
				_apart.color.SetDisable();
			}

			_apart = newApart;
			_apart.color.SetActive();
			_infoMenu.Show(_apart, false);
		}
	}

	void ChangeHouse(Choose3dMenu.Slice slice)
	{
		HideUI();
	}

	void ChangeFloor(int index)
	{
		if (_isShow)
		{
			HideUI();
		}
	}

	public void HideUI()
	{
		_infoMenu.Hide();
		if (_apart != null)
		{
			_apart.color.SetDisable();
			_apart = null;
		}
	}

	[SerializeField] Base					_base;
	[SerializeField] ClickController		_clickController;
	[SerializeField] Choose3dMenu			_chooseController;
	[SerializeField] InfoMenu				_infoMenu;

	Base.Apartament		_apart;
	bool 				_isShow = false;
}
