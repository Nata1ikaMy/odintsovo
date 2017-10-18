using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomConditions : MonoBehaviour
{
	public System.Action<string, bool> 	ChangeConditionEvent;
	public System.Action				ChangeEvent;

	[System.Serializable]
	public class Condition
	{
		[SerializeField] string		_id;
		[SerializeField] Button		_button;
		[SerializeField] GameObject	_active;

		bool 						_isActive = true;
		RoomConditions 				_controller;

		public void Activate(RoomConditions controller)
		{
			_controller = controller;
			_button.onClick.AddListener(Click);
		}

		public void Deactivate()
		{
			_button.onClick.RemoveListener(Click);
		}

		public string ActiveId()
		{
			return _isActive ? _id : null;
		}

		void Click()
		{
			_isActive = !_isActive;
			_active.SetActive(_isActive);
			_controller.Click(_id, _isActive);
		}
	}

	void Start()
	{
		for (int i = 0; i < _condition.Length; i++)
		{
			_condition[i].Activate(this);
		}
	}

	void OnDestroy()
	{
		for (int i = 0; i < _condition.Length; i++)
		{
			_condition[i].Deactivate();
		}
	}

	public void Click(string id, bool active)
	{
		if (ChangeConditionEvent != null)
		{
			ChangeConditionEvent(id, active);
		}
		if (ChangeEvent != null)
		{
			ChangeEvent();
		}
	}

	public string GetCondition()
	{
		string result = null;
		for (int i = 0; i < _condition.Length; i++)
		{
			string activeId = _condition[i].ActiveId();
			if (! string.IsNullOrEmpty(activeId))
			{
				if (string.IsNullOrEmpty(result))
				{
					result = activeId;
				}
				else
				{
					result += "_" + activeId;
				}
			}
		}
		return result;
	}

	[SerializeField] Condition[]	_condition;
}
