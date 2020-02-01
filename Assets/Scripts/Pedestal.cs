using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour, IInteractable
{
    public bool isOccupied = false;

    public void SetIdol(Idol item, Player player)
    {
        isOccupied = true;
        item.transform.parent = transform;
        item.transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y, transform.position.z);
    }
}
