using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseElement : MonoBehaviour
{
	public System.Action<Base.Apartament> ClickEvent;
	public System.Action<Base.Apartament> FavoriteEvent;

	public void SetColor(bool dark)
	{
		_homeImage.color = dark ? _homeDarkColor : _homeColor;
		_sectionImage.color = dark ? _sectionDarkColor : _sectionColor;
		_floorImage.color = dark ? _floorDarkColor : _floorColor;
		_roomImage.color = dark ? _roomDarkColor : _roomColor;
		_squareImage.color = dark ? _squareDarkColor : _squareColor;
		_priceImage.color = dark ? _priceDarkColor : _priceColor;
	}

	public void SetFavorite(bool isFavorite)
	{
		_apart.isFavorite = isFavorite;
		_favoriteImage.SetActive(_apart.isFavorite);
	}

	public void ChangeFavorite()
	{
		SetFavorite(!_apart.isFavorite);
		if (FavoriteEvent != null)
		{
			FavoriteEvent(_apart);
		}
	}

	public void SetInfo(Base.Apartament apart)
	{
		_apart = apart;

		_homeText.text = string.Format("{0}/{1}", _apart.house, _apart.number);
		_sectionText.text = _apart.section.ToString();
		_floorText.text = _apart.floor.ToString();
		_roomText.text = _apart.room == 0 ? "СТУДИЯ" : _apart.room.ToString();
		_squareText.text = _apart.square.ToString();
		_priceText.text = _apart.price.ToString("### ### ###");
		SetFavorite(_apart.isFavorite);
	}

	public void Click()
	{
		if (ClickEvent != null)
		{
			ClickEvent(_apart);
		}
	}

	[SerializeField] GameObject	_favoriteImage;

	[SerializeField] Image		_homeImage;
	[SerializeField] Image		_sectionImage;
	[SerializeField] Image		_floorImage;
	[SerializeField] Image 		_roomImage;
	[SerializeField] Image		_squareImage;
	[SerializeField] Image		_priceImage;

	[SerializeField] Text		_homeText;
	[SerializeField] Text		_sectionText;
	[SerializeField] Text		_floorText;
	[SerializeField] Text 		_roomText;
	[SerializeField] Text		_squareText;
	[SerializeField] Text		_priceText;

	[SerializeField] Color		_homeColor;
	[SerializeField] Color		_sectionColor;
	[SerializeField] Color		_floorColor;
	[SerializeField] Color 		_roomColor;
	[SerializeField] Color		_squareColor;
	[SerializeField] Color		_priceColor;

	[SerializeField] Color		_homeDarkColor;
	[SerializeField] Color		_sectionDarkColor;
	[SerializeField] Color		_floorDarkColor;
	[SerializeField] Color 		_roomDarkColor;
	[SerializeField] Color		_squareDarkColor;
	[SerializeField] Color		_priceDarkColor;

	Base.Apartament 			_apart;
}
