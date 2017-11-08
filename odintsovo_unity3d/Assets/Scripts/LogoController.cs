using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoController : MonoBehaviour
{
	void Start()
	{
		_base.LoadObjEvent += LoadComplete;
		LoadComplete();
	}

	void OnDestroy()
	{
		_base.LoadObjEvent -= LoadComplete;
	}

	void LoadComplete()
	{
		StartCoroutine(HideCoroutine());
	}

	IEnumerator HideCoroutine()
	{
		yield return null;
		yield return null;
		yield return null;

		Color color = _image.color;
		for (int i = 10; i > 0; i--)
		{
			color.a = i / 10f;
			_image.color = color;
			yield return null;
		}
		this.gameObject.SetActive(false);
	}

	[SerializeField] Base	_base;
	[SerializeField] Image	_image;
}
