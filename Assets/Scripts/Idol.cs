﻿using UnityEngine;
using System.Collections;

public class Idol : MonoBehaviour
{
    [Range(1, 2)]
    public int level;
    public int point;
    public IdolRepairedStatus status;

    public void ResetStats()
    {
        status = IdolRepairedStatus.broken;
    }

    public void StatusUp()
    {
        switch(level)
        {
            case 1:
                status = IdolRepairedStatus.repaired;
                break;
            case 2:
                if(status == IdolRepairedStatus.semiBorken)
                {
                    status = IdolRepairedStatus.repaired;
                }
                else
                {
                    status = IdolRepairedStatus.semiBorken;
                }
                break;
        }
    }

    public void Broke()
    {
        status = IdolRepairedStatus.broken;
    }
}

public enum IdolRepairedStatus
{
    broken,
    semiBorken,
    repaired
}
