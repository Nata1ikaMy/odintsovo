// Marmoset Skyshop
// Copyright 2013 Marmoset LLC
// http://marmoset.co

using UnityEngine;
using System.Collections;

public class ProbeDemo : MonoBehaviour {
	private bool spinning = true;
	public float guiAlpha = 0.8f;
	private float helpAlpha = 0.25f;
	public Transform mesh = null;
	
	public Texture2D helpTex = null;
	private Color helpColor = new Color(1f,1f,1f,0f);
	private float targetExposure = 1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		mset.SkyManager manager = mset.SkyManager.Get();
		if(manager && manager.GlobalSky) {
			mset.Sky globalSky = manager.GlobalSky;

			if( Input.GetKeyDown(KeyCode.S) ) spinning = !spinning;		

			if( Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadEquals) ) {
				targetExposure = Mathf.Min( targetExposure+0.2f, 2f );
			}
			if( Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus) ) {
				targetExposure = Mathf.Max( 0.05f, targetExposure-0.2f );
			}

			if( Mathf.Abs( globalSky.CamExposure - targetExposure )  > 0.01f ) {
				globalSky.CamExposure = 0.95f*globalSky.CamExposure + 0.05f*targetExposure;
			} else {
				globalSky.CamExposure = targetExposure;
			}
		}
	}
	
	Vector3 angularVel = new Vector3(0,6f,0);
	void FixedUpdate() {
		if( spinning && mesh ) {
			mesh.transform.Rotate(angularVel*Time.fixedDeltaTime);
		}
	}
	
	void OnGUI() {
		Rect texRect = GetComponent<Camera>().pixelRect;
		helpColor.a = guiAlpha;
		
		if( helpTex ) {
			texRect.width = 0.75f*helpTex.width;
			texRect.height = 0.75f*helpTex.height;
			texRect.y = -50;//camera.pixelHeight - texRect.height + 40;
			texRect.x = GetComponent<Camera>().pixelWidth - texRect.width;
			
			Rect hoverRect = texRect;
			hoverRect.x += 0.5f*hoverRect.width;			
			hoverRect.width *= 0.5f;
			Vector3 mouse = Input.mousePosition;
			mouse.y = GetComponent<Camera>().pixelHeight-mouse.y;
			if( hoverRect.Contains(mouse) )	helpAlpha = Mathf.Lerp(helpAlpha,1.00f,0.01f);
			else 							helpAlpha = Mathf.Lerp(helpAlpha,0.25f,0.01f);
			helpColor.a = helpAlpha * guiAlpha;
			GUI.color = helpColor;
			GUI.DrawTexture(texRect, helpTex);
		}
	}
}
