using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanoramaCameraController : MonoBehaviour 
{
	void Start()
	{
		canRotation = true;
	}

    void Update ()
    {
		if (canRotation && !_corutineRotation && CameraController.motion)
		{
			StartCoroutine(CorutineRotation());
			return;
		}
#if ! UNITY_EDITOR
		if (!_corutineScale && CameraController.two_touches)
		{
			StartCoroutine(Scale());
		}
#endif
	}

    IEnumerator CorutineRotation()
    {
        _corutineRotation = true;

        Vector3 position = CameraController.mousePosition;
        Vector3 delta;
        yield return null;

		while (CameraController.motion && canRotation)
        {
            delta = CameraController.mousePosition - position;

			_cameraTransform.localRotation *= Quaternion.Euler(0.05f * delta.y * Vector3.right);
			_panorama.localRotation *= Quaternion.Euler(0.1f * delta.x * Vector3.forward);

            position = CameraController.mousePosition;
            yield return null;
        }

        _corutineRotation = false;
        yield return null;
    }

#if ! UNITY_EDITOR
    IEnumerator Scale()
    {
        _corutineScale = true;

        float distance = delta;
        float distance1;

        while (CameraController.two_touches)
        {
            distance1 = delta;
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView + (distance - distance1) * 0.25f, _minFied, _maxFied);
            distance = distance1;
            yield return null;
        }

        _corutineScale = false;
        yield return null;
    }

    float delta
    {
        get
        {
            if (Input.touchCount == 2)
            {
                return (Input.touches[0].position - Input.touches[1].position).magnitude;
            }
            else
            {
                return 0;
            }
        }
    }
#endif

	public bool canRotation
	{
		get;
		set;
	}

    [SerializeField] Transform          _cameraTransform;
    [SerializeField] Camera             _camera;
    [SerializeField] Transform          _panorama;

	//[SerializeField] Text				_compassText;

    bool _corutineRotation = false;
    bool _corutineScale = false;

    float _minRotationVertical = 30f;
    float _maxRotationVertical = 150f;

    #if ! UNITY_EDITOR
    float _minFied = 30f;
    float _maxFied = 100f;
    #endif

    Vector3         _positionMainCamera;
    Quaternion      _rotationMainCamera;
    float           _fied;

	//float 			_angle;
}
