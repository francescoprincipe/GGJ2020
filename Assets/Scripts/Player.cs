using System.Collections;
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

    [HideInInspector]
    public PlayerImage Image;
    public float GrabRange;
    public AnimationCurve MovementSpeed;
    public Transform Hands;
    public LayerMask WhatCanBeTaken;
    public LayerMask WhereCanRelease;

    private bool canSprint = true;
    private bool stunned = false;
    private bool isSprinting = false;
    private float sprintMultilyer=1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        selfCollider = GetComponent<BoxCollider>();
        Image = GetComponentInChildren<PlayerImage>();
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

        if (canSprint && (Input.GetButtonDown(playerInput.DashInputName)))
        {
            Debug.Log("Tryng to sprint");
            StartCoroutine("Sprint");
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = currentDirection * sprintMultilyer * MovementSpeed.Evaluate(evaluatingTime);
        if (currentDirection != Vector3.zero)
        {
            transform.forward = currentDirection;
        }
        else
        {
            evaluatingTime = 0f;
        }
    }

    private  IEnumerator Sprint()
    {
        Debug.Log("SPRINTTTT");
        isSprinting = true;
        canSprint = false;
        stunned = true;
        sprintMultilyer = 3;
        yield return new WaitForSeconds(.2f);
        sprintMultilyer = 1;
        canSprint = true;
        isSprinting = false;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (isSprinting && coll.gameObject.name.Equals("Player 2"))
        {
            Destroy(gameObject);
            Destroy(coll.gameObject);
        }
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhatCanBeTaken);
        if (colliders.Length > 0 && canTakeItems)
        {
            canTakeItems = false;
            itemInHands = colliders[0].gameObject;

            Pedestal pedestal = itemInHands.transform.parent.GetComponent<Pedestal>();
            if (pedestal != null)
            {
                pedestal.isOccupied = false;
            }

            Altar altar = itemInHands.transform.parent.GetComponent<Altar>();
            if (altar != null)
            {
                altar.ResetAltar();
            }

            Image.ChangeImageState();
            itemInHands.transform.parent = transform;
            itemInHands.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
        }
    }

    public void Release()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhereCanRelease);
        if (colliders.Length > 0 && !canTakeItems)
        {
            var interactable = colliders[0].gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.SetIdol(itemInHands.GetComponent<Idol>(), this);
                Image.ChangeImageState();
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
