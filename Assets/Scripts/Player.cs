using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerInput playerInput;
    public PlayerInfo playerInfo;
    public PlayerImages playerImages;
    public int playerIndex;

    private Rigidbody rb;
    private Idol itemInHands;
    private BoxCollider selfCollider;
    private float evaluatingTime;
    public bool canTakeItems = true;
    private ParticleSystem particleSystem;

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

    private bool touchingOtherPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        selfCollider = GetComponent<BoxCollider>();
        Image = GetComponentInChildren<PlayerImage>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.enableEmission = false;
        playerInfo.points = 0f;
    }

    void Update()
    {
        currentDirection = new Vector3(Input.GetAxisRaw(playerInput.HorizontalInputName), 0f, Input.GetAxisRaw(playerInput.VerticalInputName)).normalized;
        if (currentDirection != Vector3.zero)
        {
            evaluatingTime += Time.deltaTime;
            particleSystem.enableEmission = true;
        }
        else
        {
            particleSystem.enableEmission = false;
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
        if (other.gameObject.tag == "Player")
            touchingOtherPlayer = true;
        if (isSprinting && (other.gameObject.tag == "Player"))
        {
            other.gameObject.GetComponent<Player>().Stun();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
            touchingOtherPlayer = false;
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
            itemInHands = colliders[0].gameObject.GetComponent<Idol>();
            canTakeItems = false;

            if (itemInHands.myPlayerIndex == playerIndex)
            {
                if (itemInHands.level == 1)
                {
                    if (itemInHands.status == IdolRepairedStatus.broken)
                    {
                        Image.ChangeImageSprite(playerImages.Workbench_1);
                    }
                    else if (itemInHands.status == IdolRepairedStatus.repaired)
                    {
                        Image.ChangeImageSprite(playerImages.Altar);
                    }
                }
                else if (itemInHands.level == 2)
                {
                    if (itemInHands.status == IdolRepairedStatus.broken)
                    {
                        Image.ChangeImageSprite(playerImages.Workbench_1);
                    }
                    else if (itemInHands.status == IdolRepairedStatus.semiBroken)
                    {
                        Image.ChangeImageSprite(playerImages.Workbench_2);
                    }
                    else if (itemInHands.status == IdolRepairedStatus.repaired)
                    {
                        Image.ChangeImageSprite(playerImages.Altar);
                    }
                }
            }
            else
            {
                if (itemInHands.status != IdolRepairedStatus.broken)
                {
                    Image.ChangeImageSprite(playerImages.Workbench_3);
                }
            }

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
