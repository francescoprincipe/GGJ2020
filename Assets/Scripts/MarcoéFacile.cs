using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcoéFacile : MonoBehaviour
{
    private float movementSpeed = 25f;
    private Vector3 startPoint;
    private Vector3 endPoint;

    private void Start()
    {
        startPoint = transform.position;
        endPoint = transform.position + Vector3.forward * 150f;
    }

    private void Update()
    {
        transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        if (transform.position.z >= endPoint.z)
        {
            transform.position = startPoint;
        }

    }
}
