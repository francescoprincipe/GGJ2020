using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Collider itemInHands;
    private bool canTakeItems = true;
    private float evaluatingTime;

    public AnimationCurve MovementSpeed;
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
        evaluatingTime += Time.deltaTime;
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal_1") * MovementSpeed.Evaluate(evaluatingTime), 0f, Input.GetAxisRaw("Vertical_1") * MovementSpeed.Evaluate(evaluatingTime));
        Flip(rb.velocity.normalized);
        if (rb.velocity == Vector3.zero)
        {
            evaluatingTime = 0f;
        }
    }

    public void Flip(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
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
