using UnityEngine;
using UnityEditor;

public class IdolOnAltar 
{
    public Idol item;
    public PlayerInfo playerInfo;
    public int steps;

    public IdolOnAltar(Idol item, PlayerInfo playerInfo)
    {
        this.item = item;
        this.playerInfo = playerInfo;
        steps = 0;
    }
}