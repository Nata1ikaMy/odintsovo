using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
	public System.Action<Vector3, bool> 			ObjPositionEvent; //true - immediately
	public System.Action<Vector3, Vector3, float> 	ObjRotationAroundEvent;
	public System.Action<Quaternion, bool> 			ObjRotationEvent;

	public System.Action<Vector3, bool> 			CamPositionEvent; //true - immediately
	public System.Action<Vector3, Vector3, float> 	CamRotationAroundEvent;
	public System.Action<Quaternion, bool> 			CamRotationEvent;

	public System.Action<float> 					CamFiedOfViewEvent;

	void Start()
	{
		_plane = new Plane(Vector3.up, _obj.position);
		_cameraTransform = _camera.gameObject.transform;
	}

	public void ObjPosition(Vector3 position, bool immediately)
	{
		if (immediately)
		{
			_obj.position = position;
		}
		else
		{
			StartSmoothMotion(position);
		}

		if (ObjPositionEvent != null)
		{
			ObjPositionEvent(position, immediately);
		}
	}

	public void ObjRotationAround(Vector3 point, Vector3 axis, float angle)
	{
		_obj.RotateAround(point, axis, angle);

		if (ObjRotationAroundEvent != null)
		{
			ObjRotationAroundEvent(point, axis, angle);
		}
	}

	public void ObjRotation(Quaternion angle, bool immediately)
	{
		if (immediately)
		{
			_obj.rotation = angle;
		}
		else
		{
			StartSmoothRotation(angle);
		}

		if (ObjRotationEvent != null)
		{
			ObjRotationEvent(angle, immediately);
		}
	}

	public void CamPosition(Vector3 position, bool immediately)
	{
		if (immediately)
		{
            if (! PointIntoCollider(position))
            {
                return;
            }
			_parentCamera.position = position;
		}
		else
		{
			StartSmoothMotion(position);
		}

		if (CamPositionEvent != null)
		{
			CamPositionEvent(position, immediately);
		}
	}

	public void CamRotationAround(Vector3 point, Vector3 axis, float angle)
	{
        if (! RotationAround(point, angle))
        {
            return;
        }
		_parentCamera.RotateAround(point, axis, angle);
		if (CamRotationAroundEvent != null)
		{
			CamRotationAroundEvent(point, axis, angle);
		}
	}

	public void CamRotation(Quaternion angle, bool immediately)
	{
		if (immediately)
		{
			_parentCamera.rotation = angle;
		}
		else
		{
			StartSmoothRotation(angle);
		}

		if (CamRotationEvent != null)
		{
			CamRotationEvent(angle, immediately);
		}
	}

	public void CamFiedOfView(float fied)
	{
		_camera.fieldOfView = fied;
		if (CamFiedOfViewEvent != null)
		{
			CamFiedOfViewEvent(fied);
		}
	}

    bool PointIntoCollider(Vector3 point)
    {
		if ((_collider == null || _collider.Length == 0) && string.IsNullOrEmpty(_tagCollider))
        {
            return true;
        }

        int count = 0; //количество коллайдеров для заданной точки
		//string debug = "up:";

        Ray ray = new Ray(point, Vector3.up);
        RaycastHit[] hit = Physics.RaycastAll(ray);
        if (hit != null && hit.Length != 0)
        {
            for (int i=0; i<hit.Length; i++)
            {
				if (string.IsNullOrEmpty(_tagCollider)) //указание коллайдера прямыми ссылками
				{
					for (int j = 0; j < _collider.Length; j++)
					{
						if (hit[i].collider.gameObject == _collider[j])
						{
							//debug += hit[i].collider.name + ", ";
							count++;
							break;
						}
					}
				}
				else if (hit[i].collider.gameObject.tag == _tagCollider) //коллайдер по тегу
				{
					count++;
				}
            }
        }

		//debug += "down:";
        ray = new Ray(point, Vector3.down);
        hit = Physics.RaycastAll(ray);
        if (hit != null && hit.Length != 0)
        {
            for (int i=0; i<hit.Length; i++)
            {
				if (string.IsNullOrEmpty(_tagCollider)) //указание коллайдера прямыми ссылками
				{
					for (int j = 0; j < _collider.Length; j++)
					{
						if (hit[i].collider.gameObject == _collider[j])
						{
							//debug += hit[i].collider.name + ", ";
							count++;
							break;
						}
					}
				}
				else if (hit[i].collider.gameObject.tag == _tagCollider)
				{
					count++;
				}
            }
        }

		//debug += "max:";
        int countHighest = 0; // max collider
        point.y = 2 * _maxY;
        ray = new Ray(point, Vector3.down);
        hit = Physics.RaycastAll(ray);
        if (hit != null && hit.Length != 0)
		{
            for (int i=0; i<hit.Length; i++)
            {
				if (string.IsNullOrEmpty(_tagCollider)) //указание коллайдера прямыми ссылками
				{
					for (int j = 0; j < _collider.Length; j++)
					{
						if (hit[i].collider.gameObject == _collider[j])
						{
							//debug += hit[i].collider.name + ", ";
							countHighest++;
							break;
						}
						else
						{
							//debug += hit[i].collider.name.ToUpper() + ", ";
						}
					}
				}
				else  if (hit[i].collider.gameObject.tag == _tagCollider)
				{
					countHighest++;
				}
            }
        }

		//debug += string.Format ("result: {0} > {1}", count, countHighest);
		//Debug.Log (debug);
        if (count < countHighest)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool RotationAround(Vector3 point, float angle)
    {
        Vector3 camPosition = _parentCamera.position - point;
        float sin = Mathf.Sin(-Mathf.Deg2Rad * angle);
        float cos = Mathf.Cos(-Mathf.Deg2Rad * angle);
        Vector3 newCamPosition = new Vector3();

        newCamPosition.x = camPosition.x * cos - camPosition.z * sin;
        newCamPosition.y = camPosition.y;
        newCamPosition.z = camPosition.x * sin + camPosition.z * cos;

        newCamPosition += point;

        return PointIntoCollider(newCamPosition);
    }

	void Update()
	{
		if (!_motionCoroutine && motion)
		{
			StartCoroutine(Motion());
			return;
		}

#if ! UNITY_EDITOR

		if (!_twoTouchesCoroutine && two_touches)
		{
			StartCoroutine(TwoTouches());
			return;
		}

#else
		if (! _rotationMouseCoroutine && Input.GetMouseButton(1))
		{
			if (_rotationAroundOnlyMouse)
			{
				StartCoroutine(RotationMouseAround());
			}
			else
			{
				StartCoroutine(RotationMouse());
			}
		}

		//scale колесиком мыши
		if (Input.mouseScrollDelta.y != 0)
		{
			if (_changeFied)
			{
				CamFiedOfView(Mathf.Clamp(_camera.fieldOfView - Input.mouseScrollDelta.y, _minFied, _maxFied));
			}
			else if (_transformObj)
			{
				Vector3 position = _obj.position;
				position.y = Mathf.Clamp(position.y + Input.mouseScrollDelta.y * _fiedToY, _minY, _maxY);
                ObjPosition(position, true);
			}
			else
			{
				Vector3 position = _parentCamera.position;
				position.y = Mathf.Clamp(position.y - Input.mouseScrollDelta.y * _fiedToY*0.1f, _minY, _maxY);
				CamPosition(position, true);
			}
		}

#endif

	}

	IEnumerator Motion()
	{
		_motionCoroutine = true;
		
		Ray ray;
		float distance;
		Vector3 controlPoint;
		Vector3 positionObj;
		
		while (motion)
		{
			ray = _camera.ScreenPointToRay(mousePosition);
			_plane.Raycast(ray, out distance);		
			controlPoint = ray.GetPoint(distance);

			yield return null;

			if (!motion)
			{
				break;
			}

			ray = _camera.ScreenPointToRay(mousePosition);
			_plane.Raycast(ray, out distance);
			positionObj = ray.GetPoint(distance) - controlPoint;

			if (_transformObj)
			{
				positionObj += _obj.position;
				positionObj.x = Mathf.Clamp(positionObj.x, _minX, _maxX);
				positionObj.y = Mathf.Clamp(positionObj.y, _minY, _maxY);
				positionObj.z = Mathf.Clamp(positionObj.z, _minZ, _maxZ);

				ObjPosition(positionObj, true);
			}
			else
			{
				positionObj = _parentCamera.position - positionObj;
				positionObj.x = Mathf.Clamp(positionObj.x, _minX, _maxX);
				positionObj.y = Mathf.Clamp(positionObj.y, _minY, _maxY);
				positionObj.z = Mathf.Clamp(positionObj.z, _minZ, _maxZ);

				CamPosition(positionObj, true);
			}
		}

		_motionCoroutine = false;
	}

#if ! UNITY_EDITOR

	IEnumerator TwoTouches()
	{
		_twoTouchesCoroutine = true;


		Ray ray;
		Ray ray1;
		float distance;
		float distance1;
		Vector3 controlPoint;
		Vector3 controlPoint1;

		float deltaControlPoints;

		Ray newRay;
		float newDistance;
		Vector3 newControlPoint;
		Vector3 newControlPoint1;
		float delta;
		float delta1;

		Vector3 positionObj;

		while (two_touches)
		{
			_plane = new Plane(Vector3.up, _obj.position);

			ray = _camera.ScreenPointToRay(TouchPosition(0));
			_plane.Raycast(ray, out distance);
			controlPoint = ray.GetPoint(distance);

			ray1 = _camera.ScreenPointToRay(TouchPosition(1));
			_plane.Raycast(ray1, out distance1);
			controlPoint1 = ray1.GetPoint(distance1);

			deltaControlPoints = (controlPoint1 - controlPoint).magnitude;

			yield return null;

			if (!two_touches)
			{
				break;
			}

			//SCALE
			newRay = _camera.ScreenPointToRay(TouchPosition(0));
			_plane.Raycast(newRay, out newDistance);
			newControlPoint = newRay.GetPoint(newDistance);//первый тач указывает на эту точку без учета движения камеры
			delta = Projection(controlPoint1 - controlPoint, newControlPoint - controlPoint); //на это число сократилось расстояние между тачами со стороны нулевого тача
			delta = distance1 - distance1 * (deltaControlPoints - delta) / deltaControlPoints; // deltaControlPoints / (deltaControlPoints - delta) = distance1 / (distance1 - X)


			newRay = _camera.ScreenPointToRay(TouchPosition(1));
			_plane.Raycast(newRay, out newDistance);
			newControlPoint1 = newRay.GetPoint(newDistance);
			delta1 = Projection(controlPoint - controlPoint1, newControlPoint1 - controlPoint1);
			delta1 = distance - distance * (deltaControlPoints - delta1) / deltaControlPoints;

			if (Mathf.Abs(delta) > 0.1f && Mathf.Abs(delta1) > 0.1f)
			{
				positionObj = (controlPoint - _cameraTransform.position).normalized * delta1 + (controlPoint1 - _cameraTransform.position).normalized * delta;

				if (_changeFied)
				{
					CamFiedOfView(Mathf.Clamp(_camera.fieldOfView + positionObj.y / _fiedToY, _minFied, _maxFied));
				}
				else if (_transformObj)
				{
					positionObj += _obj.position;
					positionObj.x = Mathf.Clamp(positionObj.x, _minX, _maxX);
					positionObj.y = Mathf.Clamp(positionObj.y, _minY, _maxY);
					positionObj.z = Mathf.Clamp(positionObj.z, _minZ, _maxZ);

					ObjPosition(positionObj, true);
				}
				else
				{
					positionObj = _parentCamera.position - positionObj;
					positionObj.x = Mathf.Clamp(positionObj.x, _minX, _maxX);
					positionObj.y = Mathf.Clamp(positionObj.y, _minY, _maxY);
					positionObj.z = Mathf.Clamp(positionObj.z, _minZ, _maxZ);

					CamPosition(positionObj, true);
				}
			}
			//ВРАЩЕНИЕ
			//относительно первого нажатия
			float angle = AngleSigned(newControlPoint - controlPoint1, controlPoint - controlPoint1);

			if (_transformObj)
			{
				ObjRotationAround(controlPoint1, Vector3.up, angle);
			}
			else
			{
				CamRotationAround(controlPoint1, Vector3.up, -angle);
			}

			//вращение относительного второго нажатия
			angle = AngleSigned(newControlPoint1 - controlPoint, controlPoint1 - controlPoint);

			if (_transformObj)
			{
				ObjRotationAround(controlPoint, Vector3.up, angle);
			}
			else
			{
				CamRotationAround(controlPoint1, Vector3.up, -angle);
			}
		}

		_twoTouchesCoroutine = false;
	}

	Vector3 TouchPosition(int index)
	{
		if (index < Input.touchCount)
		{
			Touch touch = Input.GetTouch(index);
			return new Vector3(touch.position.x, touch.position.y, 0);
		}
		else
		{
			return Vector3.zero;
		}
	}

#else
	IEnumerator RotationMouse()
	{
		_rotationMouseCoroutine = true;

		Vector3 mousePos = Input.mousePosition;

		Vector3 delta;
		Vector3 currentRotation;

		yield return null;

		while (Input.GetMouseButton(1))
		{
			delta = (Input.mousePosition - mousePos) * 0.5f;

			if (_transformObj)
			{
				currentRotation = _obj.rotation.eulerAngles;
				currentRotation.x = Mathf.Clamp (currentRotation.x + delta.y, _minRotationVertical, _maxRotationVertical);
				currentRotation.y -= delta.x;

				ObjRotation(Quaternion.Euler(currentRotation), true);
			}
			else
			{
				currentRotation = _parentCamera.rotation.eulerAngles;
				currentRotation.x = Mathf.Clamp (currentRotation.x - delta.y, _minRotationVertical, _maxRotationVertical);
				currentRotation.y += delta.x;

				CamRotation(Quaternion.Euler(currentRotation), true);
			}

			mousePos = Input.mousePosition;
			yield return null;
		}

		_rotationMouseCoroutine = false;
	}

	IEnumerator RotationMouseAround()
	{
		_rotationMouseCoroutine = true;

		Vector3 delta;
		Vector3 mousePos = mousePosition;
		Ray ray = _camera.ScreenPointToRay(mousePos);

		float distance;
		_plane.Raycast(ray, out distance);

		RaycastHit[] hit = Physics.RaycastAll(ray);
		int index = GetNearHit(hit);
		if (index >= 0 && hit[index].collider.gameObject.transform.position.y > _minPositionForRotation)
		{
			distance = hit[index].distance;
			Debug.Log(string.Format("ROTATION AROUND name={0} y={1}", hit[index].collider.gameObject.name, hit[index].collider.gameObject.transform.position.y));
		}

		Vector3 controlPoint = ray.GetPoint(distance);

		while (Input.GetMouseButton(1))
		{
			delta = (mousePosition - mousePos) * 0.5f;

			//ВРАЩЕНИЕ
			float angle = delta.x;

			if (_transformObj)
			{
				ObjRotationAround(controlPoint, Vector3.up, -angle);
			}
			else
			{
				CamRotationAround(controlPoint, Vector3.up, angle);
			}

			mousePos = mousePosition;
			//ray = _camera.ScreenPointToRay(mousePos);
			//_plane.Raycast(ray, out distance);
			//controlPoint = ray.GetPoint(distance);

			yield return null;
		}

		_rotationMouseCoroutine = false;
	}

#endif

	float Projection(Vector3 to, Vector3 from) //проекиця вектора from на ось to
	{
		return (to.x * from.x + to.y * from.y + to.z * from.z) / to.magnitude;
	}

	public static float AngleSigned(Vector3 from, Vector3 to) //угол между векторами -180...180
	{
		Vector3 norm = new Vector3(-from.z, 0, from.x);
		int sgn = Vector3.Angle(norm, to) < 90f ? 1 : -1;
		return sgn * Vector3.Angle(from, to);
	}

	public static Vector3 mousePosition
	{
		get
		{
#if ! UNITY_EDITOR
			if (Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);
				return new Vector3(touch.position.x, touch.position.y, 0);
			}
			else
			{
				return Vector3.zero;
			}
#else
			return Input.mousePosition;			
#endif
		}
	}

	public static bool motion
	{
		get
		{
            if (IsPointerOverUIObject())
            {
                return false;
            }

#if ! UNITY_EDITOR
            if (Input.touchCount != 1)
			{
                return false;
			}
			else
			{
                return true;
			}
#else
            if (! Input.GetMouseButton(0))
			{
				return false;
			}
			else
			{
				return true;
			}
#endif
		}
	}
	
