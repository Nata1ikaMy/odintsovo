﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoramaShowController : MonoBehaviour
{
	[System.Serializable]
	public class ShowButton
	{
		[SerializeField] Button _button;
		[SerializeField] float	_angle;

		PanoramaShowController	_controller;

		public void Activate(PanoramaShowController controller)
		{
			_controller = controller;
			_button.onClick.AddListener(OnClick);
		}

		public void Deactivate()
		{
			_button.onClick.RemoveListener(OnClick);
		}

		void OnClick()
		{
			_controller.Show(_button.gameObject.name, _angle);
		}
	}

	void Start()
	{
		for (int i = 0; i < _buttons.Length; i++)
		{
			_buttons[i].Activate(this);
		}
	}

	void OnDestroy()
	{
		for (int i = 0; i < _buttons.Length; i++)
		{
			_buttons[i].Deactivate();
		}
	}

	public void Show(string name, float angle)
	{
		for (int i = 0; i < _show.Length; i++)
		{
			_show[i].SetActive(true);
		}

		for (int i = 0; i < _hide.Length; i++)
		{
			_hide[i].SetActive(false);
		}

		_cameraController.enabled = false;

		_panorama.material.mainTexture = Resources.Load<Texture>(name);
		_panoramaCameraController.SetCoroutineState(angle, Input.compass.trueHeading);
	}

	public void Hide()
	{
		for (int i = 0; i < _show.Length; i++)
		{
			_show[i].SetActive(false);
		}

		for (int i = 0; i < _hide.Length; i++)
		{
			_hide[i].SetActive(true);
		}

		_cameraController.enabled = true;

		Texture temp = _panorama.material.mainTexture;
		_panorama.material.mainTexture = null;
		Resources.UnloadAsset(temp);
		temp = null;
	}

	public void ShowButtonsPanorama()
	{
		_showButtonsPanorama.SetActive (false);
		_hideButtonsPanorama.SetActive (true);
		_buttonsPanorama.SetActive (true);

		Input.compass.enabled = true;
	}

	public void HideButtonsPanorama()
	{
		_showButtonsPanorama.SetActive (true);
		_hideButtonsPanorama.SetActive (false);
		_buttonsPanorama.SetActive (false);

		Input.compass.enabled = false;
	}

	[SerializeField] PanoramaCameraController	_panoramaCameraController;
	[SerializeField] CameraController			_cameraController;
	[SerializeField] MeshRenderer	_panorama;

	[SerializeField] GameObject		_showButtonsPanorama;
	[SerializeField] GameObject		_hideButtonsPanorama;
	[SerializeField] GameObject		_buttonsPanorama;

	[SerializeField] ShowButton[]	_buttons;
	[SerializeField] GameObject[]	_show;
	[SerializeField] GameObject[]	_hide;
}
