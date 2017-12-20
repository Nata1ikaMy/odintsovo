using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleMap : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(LoadCoroutine());
	}

	IEnumerator LoadCoroutine()
	{
		yield return null;
		UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://maps.googleapis.com/maps/api/staticmap?center=55.678782,%2037.290701&zoom=16&size=1500x800&maptype=roadmap&key=AIzaSyCkhaIKtz1gHkE7XpO6pRkdzEBAv4auuU0");



		yield return www.SendWebRequest();
		yield return null;

        if (www.isNetworkError)
		{
            Debug.Log(www.error);
        }
        else
		{
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			Image image = gameObject.GetComponent<Image>();
			image.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        }

		yield return null;
	}
}
