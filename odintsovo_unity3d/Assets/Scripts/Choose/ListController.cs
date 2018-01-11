using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListController : MonoBehaviour
{
	public System.Action<Base.Apartament> ClickEvent;
	public System.Action<Base.Apartament> FavoriteEvent;

	void Start()
	{
		_obj = new List<ChooseElement>();
		_otherList.FavoriteEvent += SetFavorite;
		_infoMenu.FavoriteEvent += SetFavorite;
	}

	void OnDestroy()
	{
		_otherList.FavoriteEvent -= SetFavorite;
		_infoMenu.FavoriteEvent -= SetFavorite;
	}

	public enum TypeSort
	{
		home = 0,
		section = 1,
		floor = 2,
		room = 3,
		square = 4,
		price = 5
	};

	public void UpdateScroll(List<Base.Apartament> list)
	{
		/*if (_apart == null)
		{
			_apart = new List<Base.Apartament>();
		}
		else
		{
			_apart.Clear();
		}
		if (list != null && list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Base.Apartament apart = new Base.Apartament(list[i]);
				_apart.Add(apart);
			}
		}*/
		_apart = list;
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
					element.FavoriteEvent += Favorite;
					element.ClickEvent += Click;

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

				element.FavoriteEvent -= Favorite;
				element.ClickEvent -= Click;

				_obj.Remove(element);
				GameObject.Destroy(element.gameObject);
			}
		}

		for (int i = 0; i < _apart.Count; i++)
		{
			_obj[i].SetInfo(_apart[i]);
		}
	}

	void Favorite(Base.Apartament apart)
	{
		if (FavoriteEvent != null)
		{
			FavoriteEvent(apart);
		}
		SetFavorite(apart);
	}

	void Click(Base.Apartament apart)
	{
		if (ClickEvent != null)
		{
			ClickEvent(apart);
		}
	}

	void SetFavorite(Base.Apartament apart)
	{
		if (_apart != null)
		{
			for (int i = 0; i < _apart.Count; i++)
			{
				if (_apart[i].Equals(apart))
				{
					_apart[i].isFavorite = apart.isFavorite;
					_obj[i].SetFavorite(apart.isFavorite);
				}
			}
		}
	}

	[SerializeField] Transform			_parent;
	[SerializeField] GameObject			_prefab;

	[SerializeField] GameObject			_sortHome;
	[SerializeField] GameObject			_sortSection;
	[SerializeField] GameObject			_sortFloor;
	[SerializeField] GameObject			_sortRoom;
	[SerializeField] GameObject			_sortSquare;
	[SerializeField] GameObject			_sortPrice;
	[SerializeField] ListController		_otherList;
	[SerializeField] InfoMenu 			_infoMenu;

	List<Base.Apartament>	_apart;
	List<ChooseElement>		_obj;

	TypeSort 				_sort = TypeSort.home;
	bool 					_minToMax = true;
}
