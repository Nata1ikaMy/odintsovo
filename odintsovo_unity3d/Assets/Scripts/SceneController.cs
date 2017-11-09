﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public System.Action SceneIsLoadEvent;

	void Start()
	{
		StartCoroutine(LoadYourAsyncScene());
	}

	IEnumerator LoadYourAsyncScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("model", LoadSceneMode.Additive);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		yield return null;

		if (SceneIsLoadEvent != null)
		{
			SceneIsLoadEvent();
		}

		yield return null;
	}
}
