using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public System.Action SceneIsLoadEvent;

	public SliceController		sliceA { get; private set; }
	public SliceController		sliceB { get; private set; }
	public SliceController		sliceC { get; private set; }
	public SliceController		sliceD { get; private set; }


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

		SliceController[] slice = GameObject.FindObjectsOfType<SliceController>();
		for (int i = 0; i < slice.Length; i++)
		{
			slice[i].cameraTransform = _camera;

			if (slice[i].name == "A_Slice_Controller")
			{
				sliceA = slice[i];
			}
			else if (slice[i].name == "B_Slice_Controller")
			{
				sliceB = slice[i];
			}
			else if (slice[i].name == "C_Slice_Controller")
			{
				sliceC = slice[i];
			}
			else if (slice[i].name == "D_Slice_Controller")
			{
				sliceD = slice[i];
			}
		}

		yield return null;

		if (SceneIsLoadEvent != null)
		{
			SceneIsLoadEvent();
		}
		yield return null;
	}

	[SerializeField] Transform		_camera;
}
