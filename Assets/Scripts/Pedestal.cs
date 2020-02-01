using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour, IIterable
{
    public void SetItem(Item item, Player player)
    {
        item.transform.parent = null;
        item.transform.position = transform.position;
    }
}
