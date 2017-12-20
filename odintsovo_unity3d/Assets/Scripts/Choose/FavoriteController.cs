using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteController : MonoBehaviour
{
	void Start()
	{
		_apart = new List<Base.Apartament>();
		_favoriteList.FavoriteEvent += Favorite;
		_chooseList.FavoriteEvent += Favorite;
		_info.FavoriteEvent += Favorite;
	}

	public void Add(Base.Apartament element)
	{
		_apart.Add(element);
		_favoriteList.UpdateScroll(_apart);
	}

	public bool Remove(Base.Apartament element)
	{
		for (int i = 0; i < _apart.Count; i++)
		{
			if (_apart[i].Equals(element))
			{
				_apart.Remove(_apart[i]);
				_favoriteList.UpdateScroll(_apart);
				return true;
			}
		}
		return false;
	}

	public void Clear()
	{
		_apart.Clear();
		_favoriteList.UpdateScroll(_apart);
	}

	public bool Contains(Base.Apartament element)
	{
		if (_apart == null)
		{
			return false;
		}

		for (int i = 0; i < _apart.Count; i++)
		{
			if (_apart[i].Equals(element))
			{
				return true;
			}
		}
		return false;
	}

	void Favorite(Base.Apartament apart)
	{
		if (apart.isFavorite)
		{
			Add(apart);
		}
		else
		{
			Remove(apart);
		}
	}

	[SerializeField] Base				_base;
	[SerializeField] ListController		_favoriteList;
	[SerializeField] ListController		_chooseList;
	[SerializeField] InfoMenu			_info;

	List<Base.Apartament>	_apart;
}
