using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomsColor : MonoBehaviour
{

	void Start()
	{
        MeshRenderer[] mesh = gameObject.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i].name.Contains("Rooms"))
            {
                _mesh = mesh[i];
            }
            else if (mesh[i].name.Contains("vnesh_col"))
            {
                _child = mesh[i].gameObject;
            }
        }

        _color = new Color(0.8f, 0.8f, 0.8f, 1);
        _colorActive = new Color(0, 200f/255f, 0f, 1f);

        SetDisable();
	}

    public void SetActive()
    {
        if (_mesh != null)
        {
            _mesh.material.color = _colorActive;
        }
        _child.SetActive(true);
    }

    public void SetDisable()
    {
        if (_mesh != null)
        {
            _mesh.material.color = _color;
        }
        _child.SetActive(false);
    }

    public void SetInfo(Base.Apartament apart)
    {
        if (apart.room == 0)
        {
            _color = new Color(1f, 165f/255f, 165f/255f, 1f); //красный
        }
        else if (apart.room == 1)
        {
            _color = new Color(165f/255f, 165f/255f, 1f, 1f); // синий
        }
        else if (apart.room == 2)
        {
            _color = new Color(1f, 1f, 165f/255f, 1f); // желтый
        }
        else if (apart.room == 3)
        {
            _color = new Color(1f, 165f/255f, 1f, 1f); //розовый
        }
        else if (apart.room == 4)
        {
            _color = new Color(165f/255f, 1f, 1f, 1f); //голубой
        }

        if (textPol != null)
        {
			textPol.text = string.Format("№{0}\n{1}\n{2} кв.м.", apart.number, apart.room == 0 ? "Студия" : ("Комнат: " + apart.room), apart.square);
        }
        SetDisable();
    }

    public Text                     textPol;

	MeshRenderer					_mesh;
    GameObject                      _child;
    Color                           _color;
    Color                           _colorActive;
}
