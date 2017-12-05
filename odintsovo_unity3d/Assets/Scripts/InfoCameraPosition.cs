using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCameraPosition : MonoBehaviour
{
	[System.Serializable]
	public class Position
	{
		[SerializeField] string 	_house;
		[SerializeField] bool		_isUpSide;
		[SerializeField] int		_section;
		[SerializeField] Vector3	_position;
		[SerializeField] Vector3	_rotation;

		public bool Contains(bool isUp, string house, int section)
		{
			if (_isUpSide == isUp && _section == section && _house.Contains(house))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public Vector3 position
		{
			get
			{
				return _position;
			}
		}

		public Vector3 rotation
		{
			get
			{
				return _rotation;
			}
		}
	}

	public void SetPosition(Base.Apartament apart)
	{
		bool isUp = GetIsUp(apart);
		int index = -1;
		for (int i = 0; i < _position.Length; i++)
		{
			if (_position[i].Contains(isUp, apart.house, apart.section))
			{
				index = i;
				break;
			}
		}

		if (index >= 0)
		{
			Vector3 pos = _position[index].position;
			pos.y = apart.color.gameObject.transform.position.y + 20f;
			_cameraController.CamPosition(pos, false);

			_cameraRotation.CamRotation(_position[index].rotation);
		}
	}

	bool GetIsUp(Base.Apartament apart)
	{
		if (apart.house == "A")
		{
			return GetIsUpA(apart);
		}

		return true;
	}

	bool GetIsUpA(Base.Apartament apart)
	{
		if (apart.section == 1)
		{
			if (apart.numberFloor == 1 || apart.numberFloor == 2 || apart.numberFloor == 9)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		if (apart.section == 2)
		{
			if (apart.numberFloor == 1 || apart.numberFloor == 2 || apart.numberFloor == 8 || apart.numberFloor == 9)
			{
				return true;
			}
			else if (apart.numberFloor == 7 && apart.groupFloor == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		if (apart.section == 3)
		{
			if (apart.numberFloor == 1 || apart.numberFloor == 8 || apart.numberFloor == 9)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	[SerializeField] CameraController			_cameraController;
	[SerializeField] CameraRotationController	_cameraRotation;
	[SerializeField] Position[]					_position;
}
