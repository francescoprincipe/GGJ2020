using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerInfo playerInfo;

    private Rigidbody rb;
    private GameObject itemInHands;
    private BoxCollider selfCollider;
    private float evaluatingTime;
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
        currentDirection = new Vector3(Input.GetAxisRaw(playerInput.HorizontalInputName), 0f, Input.GetAxisRaw(playerInput.VerticalInputName)).normalized;
        if (currentDirection != Vector3.zero)
        {
            evaluatingTime += Time.deltaTime;
        }
        if (Input.GetButtonDown(playerInput.InteractionInputName))
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
            rb.velocity = currentDirection * MovementSpeed.Evaluate(evaluatingTime);
            if (currentDirection != Vector3.zero)
            {
                transform.forward = currentDirection;
            }
            else
            {
                evaluatingTime = 0f;
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

            var pedestal = itemInHands.transform.parent.GetComponent<Pedestal>();
            if(pedestal != null)
                pedestal.GetComponent<Pedestal>().isOccupied = false;

            itemInHands.transform.parent = transform;
            itemInHands.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
        }
    }

    public void Release()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhereCanRelease);
        if (colliders.Length > 0 && !canTakeItems)
        {
            var iterable = colliders[0].gameObject.GetComponent<IInteractable>();
            if(iterable != null)
            {
                iterable.SetIdol(itemInHands.GetComponent<Idol>(), this);
                canTakeItems = true;
                itemInHands = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Hands.position, GrabRange);
    }
}
