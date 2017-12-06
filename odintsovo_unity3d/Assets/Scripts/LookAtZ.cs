using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LookAtZ : MonoBehaviour
{    
	void Awake()
	{
		if (_sceneController != null)
		{
			_sceneController.SceneIsLoadEvent += LoadScene;
		}
	}

	void OnDestroy()
	{
		if (_sceneController != null)
		{
			_sceneController.SceneIsLoadEvent -= LoadScene;
		}
	}

	void LoadScene()
	{
		_transformByName = new List<Transform>();
		for (int i = 0; i < _name.Length; i++)
		{
			GameObject go = GameObject.Find(_name[i]);
			if (go != null)
			{
				Text[] text = go.GetComponentsInChildren<Text>(true);
				for (int j = 0; j < text.Length; j++)
				{
					_transformByName.Add(text[j].gameObject.transform);
				}
			}
		}
	}

    void Update ()
    {
        for (int i = 0; i < _canvasTransform.Length; i++)
        {
			SetPosition(_canvasTransform[i]);
        }

		if (_transformByName != null)
		{
			for (int i = 0; i < _transformByName.Count; i++)
			{
				SetPosition(_transformByName[i]);
			}
		}
    }

	void SetPosition(Transform obj)
	{
		if (_lookAtClassic)
		{
			obj.localEulerAngles = new Vector3(90, 180, 180 - _target.rotation.eulerAngles.y);
		}
		else
		{
			Vector3 targetPostition = new Vector3(_target.position.x, obj.position.y, _target.position.z);
			obj.LookAt(targetPostition);
		}
	}

	[SerializeField] Transform 			_target;
	[SerializeField] Transform[] 		_canvasTransform;

	[SerializeField] bool 				_lookAtClassic = false;
	[SerializeField] float 				_smooth = 2.0F;

	[SerializeField] SceneController	_sceneController;
	[SerializeField] string[]			_name;

	List<Transform>						_transformByName;

}
