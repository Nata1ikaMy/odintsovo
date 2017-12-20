using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleMap : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(LoadCoroutine());
	}

	IEnumerator LoadCoroutine()
	{
		yield return null;
		UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://www.my-server.com/image.png");
        yield return www.Send();

        if (www.isNetworkError)
		{
            Debug.Log(www.error);
        }
        else
		{
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }

		yield return null;
	}
}
