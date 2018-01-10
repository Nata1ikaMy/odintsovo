using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleMap : MonoBehaviour
{
	void Start()
	{
		_slider.onValueChanged.AddListener(ChangeSliderValue);
		ChangeSliderValue(_slider.value);
		StartCoroutine(LoadCoroutine());
	}

	void OnDestroy()
	{
		_slider.onValueChanged.RemoveListener(ChangeSliderValue);
	}

	IEnumerator LoadCoroutine()
	{
		yield return null;

		UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://maps.googleapis.com/maps/api/staticmap?" +
			"center=55.678782,%2037.290701" +
			"&zoom=" + _scale.ToString() + 
			"&size=640x640" +
			"&maptype=roadmap&key=AIzaSyCkhaIKtz1gHkE7XpO6pRkdzEBAv4auuU0" +
			"&path=color:0x00000000|fillcolor:0xFFFF0033|55.677287,37.291205|55.678817,37.289132|55.680037,37.292145|55.679455,37.293074|55.678880,37.291937|55.678163,37.293002" +
			"&path=color:blue|weight:5|55.672146,37.281650|55.673151,37.283645|55.673710,37.283892|55.676104,37.288983|55.676880,37.287774|55.678405,37.290988");


		yield return www.SendWebRequest();
		yield return null;


        if (www.isNetworkError)
		{
            Debug.Log(www.error);
        }
        else
		{
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			_image.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        }

		yield return null;
	}

	void ChangeSliderValue(float value)
	{
		_scale = Mathf.Clamp((int)value, (int)_slider.minValue, (int)_slider.maxValue);
		StartCoroutine(LoadCoroutine());
	}

	[SerializeField] Image		_image;
	[SerializeField] Slider		_slider;

	int							_scale;
}