#if ! UNITY_EDITOR
    public static bool two_touches
	{
		get
		{
            if (IsPointerOverUIObject() || Input.touchCount != 2)
            {
                return false;
            }
			else
			{
                return true;
			}
		}
	}
#endif

    public static bool IsPointerOverUIObject()
    {
#if ! UNITY_EDITOR
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId))
            {
                return true;
            }
        }
        return false;
#else
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }

	void StartSmoothMotion(Vector3 position)
	{
		_position = position;
		_timeMotion = _time;
		if (! _isMotion)
		{
			StartCoroutine(SmoothMotion());
		}
	}

	void StartSmoothRotation(Quaternion angle)
	{
		_rotation = angle.eulerAngles;
		_timeRotation = _time;
		if (!_isRotation)
		{
			StartCoroutine(SmoothRotation());
		}
	}

	IEnumerator SmoothMotion() //плавное движение к определенному положению
	{
		_isMotion = true;
		float delta;
		Vector3 position;

		while (_timeMotion > 0)
		{
			delta = Time.deltaTime;
			if (_timeMotion > delta)
			{
				position = _transformObj ? _obj.position : _parentCamera.position;
				position += (_position - position) * delta / _timeMotion;
			}
			else
			{
				position = _position;
			}

			if (_transformObj)
			{
				_obj.position = position;
			}
			else
			{
				_parentCamera.position = position;
			}

			_timeMotion -= delta;
			yield return null;
		}

		_isMotion = false;
	}

	IEnumerator SmoothRotation() //плавное вращение к опредленному положению
	{
		_isRotation = true;
		float delta;
		Vector3 currentRotation;

		while (_timeRotation > 0)
		{
			delta = Time.deltaTime;
			if (_timeRotation > delta)
			{
				currentRotation = _transformObj ? _obj.rotation.eulerAngles : _parentCamera.rotation.eulerAngles;
				currentRotation = currentRotation + (_rotation - currentRotation) * delta / _timeRotation;
			}
			else
			{
				currentRotation = _rotation;
			}

			if (_transformObj)
			{
				_obj.rotation = Quaternion.Euler(currentRotation);
			}
			else
			{
                _parentCamera.rotation = Quaternion.Euler(currentRotation);
			}

			_timeRotation -= delta;
			yield return null;
		}

		_isRotation = false;
	}

	public static int GetNearHit(RaycastHit[] hit)
	{
		int result = -1;
		for (int i = 0; i < hit.Length; i++)
		{
			if (hit[i].collider != null && (result < 0 || hit[i].distance < hit[result].distance))
			{
				result = i;
			}
		}
		return result;
	}

	[SerializeField] Camera 		_camera;
	[SerializeField] Transform 		_obj;
	[SerializeField] Transform 		_parentCamera;
	[SerializeField] bool 			_transformObj;	//true - будет двигаться _obj, false - _parentCamera
	[SerializeField] bool			_rotationAroundOnlyMouse;
	[SerializeField] float 			_minPositionForRotation;
	[SerializeField] string			_tagCollider;
    [SerializeField] GameObject[]   _collider;

	[SerializeField] int _minX;
	[SerializeField] int _maxX;
	[SerializeField] int _minY;
	[SerializeField] int _maxY;
	[SerializeField] int _minZ;
	[SerializeField] int _maxZ;
	[SerializeField] int _minFied;
	[SerializeField] int _maxFied;
	[SerializeField] int _minRotationVertical;
	[SerializeField] int _maxRotationVertical;
	[SerializeField] bool _changeFied;
	[SerializeField] int _fiedToY;

	Plane 		        _plane;
	Transform 	        _cameraTransform;


	//плавное движение/вращение
	bool		_isMotion = false;
	bool		_isRotation = false;
	Vector3 	_position;
	float		_timeMotion;
	Vector3		_rotation;
	float		_timeRotation;

	//время плавного движения до контрольной точки
	const float _time = 1f;

	//движение/вращение при управлении
	bool 		        _motionCoroutine = false;
#if ! UNITY_EDITOR
	bool 		        _twoTouchesCoroutine = false;
#else
	bool		_rotationMouseCoroutine = false;
#endif
}
