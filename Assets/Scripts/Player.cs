using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Collider itemInHands;
    private bool canTakeItems = true;

    public float MovementSpeed;
    public Transform Hands;
    public LayerMask WhatCanBeTaken;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canTakeItems)
            {
                Interact();
            }
            else
            {
                Release();
            }
        }
    }

    public void Move()
    {
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal_1") * MovementSpeed * 100 * Time.deltaTime, 0f, Input.GetAxisRaw("Vertical_1") * MovementSpeed * 100 * Time.deltaTime);
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, 0.5f, WhatCanBeTaken);
        if (colliders.Length > 0 && canTakeItems)
        {
            itemInHands = colliders[0];
            canTakeItems = false;
            itemInHands.transform.parent = transform;
        }
    }

    public void Release()
    {
        if (!canTakeItems)
        {
            canTakeItems = true;
            itemInHands.transform.parent = null;
        }
    }
}
