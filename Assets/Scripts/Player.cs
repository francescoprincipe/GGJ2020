using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerInput playerInput;
    public PlayerInfo playerInfo;
    public int playerIndex;

    private Rigidbody rb;
    private Idol itemInHands;
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

    public float sprintCooldown = 4f;
    public float sprintLenght = .1f;
    private bool canSprint = true;
    private bool stunned = false;
    private bool isSprinting = false;
    private float dashMultiplier = 1;
    public float stunTime = 3f;

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

        if (canSprint && (Input.GetButtonDown(playerInput.DashInputName)) && rb.velocity != Vector3.zero)
        {
            StartCoroutine(Sprint());
        }

    }

    private void FixedUpdate()
    {

        rb.velocity = currentDirection * dashMultiplier * MovementSpeed.Evaluate(evaluatingTime);
        if (currentDirection != Vector3.zero)
        {
            transform.forward = currentDirection;
        }
        else
        {
            evaluatingTime = 0f;
        }
    }

    private IEnumerator Sprint()
    {
        StartCoroutine(SprintCooldown());
        evaluatingTime = 1;
        isSprinting = true;
        dashMultiplier = 3;
        yield return new WaitForSeconds(sprintLenght);
        dashMultiplier = 1;
        isSprinting = false;
    }

    private IEnumerator SprintCooldown()
    {
        canSprint = false;
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = true;
    }


    private void OnCollisionEnter(Collision other)
    {

        if (isSprinting && (other.gameObject.tag == "Player"))
        {
            other.gameObject.GetComponent<Player>().Stun();
        }

    }
    public void Stun()
    {
        StartCoroutine(StunCoroutine());
    }
    private IEnumerator StunCoroutine()
    {
        dashMultiplier = 0;
        stunned = true;
        //animazione stun
        yield return new WaitForSeconds(stunTime);
        dashMultiplier = 1;
        stunned = false;
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhatCanBeTaken);
        if (colliders.Length > 0 && canTakeItems)
        {
            canTakeItems = false;
            itemInHands = colliders[0].gameObject.GetComponent<Idol>();

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
            itemInHands.myPlayerIndex = playerIndex;
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
                if (interactable.SetIdol(itemInHands.GetComponent<Idol>(), this))
                {
                    Image.ChangeImageState();
                    canTakeItems = true;
                    itemInHands = null;
                }
            }
        }
    }

    public void GiveIdol(Idol idol)
    {
        itemInHands = idol;
        itemInHands.transform.parent = transform;
        itemInHands.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Hands.position, GrabRange);
    }
}
