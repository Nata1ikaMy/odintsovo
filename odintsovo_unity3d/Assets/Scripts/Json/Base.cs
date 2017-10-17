using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    public class Apartament
    {
        public int      id;         //порядковый номер
        public int      number;    	//настоящий номер
		public int		room;		//количество комнат
        public int      floor;      //этаж
		public int		section;	//секция
		public string   square;     //площадь
		public string	mprice;		//цена за кв. м.
		public string 	price;		//цена
		public string	seller;		//продавец
		public string	house;      //корпус
		public bool		isSale;		//в продаже

        public GameObject   model;
        public MeshRenderer mesh;

        //public Sprite[]     sprite;
        //public Sprite       minimap;

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

			if (dictionary.ContainsKey("square"))
			{
				square = dictionary["square"].ToString();
			}
			else
			{
				return false;
			}

			if (dictionary.ContainsKey("mprice"))
			{
				mprice = dictionary["mprice"].ToString();
			}
			else
			{
				return false;
			}

			if (dictionary.ContainsKey("price"))
			{
				price = dictionary["price"].ToString();
			}
			else
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
				else
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

			return true;
        }

		public Sprite GetIcon(bool onlyApart)
		{
			Texture2D texture = Resources.Load<Texture2D>(string.Format("Apartament/{0}/{1}{2}", house,  number, onlyApart ? "" : "_section"));
			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			return sprite;
		}

        public override string ToString()
        {
			return string.Format("id={0}, number={1}, room={2}, section = {3}, floor={4}, square={5}, mprice={6}, price={7}, seller={8}, house={9}, isSale={10}", id, number, room, section, floor, square, mprice, price, seller, house, isSale);
        }

		public void Show(bool show)
		{
			if (model != null)
			{
				model.SetActive(show);
			}
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
		// += LoadObj;
    }

    void OnDestroy()
    {
        _json.LoadComplete -= LoadInfo;
		// -= LoadObj;
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
		LoadObj();
    }

	void LoadObj()
	{
		if (_isLoadInfo && _isLoadScene && !_isLoadObj)
		{
			//получим список объектов на модели
			/*MeshCollider[] mechCollider = _parentApart.GetComponentsInChildren<MeshCollider>();
			_techObj = new List<Obj>();
			for (int i = 0; i < mechCollider.Length; i++)
			{
				_techObj.Add(new Obj(mechCollider[i].gameObject));
			}*/
		}
	}

    /*public Apartament GetById(int id)
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
    }*/

	public List<Apartament> GetAll()
	{
		List<Apartament> result = new List<Apartament>();
		for (int i = 0; i < _apartament.Count; i++)
		{
			result.Add(_apartament[i]);
		}
		return result;
	}


    [SerializeField] JsonUse        _json;

    List<Apartament> 	_apartament;
	bool 				_isLoadInfo = false;
	bool				_isLoadScene = false;
	bool 				_isLoadObj = false;
}
