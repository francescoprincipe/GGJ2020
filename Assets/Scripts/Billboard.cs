using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool FollowCamera;

    void LateUpdate()
    {
        if (FollowCamera)
        {
            transform.LookAt(Camera.main.transform.position);
        }
        else
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
