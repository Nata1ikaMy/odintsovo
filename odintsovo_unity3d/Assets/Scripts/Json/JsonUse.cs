using UnityEngine;
using System.Collections;
using MiniJSON;
using System.Collections.Generic;

public class JsonUse : MonoBehaviour 
{
    const string                _url = "http://cs.loc/hydra/json/data.json";
    [SerializeField]TextAsset   _textAsset;

	public System.Action LoadComplete;

	public bool isLoad
	{
		get
		{
			return _isLoad;
		}
		private set
		{
			_isLoad = value;
			if (value && LoadComplete != null)
			{
				LoadComplete();
			}
		}
	}

	void Start() 
	{
        if (_textAsset != null)
        {
            _text = _textAsset.text;
            Save();
        }
        else
        {
            StartCoroutine(Load());
        }
	}

	IEnumerator Load()
	{
		yield return null;
		isLoad = false;
		WWW www = new WWW(_url); 
		yield return www;
		_text = www.text;
		Save();
	}

	void Save()
	{
		_list = Json.Deserialize(_text) as List<System.Object>;

		if (_list != null)
		{
			isLoad = true;
		}
	}

	public List<System.Object> GetList()
    {
		return _list;
    }

	string 								_text;
	List<System.Object> 				_list;
	bool								_isLoad = false;
}
