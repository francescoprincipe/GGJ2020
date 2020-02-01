using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    public WorkbenchImages WorkbenchImages;
    public WorkbenchInput WorkbenchInput;
    [Range(1,2)]
    public int workbenchLevel = 1;

    [DraggablePoint] public Vector3 itemPosition;
    private Item item;

    public void SetItem(Item item, Player player)
    {
        this.item = item;
        this.item.transform.position = itemPosition;
        PlayQuickTimeEvent(player);
    }

    public void PlayQuickTimeEvent(Player player)
    {
        int rngNumber = Random.Range(0, WorkbenchInput.EventButtons.Count);
        //player.Image.ChangeImageSprite(WorkbenchImages.EventButtonImages[rngNumber]);
    }
}
