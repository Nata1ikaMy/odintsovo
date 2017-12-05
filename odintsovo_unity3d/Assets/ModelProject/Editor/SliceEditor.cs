using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SliceEditor : MonoBehaviour
{
	[MenuItem("Tools/SetSliceController")]
	static void SetSliceController()
	{
		string build = "A_";
		int count = 27;

		GameObject go = GameObject.Find(build + "Slice_Controller");
		SliceController	controller = go.GetComponent<SliceController>();

		go = GameObject.Find(build + "SliceUI");
		Button[] button = go.GetComponentsInChildren<Button>();
		if (count != button.Length)
		{
			Debug.Log(string.Format("count != button.length; {0} != {1}", count, button.Length));
		}

		go = GameObject.Find(build + "build");
		Animator[] anim = go.GetComponentsInChildren<Animator>();
		if (count != anim.Length)
		{
			Debug.Log(string.Format("count != anim.length; {0} != {1}", count, anim.Length));
		}

		for (int i = 0; i < count; i++)
		{
			SliceController.Obj[] obj = new SliceController.Obj[4];
			for (int j = 0; j < 4; j++)
			{
				obj[j] = new SliceController.Obj();
			}

			Image[] image = button[i].GetComponentsInChildren<Image>(true);
			for (int j = 0; j < image.Length; j++)
			{
				if (image[j].gameObject.name == "activeImage")
				{
					obj[0].obj = image[j].gameObject;
					obj[0].whenShow = SliceController.WhenShow.before;
					break;
				}
			}


			go = GameObject.Find(string.Format("{0}show_animate ({1})", build, count- 1 - i));
			if (go == null)
			{
				Debug.Log(string.Format("{0}show_animate ({1}) == null", build, count- 1 - i));
			}
			obj[1].obj = go;
			obj[1].whenShow = SliceController.WhenShow.animate;


			go = GameObject.Find(string.Format("{0}show_only ({1})", build, count - 1 - i));
			if (go == null)
			{
				Debug.Log(string.Format("{0}show_only ({1}) == null", build, count - 1 - i));
			}
			obj[2].obj = go;
			obj[2].whenShow = SliceController.WhenShow.direction;


			go = GameObject.Find(string.Format("{0}text3d ({1})", build, count- 1 - i));
			if (go == null)
			{
				Debug.Log(string.Format("{0}text3d ({1}) == null", build, count- 1 - i));
			}
			obj[3].obj = go;
			obj[3].whenShow = SliceController.WhenShow.direction;

			controller.SetFloorEditor((count - 1 - i).ToString(), i, button[i], obj, anim[i]);
		}
	}

    [MenuItem("Tools/SetTextToRooms")]
    static void SetTextToRooms()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Rooms");
        GameObject go = GameObject.Find("Canvas3d");
        Text[] text = go.GetComponentsInChildren<Text>(true);

        for (int i = 0; i < rooms.Length; i++)
        {
            string name = rooms[i].name.Remove(0, 6);
            for (int j = 0; j < text.Length; j++)
            {
                if (text[j].name.Contains(name))
                {
                    rooms[i].GetComponent<RoomsColor>().textPol = text[j];
                    break;
                }
            }

            if (rooms[i].GetComponent<RoomsColor>().textPol == null)
            {
                Debug.Log(name);
            }
        }
    }
}
