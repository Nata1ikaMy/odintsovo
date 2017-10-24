using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		this.gameObject.SetActive(false);
	}

	[SerializeField] Base	_base;
}
