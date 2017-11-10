using UnityEngine;
using System.Collections;

public class LookAtZ : MonoBehaviour {


    public Transform target;
    [SerializeField] private Transform[] canvasTransform;
    public bool lookAtClassic = false;
    private Vector3 targetPostition;
    [SerializeField] private float smooth = 2.0F;

    void Update ()
    {
        for (int i = 0; i < canvasTransform.Length; i++)
        {
            if (canvasTransform != null)
            {
                if (!lookAtClassic)
                {
                    targetPostition = new Vector3(target.position.x, canvasTransform[i].position.y, target.position.z);
                    canvasTransform[i].LookAt(targetPostition);
                }
                else
                    canvasTransform[i].localRotation = Quaternion.Euler(-90, target.rotation.eulerAngles.y + 180, canvasTransform[i].localRotation.z);
            }
        }
    }
}
