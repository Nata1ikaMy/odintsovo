using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseList : MonoBehaviour
{
	public enum TypeSort
	{
		home = 0,
		section = 1,
		floor = 2,
		room = 3,
		square = 4,
		price = 5
	};

	void Start()
	{
		_base.LoadInfoEvent += LoadBaseInfo;
	}

	void LoadBaseInfo()
	{
		_obj = new List<ChooseElement>();
		_all = _base.GetAll();
		UpdateScroll();

		_roomCondition.ChangeEvent += UpdateScroll;
		_squareCondition.ChangeEvent += UpdateScroll;
		_floorCondition.ChangeEvent += UpdateScroll;
		_priceCondition.ChangeEvent += UpdateScroll;
	}

	void OnDestroy()
	{
		_base.LoadInfoEvent -= LoadBaseInfo;

		_roomCondition.ChangeEvent -= UpdateScroll;
		_squareCondition.ChangeEvent -= UpdateScroll;
		_floorCondition.ChangeEvent -= UpdateScroll;
		_priceCondition.ChangeEvent -= UpdateScroll;
	}

	void UpdateScroll()
	{
		if (_apart != null)
		{
			_apart.Clear();
		}
		else
		{
			_apart = new List<Base.Apartament>();
		}

		string roomCondition = _roomCondition.GetCondition();
		for (int i = 0; i < _all.Count; i++)
		{
			if (string.IsNullOrEmpty(roomCondition) || !roomCondition.Contains(_all[i].room.ToString()))
			{
				continue;
			}
			if (_all[i].square > _squareCondition.maxValue || _all[i].square < _squareCondition.minValue)
			{
				continue;
			}
			if (_all[i].floor > _floorCondition.maxValue || _all[i].floor < _floorCondition.minValue)
			{
				continue;
			}
			if (_all[i].price > _priceCondition.maxValue * 1000000 || _all[i].price < _priceCondition.minValue * 1000000)
			{
				continue;
			}

			_apart.Add(_all[i]);
		}

		Sort(_minToMax);
	}

	void Sort(bool minToMax)
	{
		_minToMax = minToMax;

		_sortHome.SetActive(_sort == TypeSort.home);
		_sortSection.SetActive(_sort == TypeSort.section);
		_sortFloor.SetActive(_sort == TypeSort.floor);
		_sortRoom.SetActive(_sort == TypeSort.room);
		_sortSquare.SetActive(_sort == TypeSort.square);
		_sortPrice.SetActive(_sort == TypeSort.price);

		Vector3 scale = _minToMax ? Vector3.one : new Vector3(1, -1, 1);

		if (_sort == TypeSort.home)
		{
			_sortHome.transform.localScale = scale;
		}
		if (_sort == TypeSort.section)
		{
			_sortSection.transform.localScale = scale;
		}
		if (_sort == TypeSort.floor)
		{
			_sortFloor.transform.localScale = scale;
		}
		if (_sort == TypeSort.room)
		{
			_sortRoom.transform.localScale = scale;
		}
		if (_sort == TypeSort.square)
		{
			_sortSquare.transform.localScale = scale;
		}
		if (_sort == TypeSort.price)
		{
			_sortPrice.transform.localScale = scale;
		}


		if (_apart.Count > 1)
		{			
			Base.Apartament change = new Base.Apartament();
			for (int i = 0; i < _apart.Count - 1; i++)
			{
				for (int j = i; j < _apart.Count; j++)
				{
					bool needSort = false;
					if (_sort == TypeSort.home)
					{
						if (_minToMax && _apart[i].number > _apart[j].number || !_minToMax && _apart[i].number < _apart[j].number)
						{
							needSort = true;
						}
					}
					else if (_sort == TypeSort.section)
					{
						if (_minToMax && _apart[i].section > _apart[j].section || !_minToMax && _apart[i].section < _apart[j].section)
						{
							needSort = true;
						}
					}
					else if (_sort == TypeSort.floor)
					{
						if (_minToMax && _apart[i].floor > _apart[j].floor || !_minToMax && _apart[i].floor < _apart[j].floor)
						{
							needSort = true;
						}
					}
					else if (_sort == TypeSort.room)
					{
						if (_minToMax && _apart[i].room > _apart[j].room || !_minToMax && _apart[i].room < _apart[j].room)
						{
							needSort = true;
						}
					}
					else if (_sort == TypeSort.square)
					{
						if (_minToMax && _apart[i].square > _apart[j].square || !_minToMax && _apart[i].square < _apart[j].square)
						{
							needSort = true;
						}
					}
					else if (_sort == TypeSort.price)
					{
						if (_minToMax && _apart[i].price > _apart[j].price || !_minToMax && _apart[i].price < _apart[j].price)
						{
							needSort = true;
						}
					}


					if (needSort)
					{
						change.SetApartament(_apart[i]);
						_apart[i].SetApartament(_apart[j]);
						_apart[j].SetApartament(change);
					}
				}
			}
		}

		UpdateObj();
	}

	public void ClickSort(int type)
	{
		TypeSort typeEnum = (TypeSort)type;
		if (typeEnum == _sort)
		{
			Sort(!_minToMax);
		}
		else
		{
			_sort = typeEnum;
			Sort(true);
		}
	}

	void UpdateObj()
	{
		if (_obj.Count < _apart.Count) //добавляем физически объекты
		{
			int index = _obj.Count;

			for (int i = index; i < _apart.Count; i++)
			{
				GameObject go = (GameObject)GameObject.Instantiate(_prefab);
				go.transform.parent = _parent;
				ChooseElement element = go.GetComponent<ChooseElement>();
				if (element != null)
				{
					element.SetColor(i % 2 == 0);
					_obj.Add(element);
				}
			}
		}
		else if (_obj.Count > _apart.Count) //удаляем
		{
			int index = _obj.Count;
			for (int i = index - 1; i >= _apart.Count; i--)
			{
				ChooseElement element = _obj[i];
				_obj.Remove(element);
				GameObject.Destroy(element.gameObject);
			}
		}

		for (int i = 0; i < _apart.Count; i++)
		{
			_obj[i].SetInfo(_apart[i]);
		}
	}


	[SerializeField] Base				_base;
	[SerializeField] RoomConditions		_roomCondition;
	[SerializeField] MinmaxSlider		_squareCondition;
	[SerializeField] MinmaxSlider		_floorCondition;
	[SerializeField] MinmaxSlider		_priceCondition;

	[SerializeField] Transform			_parent;
	[SerializeField] GameObject			_prefab;

	[SerializeField] GameObject			_sortHome;
	[SerializeField] GameObject			_sortSection;
	[SerializeField] GameObject			_sortFloor;
	[SerializeField] GameObject			_sortRoom;
	[SerializeField] GameObject			_sortSquare;
	[SerializeField] GameObject			_sortPrice;

	List<Base.Apartament>	_all;
	List<Base.Apartament>	_apart;
	List<ChooseElement>		_obj;

	TypeSort 				_sort = TypeSort.home;
	bool 					_minToMax = true;
}
