using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseList : MonoBehaviour
{
	void Start()
	{
		_base.LoadInfoEvent += LoadBaseInfo;
	}

	void LoadBaseInfo()
	{		
		_all = _base.GetAll();
		UpdateScroll();

		_roomCondition.ChangeEvent += UpdateScroll;
		_squareCondition.ChangeEvent += UpdateScroll;
		_floorCondition.ChangeEvent += UpdateScroll;
		_priceCondition.ChangeEvent += UpdateScroll;

		_list.FavoriteEvent += Favorite;
		_favorite.FavoriteEvent += Favorite;
	}

	void OnDestroy()
	{
		_base.LoadInfoEvent -= LoadBaseInfo;

		_roomCondition.ChangeEvent -= UpdateScroll;
		_squareCondition.ChangeEvent -= UpdateScroll;
		_floorCondition.ChangeEvent -= UpdateScroll;
		_priceCondition.ChangeEvent -= UpdateScroll;

		_list.FavoriteEvent -= Favorite;
		_favorite.FavoriteEvent -= Favorite;
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

		_list.UpdateScroll(_apart);
	}

	void Favorite(Base.Apartament apart)
	{
		for (int i = 0; i < _all.Count; i++)
		{
			if (_all[i].Equals(apart))
			{
				_all[i].isFavorite = apart.isFavorite;
				break;
			}
		}
		for (int i = 0; i < _apart.Count; i++)
		{
			if (_apart[i].Equals(apart))
			{
				_apart[i].isFavorite = apart.isFavorite;
				break;
			}
		}
	}


	[SerializeField] Base				_base;
	[SerializeField] RoomConditions		_roomCondition;
	[SerializeField] MinmaxSlider		_squareCondition;
	[SerializeField] MinmaxSlider		_floorCondition;
	[SerializeField] MinmaxSlider		_priceCondition;

	[SerializeField] ListController		_list;
	[SerializeField] ListController		_favorite;

	List<Base.Apartament>	_all;
	List<Base.Apartament>	_apart;
}
