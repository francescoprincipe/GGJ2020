using UnityEngine;
using UnityEditor;

public class ItemOnAltar 
{
    public Item item;
    public PlayerInfo playerInfo;
    public int steps;

    public ItemOnAltar(Item item, PlayerInfo playerInfo)
    {
        this.item = item;
        this.playerInfo = playerInfo;
        steps = 0;
    }
}