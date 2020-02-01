using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    public WorkbenchImages WorkbenchImages;
    public WorkbenchInput WorkbenchInput;
    [Range(1,2)]
    public int workbenchLevel = 1;
    public int QTECounter;

    private KeyCode currentKeyCode;
    private Player currentPlayer;
    private bool isInQTE;
    private int currentQTECounter;
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
            for (int i = 0; i < WorkbenchInput.EventButtons.Count; i++)
            {
                if (Input.GetKeyDown(WorkbenchInput.EventButtons[i]) && WorkbenchInput.EventButtons[i] == currentKeyCode)
                {
                    Debug.Log("Correct!");
                    PlayQuickTimeEvent();
                    currentQTECounter++;
                    if (currentQTECounter >= QTECounter)
                    {
                        isInQTE = false;
                        currentPlayer.Image.ChangeImageState();
                        currentPlayer = null;
                    }
                }
                else if (Input.anyKeyDown)
                {
                    Debug.Log("Not correct!");
                    PlayQuickTimeEvent();
                }
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
        isInQTE = true;
        int rngNumber = Random.Range(0, WorkbenchInput.EventButtons.Count);
        currentKeyCode = WorkbenchInput.EventButtons[rngNumber];
        currentPlayer.Image.ChangeImageSprite(WorkbenchImages.EventButtonImages[rngNumber]);
    }
}