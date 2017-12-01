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
	}

	void OnDestroy()
	{
		_clickController.ClickObjEvent -= Click;
		_chooseController.ChangeHouseEvent -= ChangeHouse;
	}

	public void Show()
	{
		_clickController.gameObject.SetActive(true);
	}

	public void Hide()
	{
		_clickController.gameObject.SetActive(false);
		HideUI();
	}

	void Click(GameObject obj)
	{
		string[] infoString = obj.name.Split(new char[]{ '_' }, System.StringSplitOptions.RemoveEmptyEntries);
		if (infoString == null || infoString.Length != 5 || infoString[0] != "Rooms" || infoString[1] != _slice.name)
		{
			return;
		}

		if (_apart != null)
		{
			_apart.color.SetDisable();
		}

		_apart = _base.GetApart(infoString[1], infoString[2], infoString[3], infoString[4]);
		if (_apart != null)
		{
			_apart.color.SetActive();
			_infoMenu.Show(_apart, false);
		}
	}

	void ChangeHouse(Choose3dMenu.Slice slice)
	{
		_slice = slice;
		HideUI();
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

	Choose3dMenu.Slice 	_slice;
	Base.Apartament		_apart;
}
