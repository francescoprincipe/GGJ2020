using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    public WorkbenchImages WorkbenchImages;
    public WorkbenchInput WorkbenchInput;
    [Range(1,2)]
    public int workbenchLevel = 1;
    public int QTECounter;

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
            if (Input.GetKeyDown(WorkbenchInput.EventButtons[currentKeyCodeIndex]))
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

    public void SetIdol(Idol item, Player player)
    {
        currentPlayer = player;
        idol = item;
        idol.transform.position = new Vector3(transform.position.x, transform.position.y + selfCollider.size.y / 2, transform.position.z);
        idol.transform.parent = transform;
        PlayQuickTimeEvent();
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
        currentPlayer = null;
        currentQTECounter = 0;
    }
}