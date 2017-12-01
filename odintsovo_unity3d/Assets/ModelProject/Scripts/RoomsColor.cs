using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsColor : MonoBehaviour
{
	public enum ColorType
	{
		color1,
		color2,
		color3,
		color4,
		color5
	}

	void Start()
	{
        if (_colorType == ColorType.color1)
        {
            _color = new Color(0.8f, 0.8f, 0.8f, 1);
        }
        else if (_colorType == ColorType.color2)
        {
            _color = new Color(0.65f, 0.65f, 0.65f, 1f);
        }
        else if (_colorType == ColorType.color3)
        {
            _color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else if (_colorType == ColorType.color4)
        {
            _color = new Color(0.35f, 0.35f, 0.35f, 1f);
        }
        else if (_colorType == ColorType.color5)
        {
            _color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }

        _colorActive = new Color(0, 0.6f, 0f, 1f);

		_mesh = gameObject.GetComponent<MeshRenderer>();
        SetDisable();
	}

    public void SetActive()
    {
        if (_mesh != null)
        {
            _mesh.material.color = _colorActive;
        }
    }

    public void SetDisable()
    {
        if (_mesh != null)
        {
            _mesh.material.color = _color;
        }
    }

    [SerializeField] ColorType		_colorType;
	MeshRenderer					_mesh;
    Color                           _color;
    Color                           _colorActive;
}
