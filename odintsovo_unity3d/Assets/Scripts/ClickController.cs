using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ClickController : MonoBehaviour
{
    public System.Action<string> 		ClickEvent;
	public System.Action<GameObject>	ClickObjEvent;

	void Update ()
    {
#if ! UNITY_EDITOR //&& (UNITY_ANDROID || UNITY_IOS)
        if (CameraController.IsPointerOverUIObject() || Input.touchCount > 1)
        {
            _timeStart = 0f;
        }
        else if (Input.touchCount == 1)
        {
            _timeStart += Time.deltaTime;
            _mousePosition = CameraController.mousePosition;
        }
        else if (Input.touchCount == 0 && _timeStart > 0 && _timeStart < _timeMaxClick)
        {
            Click(_mousePosition);
            _timeStart = 0f;
        }
        else
        {
            _timeStart = 0f;
        }
#else
        if (CameraController.IsPointerOverUIObject() || Input.GetMouseButton(1))
        {
            _timeStart = 0f;
        }
        else if (Input.GetMouseButton(0))
        {
            _timeStart += Time.deltaTime;
        }
        else if (_timeStart > 0 && _timeStart < _timeMaxClick)
        {
            Click(CameraController.mousePosition);
            _timeStart = 0f;
        }
        else
        {
            _timeStart = 0f;
        }
#endif        
	}

    void Click(Vector3 mouse)
    {
        Ray ray = _camera.ScreenPointToRay(mouse);
        RaycastHit[] hit = Physics.RaycastAll(ray);
		RaycastHit result = new RaycastHit();
		float distance = -1f;


        if (hit != null && hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                bool hitIsOk = true;

                if (_ignoreString != null && _ignoreString.Length > 0)
                {
                    for (int j = 0; j < _ignoreString.Length; j++)
                    {
                        if (hit[i].collider.name.Contains(_ignoreString[j]))
                        {
                            hitIsOk = false;
                            break;
                        }
                    }
                }

                if (hitIsOk && _onlyString.Length > 0)
                {
                    hitIsOk = false;
                    for (int  j = 0; j < _onlyString.Length; j++)
                    {
                        if (hit[i].collider.name.Contains(_onlyString[j]))
                        {
                            hitIsOk = true;
                            break;
                        }
                    }
                }

				if (hitIsOk && (distance < 0 || hit[i].distance < distance))
                {
					result = hit[i];
					distance = hit[i].distance;
                }
            }
        }

		if (distance > 0)
		{
			if (ClickEvent != null)
			{
				ClickEvent(result.collider.gameObject.name);
			}
			if (ClickObjEvent != null)
			{
				ClickObjEvent(result.collider.gameObject);
			}
		}
    }

    [SerializeField] Camera         _camera;
    [SerializeField] string[]       _ignoreString;
    [SerializeField] string[]       _onlyString;

    float       _timeStart = 0f;
    const float _timeMaxClick = 0.4f;

#if ! UNITY_EDITOR //&& (UNITY_ANDROID || UNITY_IOS)
    Vector3     _mousePosition;
#endif

}
