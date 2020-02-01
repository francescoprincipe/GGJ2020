using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour, IInteractable
{
    public bool isOccupied = false;

    public bool SetIdol(Idol item, Player player)
    {
        if(!isOccupied)
        {
            item.transform.parent = transform;
            item.transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y, transform.position.z);
            isOccupied = true;
            return true;
        }
        return false;
    }
}
