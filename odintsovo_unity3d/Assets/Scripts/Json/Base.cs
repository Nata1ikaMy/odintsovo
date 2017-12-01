using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
	public System.Action LoadInfoEvent;
	public System.Action LoadObjEvent;

	public class Apartament
    {
        public int      id;         	//порядковый номер
        public int      number;    		//настоящий номер
		public int		room;			//количество комнат
        public int      floor;      	//этаж
		public int		section;		//секция
		public float	square;     	//площадь
		public float	mprice;			//цена за кв. м.
		public int		price;			//цена
		public string	seller;			//продавец
		public string	house;      	//корпус
		public bool		isSale;			//в продаже
		public bool		isFavorite = false;
		public int 		groupFloor;		//группа этажей со схожими планировками
		public int 		numberFloor;	//номер на лестничной площадке

		public RoomsColor		color;

        Sprite					_sprite;
        Sprite					_sectionSprite;

		public bool SetApartament(System.Object obj)
        {
			Dictionary<string, System.Object> dictionary = obj as Dictionary<string, System.Object>;
			if (dictionary == null)
			{
				Debug.Log(dictionary);
				return false;
			}

			if (! dictionary.ContainsKey("id") || ! int.TryParse(dictionary["id"].ToString(), out id))
			{
				return false;
			}

			if (! dictionary.ContainsKey("number") || ! int.TryParse(dictionary["number"].ToString(), out number))
			{
				return false;
			}

			if (! dictionary.ContainsKey("room") || ! int.TryParse(dictionary["room"].ToString(), out room))
			{
				return false;
			}

			if (! dictionary.ContainsKey("floor") || ! int.TryParse(dictionary["floor"].ToString(), out floor))
			{
				return false;
			}

			if (! dictionary.ContainsKey("section") || ! int.TryParse(dictionary["section"].ToString(), out section))
			{
				return false;
			}

			if (! dictionary.ContainsKey("square") || ! float.TryParse(dictionary["square"].ToString(), out square))
			{
				return false;
			}


			if (! dictionary.ContainsKey("mprice") || ! float.TryParse(dictionary["mprice"].ToString(), out mprice))
			{
				return false;
			}


			if (! dictionary.ContainsKey("price") || ! int.TryParse(dictionary["price"].ToString(), out price))
			{
				return false;
			}

			if (dictionary.ContainsKey("seller"))
			{
				string sellerId = dictionary["seller"].ToString();
				if (sellerId == "0")
				{
					seller = "СТИ";
				}
				else //if (sellerId == "1")
				{
					seller = "ООО \"МЕТС-ЦЕНТР\"";
				}
			}
			else
			{
				return false;
			}

			if (dictionary.ContainsKey("house"))
			{
				house = dictionary["house"].ToString();
			}
			else
			{
				return false;
			}

			if (dictionary.ContainsKey("isSale"))
			{
				string isSaleId = dictionary["isSale"].ToString();
				if (isSaleId == "0")
				{
					isSale = false;
				}
				else
				{
					isSale = true;
				}
			}
			else
			{
				return false;
			}

			if (! dictionary.ContainsKey("groupFloor") || ! int.TryParse(dictionary["groupFloor"].ToString(), out groupFloor))
			{
				return false;
			}

			if (! dictionary.ContainsKey("numberFloor") || ! int.TryParse(dictionary["numberFloor"].ToString(), out numberFloor))
			{
				return false;
			}

			return true;
        }

		public void SetApartament(Apartament copy)
		{
			id = copy.id;
			number = copy.number;
			room = copy.room;
			floor = copy.floor;
			section = copy.section;
			square = copy.square;
			mprice = copy.mprice;
			price = copy.price;
			seller = copy.seller;
			house = copy.house;
			isSale = copy.isSale;
			isFavorite = copy.isFavorite;
			groupFloor = copy.groupFloor;
			numberFloor = copy.numberFloor;
			color = copy.color;
		}

		public Sprite GetIcon(bool onlyApart)
		{
			if (onlyApart && _sprite != null)
			{
				return _sprite;
			}
			else if (!onlyApart && _sectionSprite != null)
			{
				return _sectionSprite;
			}

			else
			{
				Texture2D texture = Resources.Load<Texture2D>(string.Format("Apartament/{0}/{1}_section/{2}_{3}{4}", house, section, groupFloor, numberFloor, onlyApart ? "" : "_section"));
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

				if (onlyApart)
				{
					_sprite = sprite;
				}
				else
				{
					_sectionSprite = sprite;
				}

				return sprite;
			}
		}

        public override string ToString()
        {
			return string.Format("id={0}, number={1}, room={2}, section = {3}, floor={4}, square={5}, mprice={6}, price={7}, seller={8}, house={9}, isSale={10}", id, number, room, section, floor, square, mprice, price, seller, house, isSale);
        }

		public bool Equals(Apartament other)
		{
			return id == other.id && number == other.number && room == other.room && floor == other.floor && section == other.section && house == other.house;
		}
    }

    public class Obj
    {
        public GameObject   go;
        public string       name;

        public Obj(GameObject setGo)
        {
            go = setGo;
            name = setGo.name;
        }
    }

    void Awake()
    {
        _json.LoadComplete += LoadInfo;
		_sceneController.SceneIsLoadEvent += LoadScene;
    }

    void OnDestroy()
    {
        _json.LoadComplete -= LoadInfo;
		_sceneController.SceneIsLoadEvent -= LoadScene;
    }

    void LoadInfo()
    {
		//заполнение апартаментов из базы
        _apartament = new List<Apartament>();
		List<System.Object> list = _json.GetList();
        for (int i = 0; i < list.Count; i++)
        {
			Apartament apart = new Apartament();
			if (apart.SetApartament(list[i]))
			{
				_apartament.Add(apart);
			}
        }

		_isLoadInfo = true;
		if (LoadInfoEvent != null)
		{
			LoadInfoEvent();
		}
		LoadObj();
    }

	void LoadScene()
	{
		_isLoadScene = true;
		LoadObj();
	}

	void LoadObj()
	{
		if (_isLoadInfo && _isLoadScene && !_isLoadObj)
		{
			StartCoroutine(LoadObjCoroutine());
			_isLoadObj = true;
		}
	}

	IEnumerator LoadObjCoroutine()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("Rooms");
		if (go != null)
		{
			for (int i = 0; i < go.Length; i++)
			{
				string[] name = go[i].name.Split(new char[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries);
				if (name != null && name.Length == 5 && name[0] == "Rooms")
				{
					for (int j = 0; j < _apartament.Count; j++)
					{
						if (name[1] == _apartament[j].house && name[2] == _apartament[j].section.ToString() && name[3] == _apartament[j].floor.ToString() && name[4] == _apartament[j].numberFloor.ToString())
						{
							_apartament[i].color = go[i].GetComponent<RoomsColor>();
							break;
						}
					}
				}
				yield return null;
			}
		}

		if (LoadObjEvent != null)
		{
			LoadObjEvent();
		}
	}

    /*
    public Apartament GetById(int id)
    {
        for (int i = 0; i < _apartament.Count; i++)
        {
            if (_apartament[i].id == id)
            {
                return _apartament[i];
            }
        }
        return null;
    }

    public void Show(Apartament apart)
    {
        for (int i = 0; i < _apartament.Count; i++)
        {
            _apartament[i].Show(_apartament[i] == apart);
        }
    }
    */

	public List<Apartament> GetSale()
	{
		if (_isLoadInfo)
		{
			List<Apartament> result = new List<Apartament>();
			for (int i = 0; i < _apartament.Count; i++)
			{
				if (_apartament[i].isSale)
				{
					result.Add(_apartament[i]);
				}
			}
			return result;
		}
		else
		{
			return null;
		}
	}

	public Apartament GetApart(string house, string section, string floor, string numberFloor)
	{
		for (int i = 0; i < _apartament.Count; i++)
		{
			if (_apartament[i].house == house && _apartament[i].section.ToString() == section &&
			    _apartament[i].floor.ToString() == floor && _apartament[i].numberFloor.ToString() == numberFloor)
			{
				return _apartament[i];
			}
		}
		return null;
	}


    [SerializeField] JsonUse			_json;
	[SerializeField] SceneController	_sceneController;

    List<Apartament> 	_apartament;
	bool 				_isLoadInfo = false;
	bool				_isLoadScene = false;
	bool 				_isLoadObj = false;
}
