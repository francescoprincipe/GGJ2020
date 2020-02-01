using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    public WorkbenchImages WorkbenchImages;
    public WorkbenchInput WorkbenchInput;
    public WorkbenchInput WorkbenchInput2;
    [Range(1,3)]
    public int workbenchLevel = 1;

    private int QTECounter;
    private int currentKeyCodeIndex;
    private Player currentPlayer;
    private bool isInQTE = false;
    private int currentQTECounter = 0;
    private BoxCollider selfCollider;

    private Idol idol;

    private void Start()
    {
        selfCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isInQTE)
        {
            List<KeyCode> inputs;

            if(idol.myPlayerIndex == 0)
            {
                inputs = WorkbenchInput.EventButtons;
            }
            else
            {
                inputs = WorkbenchInput2.EventButtons;
            }

            if (Input.GetKeyDown(inputs[currentKeyCodeIndex]))
            {
                Debug.Log("Correct");
                PlayQuickTimeEvent();
                currentQTECounter++;
                if (currentQTECounter >= QTECounter)
                {
                    Debug.Log("Ended quick time event!");
                    EndQuickTimeEvent();
                }
            }
            else if (Input.GetAxisRaw(currentPlayer.playerInput.HorizontalInputName) != 0 || Input.GetAxisRaw(currentPlayer.playerInput.VerticalInputName) != 0)
            {
                Debug.Log("Ended quick time event!");
                EndQuickTimeEvent();
            }
            else if (Input.anyKeyDown)
            {
                Debug.Log("Not correct!");
                PlayQuickTimeEvent();
            }
        }
    }

    public bool SetIdol(Idol item, Player player)
    {
        currentPlayer = player;
        idol = item;
        idol.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
        idol.transform.parent = transform;

        switch (workbenchLevel)
        {
            case 1:
                if (idol.status == IdolRepairedStatus.broken && player.playerIndex == idol.myPlayerIndex)
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
        if (!isInQTE)
        {
            currentPlayer.Image.ChangeImageState();
            isInQTE = true;
        }
        int rngNumber = Random.Range(0, WorkbenchInput.EventButtons.Count);
        currentKeyCodeIndex = rngNumber;
        currentPlayer.Image.ChangeImageSprite(WorkbenchImages.EventButtonImages[rngNumber]);
        Debug.Log(currentKeyCodeIndex);
    }

    public void EndQuickTimeEvent()
    {
        isInQTE = false;
        currentPlayer.Image.ChangeImageState();
        currentQTECounter = 0;

        if(workbenchLevel == 3)
        {
            idol.Broke();
        }
        else
        {
            idol.StatusUp();
        }
    }
}