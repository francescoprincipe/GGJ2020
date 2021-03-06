﻿using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable {
    public WorkbenchImages WorkbenchImages;
    public WorkbenchInput WorkbenchInput;
    public WorkbenchInput WorkbenchInput2;
    public AudioFx audioFx;
    [Range(1, 3)]
    public int workbenchLevel = 1;

    private int QTECounter;
    private int currentKeyCodeIndex;
    private Player currentPlayer;
    private bool isInQTE = false;
    private int currentQTECounter = 0;
    private BoxCollider selfCollider;
    private AudioSource audioSource;

    public int maxPlayerDist = 80;

    private Idol idol;

    private void Start()
    {
        selfCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isInQTE)
        {
            List<KeyCode> inputs = GetInputs();

            if (Input.GetKeyDown(inputs[currentKeyCodeIndex]))
            {
                Debug.Log("Correct");
                audioSource.clip = audioFx.button;
                audioSource.Play();
                PlayQuickTimeEvent();
                currentQTECounter++;
                if (currentQTECounter >= QTECounter)
                {
                    Debug.Log("Ended quick time event!");
                    EndQuickTimeEvent();
                }
            }
            else if ((currentPlayer.transform.position - transform.position).sqrMagnitude > maxPlayerDist)
            {
                Debug.Log("Moved away from quick time event!");
                EndQuickTimeEvent();
            }
            else if (Input.anyKeyDown)
            {
                if (currentPlayer.playerIndex == idol.myPlayerIndex)
                {
                    Debug.Log("Not correct!");
                    PlayQuickTimeEvent();
                }
            }
        }
    }

    public bool SetIdol(Idol item, Player player)
    {
        currentPlayer = player;
        idol = item;
        idol.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
        idol.transform.parent = transform;
        idol.onPlayer = false;

        switch (workbenchLevel)
        {
            case 1:
                if (idol.status == IdolRepairedStatus.broken)
                {
                    QTECounter = idol.repairButtonNumber;
                    PlayQuickTimeEvent();
                }
                break;
            case 2:
                if (idol.status == IdolRepairedStatus.semiBroken && player.playerIndex == idol.myPlayerIndex)
                {
                    QTECounter = idol.repairButtonNumber;
                    PlayQuickTimeEvent();
                }
                break;
            case 3:
                if (idol.status != IdolRepairedStatus.broken && player.playerIndex != idol.myPlayerIndex)
                {
                    QTECounter = idol.brokeButtonNumber;
                    PlayQuickTimeEvent();
                }
                break;
            default:
                break;
        }

        return true;
    }

    public void PlayQuickTimeEvent()
    {
        if (!isInQTE && currentPlayer.Image.enabled)
        {
            currentPlayer.Image.ChangeImageState();
            isInQTE = true;
        }
        int rngNumber = Random.Range(0, GetInputs().Count);
        currentKeyCodeIndex = rngNumber;
        idol.onPlayer = true;
        currentPlayer.Image.ChangeImageSprite(WorkbenchImages.EventButtonImages[rngNumber]);
    }

    public void EndQuickTimeEvent()
    {
        isInQTE = false;
        currentPlayer.Image.ChangeImageState();
        if (currentQTECounter >= QTECounter)
        {
            if (workbenchLevel == 3)
            {
                idol.Broke();
                audioSource.clip = audioFx.destroyClips[Random.Range(0, audioFx.buildClips.Count)];
            }
            else
            {
                idol.StatusUp(currentPlayer.playerIndex);
                audioSource.clip = audioFx.buildClips[Random.Range(0, audioFx.buildClips.Count)];
            }
            audioSource.Play();          
        }
        currentQTECounter = 0;
        idol.onPlayer = false;
    }

    private List<KeyCode> GetInputs()
    {
        if (currentPlayer.playerIndex == 0)
        {
            return WorkbenchInput.EventButtons;
        }
        else
        {
            return WorkbenchInput2.EventButtons;
        }
    }
}