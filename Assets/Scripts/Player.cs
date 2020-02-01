using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInput playerInput;

    private Rigidbody rb;
    private GameObject itemInHands;
    private BoxCollider selfCollider;
    private bool canTakeItems = true;

    private Vector3 currentDirection;

    public float GrabRange;
    public AnimationCurve MovementSpeed;
    public Transform Hands;
    public LayerMask WhatCanBeTaken;
    public LayerMask WhereCanRelease;
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        selfCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        currentDirection = new Vector3(Input.GetAxis(playerInput.HorizontalInputName), 0f, Input.GetAxis(playerInput.VerticalInputName));
        if (Input.GetButtonDown(playerInput.ItercationInputName))
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

    private void FixedUpdate()
    {
        if(canMove)
        {
            rb.velocity = currentDirection * 5;
            if (currentDirection != Vector3.zero)
            {
                transform.forward = currentDirection;
            }
        }
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhatCanBeTaken);
        if (colliders.Length > 0 && canTakeItems)
        {
            canTakeItems = false;
            itemInHands = colliders[0].gameObject;
            itemInHands.transform.parent = transform;
            itemInHands.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
        }
    }

    public void Release()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhereCanRelease);
        if (colliders.Length > 0 && !canTakeItems)
        {
            var workbench = colliders[0].gameObject.GetComponent<Workbench>();
            if (workbench != null) {
                workbench.SetItem(itemInHands, this);
            }
            else
            {
                itemInHands.transform.position = new Vector3(colliders[0].transform.position.x, colliders[0].transform.position.y + colliders[0].transform.localScale.y / 2, colliders[0].transform.position.z);
                itemInHands.transform.parent = null;
            }
            canTakeItems = true;
            itemInHands = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Hands.position, GrabRange);
    }
}
