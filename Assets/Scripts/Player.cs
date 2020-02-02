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
    private Animator animator;

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
    private float dashMultiplier = 1;
    public float stunTime = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        selfCollider = GetComponent<BoxCollider>();
        Image = GetComponentInChildren<PlayerImage>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        particleSystem.enableEmission = false;
        playerInfo.points = 0f;
    }

    void Update()
    {
        currentDirection = new Vector3(Input.GetAxisRaw(playerInput.HorizontalInputName), 0f, Input.GetAxisRaw(playerInput.VerticalInputName)).normalized;
        if (currentDirection != Vector3.zero)
        {
            //animator.Play("Move");
            evaluatingTime += Time.deltaTime;
            particleSystem.enableEmission = true;
        }
        else
        {
            particleSystem.enableEmission = false;
            //animator.Play("Idle");
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
        dashMultiplier = 3;
        //animator.speed = 1f * dashMultiplier;
        yield return new WaitForSeconds(sprintLenght);
        dashMultiplier = 1;
        //animator.speed = 1f;
    }

    private IEnumerator SprintCooldown()
    {
        canSprint = false;
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = true;
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(Hands.position, GrabRange, WhatCanBeTaken);
        if (colliders.Length > 0 && canTakeItems && !colliders[0].gameObject.GetComponent<Idol>().onPlayer)
        {
            itemInHands = colliders[0].gameObject.GetComponent<Idol>();
            canTakeItems = false;

            if (itemInHands.status == IdolRepairedStatus.broken)
            {
                Image.ChangeImageSprite(playerImages.Workbench_1);
            }

            if (itemInHands.myPlayerIndex == playerIndex)
            {
                if (itemInHands.status == IdolRepairedStatus.repaired)
                {
                    Image.ChangeImageSprite(playerImages.Altar);
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
            itemInHands.onPlayer = true;
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
