using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Collider itemInHands;
    private BoxCollider selfCollider;
    private bool canTakeItems = true;
    private float evaluatingTime;

    public float GrabRange;
    public AnimationCurve MovementSpeed;
    public Transform Hands;
    public LayerMask WhatCanBeTaken;
    public LayerMask WhereCanRelease;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        selfCollider = GetComponent<BoxCollider>();
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
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal_1"), 0f, Input.GetAxisRaw("Vertical_1")).normalized * MovementSpeed.Evaluate(evaluatingTime);
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
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhatCanBeTaken);
        if (colliders.Length > 0 && canTakeItems)
        {
            canTakeItems = false;
            itemInHands = colliders[0];
            itemInHands.transform.parent = transform;
            itemInHands.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
        }
    }

    public void Release()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhereCanRelease);
        if (colliders.Length > 0 && !canTakeItems)
        {
            canTakeItems = true;
            itemInHands.transform.position = new Vector3(colliders[0].transform.position.x, colliders[0].transform.position.y + colliders[0].transform.localScale.y / 2, colliders[0].transform.position.z);
            itemInHands.transform.parent = null;
            itemInHands = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Hands.position, GrabRange);
    }
}
