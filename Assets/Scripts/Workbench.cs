using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    public int workbanchLevel = 0;

    [DraggablePoint] public Vector3 itemPosition;

    public Canvas canvas;

    private GameObject item;

    public void SetItem(GameObject item, Player player)
    {
        this.item = item;
        this.item.transform.position = itemPosition;
        player.canMove = false;
    }
}
