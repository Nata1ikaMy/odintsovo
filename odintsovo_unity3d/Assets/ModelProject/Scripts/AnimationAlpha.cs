using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAlpha : MonoBehaviour
{
	void Update()
	{
		if (_up)
		{
			_alpha += Time.deltaTime;
		}
		else
		{
			_alpha -= Time.deltaTime;
		}

		if (_alpha >= 0.8f)
		{
			_up = false;
		}
		if (_alpha <= 0.3f)
		{
			_up = true;
		}

		if (_mesh == null)
		{
			_mesh = GetComponent<MeshRenderer>();
		}

		if (_mesh != null)
		{
			Color color = _mesh.material.color;
			color.a = _alpha;
			_mesh.material.color = color;
		}
	}

	bool _up = true;
	float _alpha = 0.5f;
	MeshRenderer _mesh;
}
