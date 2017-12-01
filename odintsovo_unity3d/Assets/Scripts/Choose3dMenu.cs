using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choose3dMenu : MonoBehaviour
{
	public System.Action<Slice> ChangeHouseEvent;

	[System.Serializable]
	public class Slice
	{
		public string 			name;
		public string 			objName;
		[HideInInspector]
		public SliceController 	controller;
	}

	void Start()
	{
		_sceneController.SceneIsLoadEvent += CreateSliceList;
	}

	void OnDestroy()
	{
		_sceneController.SceneIsLoadEvent -= CreateSliceList;
	}
	
	public void Show()
	{
		_parent.SetActive(true);
		SetSlice(0);
	}

	public void Hide()
	{
		_parent.SetActive(false);
		_slice[_currentIndex].controller.ClickUpFloor();
		for (int i = 0; i < _slice.Length; i++)
		{
			_slice[i].controller.SetActiveButtons(false);
		}
	}

	public SliceController GetSlice(string house)
	{
		for (int i = 0; i < _slice.Length; i++)
		{
			if (_slice[i].name.Contains(house) || house.Contains(_slice[i].name) || _slice[i].objName.Contains(house))
			{
				return _slice[i].controller;
			}
		}
		return null;
	}

	void SetSlice(int index)
	{
		_slice[_currentIndex].controller.ClickUpFloor();
		_currentIndex = index;
		_prevButton.interactable = index > 0;
		_nextButton.interactable = _slice.Length > index + 1;
		_text.text = "КОРПУС " + _slice[_currentIndex].name;

		for (int i = 0; i < _slice.Length; i++)
		{
			_slice[i].controller.SetActiveButtons(i == _currentIndex);
		}

		if (ChangeHouseEvent != null)
		{
			ChangeHouseEvent(_slice[_currentIndex]);
		}
	}

	public void ClickNext()
	{
		if (_currentIndex + 1 < _slice.Length)
		{
			SetSlice(_currentIndex + 1);
		}
	}

	public void ClickPrev()
	{
		if (_currentIndex - 1 >= 0)
		{			
			SetSlice(_currentIndex - 1);
		}
	}

	void CreateSliceList()
	{
		SliceController[] controller = GameObject.FindObjectsOfType<SliceController>();
		for (int i = 0; i < controller.Length; i++)
		{
			for (int j = 0; j < _slice.Length; j++)
			{
				if (controller[i].name == _slice[j].objName)
				{
					_slice[j].controller = controller[i];
					controller[i].cameraTransform = _camera;
				}
			}
		}
	}

	[SerializeField] GameObject			_parent;
	[SerializeField] SceneController	_sceneController;
	[SerializeField] Transform			_camera;
	[SerializeField] Button				_prevButton;
	[SerializeField] Button				_nextButton;
	[SerializeField] Text				_text;
	[SerializeField] Slice[]			_slice;

	int									_currentIndex;
}
