using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SliceController : MonoBehaviour
{
    public System.Action<int> ChangeFloorEvent;
    public enum State
    {
        show,
        animate,
        hide
    };

    public enum WhenShow
    {        
        animate,
        before,
        after,
		direction
    };

    [System.Serializable]
    public class Obj
    {
        public GameObject   obj;
        public WhenShow     whenShow;
    }

    [System.Serializable]
    public class Floor
    {
		[SerializeField] string			_floorNumber;
        [SerializeField] Button         _button;
        [SerializeField] Obj[]          _obj;
        [SerializeField] Animator       _animation;
		[SerializeField] float			_minY;

        SliceController                 _slice;
        bool                            _isShow = true;

        public int index
        {
            get;
            private set;
        }

        public float minY
        {
            get
            {
                return _minY;
            }
        }

		public void Set(string number, Button button, Obj[] obj, Animator anim)
		{
			_floorNumber = number;
			_button = button;
			_animation = anim;
			_obj = new Obj[obj.Length];
			for (int i = 0; i < obj.Length; i++)
			{
				_obj[i] = obj[i];
			}
		}

        public void Activate(SliceController slice, int setIndex)
        {
            _slice = slice;
            index = setIndex;
            _button.onClick.AddListener(Click);
        }

        public void Deactivate()
        {
            _button.onClick.RemoveListener(Click);
        }

        void Click()
        {
            _slice.Click(index);
        }

        public void SetState(State state, WhenShow when)
        {
            for (int i = 0; i < _obj.Length; i++)
            {
                if (state == State.animate && _obj[i].whenShow == WhenShow.animate)
                {
                    _obj[i].obj.SetActive(true);
                }
                else if (state == State.show && _obj[i].whenShow == WhenShow.animate)
                {
                    _obj[i].obj.SetActive(true);
                }
                else if (state == State.show && _obj[i].whenShow == WhenShow.before)
                {
                    _obj[i].obj.SetActive(true);
                }
				else if (state == State.show && when == WhenShow.after && (_obj[i].whenShow == WhenShow.after || _obj[i].whenShow == WhenShow.direction))
                {
                    _obj[i].obj.SetActive(true);
                }
				else if (state == State.show && when == WhenShow.direction && _obj[i].whenShow == WhenShow.direction)
				{
					_obj[i].obj.SetActive(true);
				}
                else
                {
                    _obj[i].obj.SetActive(false);
                }
            }
        }

        public bool Animate(bool show)
        {
            if (_isShow == show || _animation == null)
            {
                return false;
            }
            else
            {
                _isShow = show;
                if (show)
                {
                    _animation.Play("Animation_9_down");
                }
                else
                {
                    _animation.Play("Animation_9_up");
                }
                return true;
            }
        }

        public void Show(bool show)
        {
            _button.gameObject.SetActive(show);
        }
    }

    [System.Serializable]
    public class DuplexApart
    {
        [SerializeField] Texture        _floorTwo;
        [SerializeField] Texture        _floorOne;
        [SerializeField] MeshRenderer   _mesh;
        [SerializeField] int[]          _numberTwo;
        [SerializeField] string[]       _materialName;
        [SerializeField] string         _nameID;

        public void SetMaterial(int floor)
        {
            bool dark = false;
            for (int i = 0; i < _numberTwo.Length; i++)
            {
                if (_numberTwo[i] == floor)
                {
                    dark = true;
                    break;
                }
            }

            for (int i = 0; i < _mesh.materials.Length; i++)
            {
                for (int j = 0; j < _materialName.Length; j++)
                {
                    if (_mesh.materials[i].name.Contains(_materialName[j]))
                    {
                        _mesh.materials[i].SetTexture(_nameID, dark ? _floorTwo : _floorOne);
                    }
                }
            }
        }
    }

    void Start()
    {
        //активация кнопок
        for (int i = 0; i < _floor.Length; i++)
        {
            _floor[i].Activate(this, i);
        }
        //_clickController.ClickEvent += ClickApart;
		_indexFloor = _floor.Length - 1;
		Click(0);
		SetActiveButtons(false);
    }

    void OnDestroy()
    {
        for (int i = 0; i < _floor.Length; i++)
        {
            _floor[i].Deactivate();
        }
        //_clickController.ClickEvent -= ClickApart;
    }

    public void HideInfo()
    {
        if (_isShowInfo)
        {
            //_info.Hide();
            _isShowInfo = false;
        }
    }

    public void Show(bool show)
    {
        _isShow = show;
        //закрыть инфо, выделение зеленым
        if (! show)
        {
            HideInfo();
        }
        //закрыть кнопки
        for (int i = 0; i < _floor.Length; i++)
        {
            _floor[i].Show(show);
        }
    }

    public void ClickUpFloor()
    {
		Click(0);
    }

    public bool Click(int index) //возвращает ложь если уже выбран этот этаж
    {
        if (index == _indexFloor)
        {
            return false;
        }

        if (ChangeFloorEvent != null)
        {
            ChangeFloorEvent(index);
        }

		if (cameraTransform != null && cameraTransform.position.y < _floor[index].minY)
        {
            StartCoroutine(CorutineCameraPosition(_floor[index].minY));
        }

        StartCoroutine(CorutineShowObj(_indexFloor, index));
        StartCoroutine(CorutineAnimationFloor(index));

        _indexFloor = index;
        if (_isShowInfo)
        {
            //_info.Hide();
            _isShowInfo = false;
        }
        return true;
    }

    IEnumerator CorutineAnimationFloor(int index)
    {
        for (int i = 0; i < index; i++)
        {
            if (_floor[i].Animate(false))
            {
                yield return null;
            }
        }

        for (int i = _floor.Length - 1; i >= index; i--)
        {
            if (_floor[i].Animate(true))
            {
                yield return null;
            }
        }
    }

    IEnumerator CorutineShowObj(int currentIndex, int newIndex)
    {
        _lockObj.SetActive(true);

        if (currentIndex < newIndex)
        {
            for (int i = currentIndex; i < newIndex; i++)
            {
                _floor[i].SetState(State.animate, WhenShow.before);
            }
			_floor[newIndex].SetState(State.show, WhenShow.direction);
        }
        else if (currentIndex > newIndex)
        {
            for (int i = currentIndex; i > newIndex; i--)
            {
                _floor[i].SetState(State.animate, WhenShow.before);
            }
			_floor[newIndex].SetState(State.show, WhenShow.before);
        }
        
        if (newIndex > currentIndex)
        {
            for (int i = 0; i < _duplex.Length; i++)
            {
                _duplex[i].SetMaterial(newIndex);
            }
        }

        /////////////////////
        yield return new WaitForSeconds(1.4f);

        _floor[newIndex].SetState(State.show, WhenShow.after);

        for (int i = 0; i < _duplex.Length; i++)
        {
            _duplex[i].SetMaterial(newIndex);
        }

        /////////////////////
        yield return new WaitForSeconds(1f);

		if (newIndex == _indexFloor)
		{
	        for (int i = 0; i < _floor.Length; i++)
	        {
	            if (newIndex != i)
	            {
	                _floor[i].SetState(State.hide, WhenShow.after);
	            }
	        }
			_floor[newIndex].SetState(State.show, WhenShow.after);
		}

        ///////////////////
        yield return new WaitForSeconds(0.5f);
        _lockObj.SetActive(false);

        yield return null;
    }

    IEnumerator CorutineCameraPosition(float y)
    {
        float time = 1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y + (y - cameraTransform.position.y) * Time.deltaTime / time, cameraTransform.position.z);
            yield return null;
        }
        cameraTransform.position = new Vector3(cameraTransform.position.x, y, cameraTransform.position.z);
        yield return null;
    }


    void ClickApart(string id)
    {
        /*if (!_isShow || ! (id.Contains((9-_indexFloor).ToString() + "etazh") || _indexFloor == 1 && (id.Contains("7etazh_a_702") || id.Contains("7etazh_a_703"))))
        {
            return;
        }
        int number = int.Parse(id.Substring(id.Length-3, 3));*/

        /*int number = int.Parse(id);
        Base.Apartament apart = _base.GetById(number);
        if (apart != null)
        {
            _isShowInfo = true;
            List<Base.Apartament> apartList = new List<Base.Apartament>();
            apartList.Add(apart);
            _info.Create(apartList);
            _info.Show(0);
            if (_indexFloor == 9 && apart.id == 105 || _indexFloor == 1 && (apart.id == 702 || apart.id == 703))
            {
                _info.LevelClick(1);
            }
        }*/
    }

	public void SetFloorEditor(string number, int index, Button button, Obj[] obj, Animator anim)
	{		
		_floor[index].Set(number, button, obj, anim);
	}

	public void SetActiveButtons(bool active)
	{
		_buttons.SetActive(active);
	}

	public int indexFloor
	{
		get
		{
			return _indexFloor;
		}
	}


	public Transform		        		cameraTransform;

    [SerializeField] Floor[]                _floor;
    [SerializeField] DuplexApart[]          _duplex;
    [SerializeField] GameObject             _lockObj;
	[SerializeField] GameObject				_buttons;

    //[SerializeField] Base                   _base;
    //[SerializeField] InfoController         _info;

    int                 _indexFloor;

    bool                _isShowInfo = false;
    bool                _isShow = true;
}
