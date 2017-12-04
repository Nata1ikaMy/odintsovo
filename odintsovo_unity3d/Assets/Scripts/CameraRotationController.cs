using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotationController : MonoBehaviour
{
	void Start()
	{
		_slider.onValueChanged.AddListener(Change);
	}

	void OnDestroy()
	{
		_slider.onValueChanged.RemoveListener(Change);
	}

	public void CamRotation(Vector3	angle)
	{
		StartCoroutine(RotationCoroutine(angle));
	}

	IEnumerator RotationCoroutine(Vector3 angle)
	{
		float time = TIME;
		while (time > 0)
		{
			float deltaTime = Time.deltaTime;
			float persent = deltaTime / time;
			Vector3 current = _camera.eulerAngles;

			_cameraController.CamRotation(Quaternion.Euler(current.x, current.y + (angle.y - current.y) * persent, current.z + (angle.z - current.z) * persent), true);
			_slider.value = current.x + (angle.x - current.x) * persent;

			time -= deltaTime;
			yield return null;
		}

		_slider.value = angle.x;
		_cameraController.CamRotation(Quaternion.Euler(angle), true);
		yield return null;
	}

	void Change(float value)
	{
		_text.text = ((int)value).ToString();
		_cameraController.CamRotation(Quaternion.Euler(new Vector3(value, _camera.eulerAngles.y, _camera.eulerAngles.z)), true);
	}

	[SerializeField] Slider				_slider;
	[SerializeField] CameraController	_cameraController;
	[SerializeField] Transform			_camera;

	[SerializeField] Text				_text;

	const float TIME = 1f; 
}
