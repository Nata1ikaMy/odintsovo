using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class LightEditor : MonoBehaviour {

	[MenuItem("Tools/Light/SameObjName")]
	static void SameObjName()
	{
		GameObject go = GameObject.Find ("Model");
		MeshRenderer[] mesh = go.GetComponentsInChildren<MeshRenderer> (true);
		List<string> list = new List<string> ();
		for (int i = 0; i < mesh.Length; i++)
		{
			if (mesh [i].lightmapIndex >= 0 && mesh [i].lightmapIndex < 1000)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (mesh [i].name == list [j])
					{
						Debug.Log ("Same obj: " + list [j]);
						break;
					}
				}
				list.Add (mesh [i].name);
			}
		}
	}
}
