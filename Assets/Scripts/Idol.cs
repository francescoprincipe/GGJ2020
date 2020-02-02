using UnityEngine;
using System.Collections;

public class Idol : MonoBehaviour
{
    [Range(1, 2)]
    public int level;
    public int point;
    public IdolRepairedStatus status;
    public int repairButtonNumber;
    public int brokeButtonNumber;

    [HideInInspector]
    public int? myPlayerIndex = null;

    public void ResetStats()
    {
        status = IdolRepairedStatus.broken;
        myPlayerIndex = null;
    }

    public void StatusUp(int playerIndex)
    {
        switch(level)
        {
            case 1:
                status = IdolRepairedStatus.repaired;
                break;
            case 2:
                if(status == IdolRepairedStatus.semiBroken)
                {
                    status = IdolRepairedStatus.repaired;
                }
                else
                {
                    status = IdolRepairedStatus.semiBroken;
                }
                break;
        }
        myPlayerIndex = playerIndex;
    }

    public void Broke()
    {
        status = IdolRepairedStatus.broken;
        myPlayerIndex = null;
    }
}

public enum IdolRepairedStatus
{
    broken,
    semiBroken,
    repaired
}
