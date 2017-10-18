using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinmaxSlider : MonoBehaviour
{
	public System.Action ChangeEvent;

	void Start()
	{
		_min.onValueChanged.AddListener(ChangeMin);
		_max.onValueChanged.AddListener(ChangeMax);
	}

	void OnDestroy()
	{
		_min.onValueChanged.RemoveListener(ChangeMin);
		_max.onValueChanged.RemoveListener(ChangeMax);
	}

	void ChangeMin(float value)
	{
		if (value > _max.value)
		{
			_min.value = _max.value;
		}
		_minText.text = minValue.ToString();

		if (ChangeEvent != null)
		{
			ChangeEvent();
		}
	}

	void ChangeMax(float value)
	{
		if (value < _min.value)
		{
			_max.value = _min.value;
		}
		_maxText.text = maxValue.ToString();

		if (ChangeEvent != null)
		{
			ChangeEvent();
		}
	}

	public float minValue
	{
		get
		{
			return Mathf.FloorToInt(_min.value * _decimalPlaces) / (float)_decimalPlaces; 
		}
	}

	public float maxValue
	{
		get
		{
			return Mathf.CeilToInt(_max.value * _decimalPlaces) / (float)_decimalPlaces; 
		}
	}


	[SerializeField] Slider	_min;
	[SerializeField] Slider _max;
	[SerializeField] Text	_minText;
	[SerializeField] Text	_maxText;
	[SerializeField] int	_decimalPlaces;
}
