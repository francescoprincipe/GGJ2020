using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Idol : MonoBehaviour
{
    [Range(1, 2)]
    public int level;
    public int point;
    public IdolRepairedStatus status;
    public int repairButtonNumber;
    public int brokeButtonNumber;
    public bool onPlayer = false;

    [HideInInspector]
    public int? myPlayerIndex = null;

    private Dictionary<int, GameObject> statusObjects = new Dictionary<int, GameObject>();

    private void Awake()
    {
        var objects = GetComponentsInChildren<Transform>();

        for(int i = 0; i < objects.Length; i++)
        {
            switch(objects[i].tag)
            {
                case "Reparied":
                    statusObjects.Add((int)IdolRepairedStatus.repaired, objects[i].gameObject);
                    break;
                case "Broken":
                    statusObjects.Add((int)IdolRepairedStatus.broken, objects[i].gameObject);
                    break;
                case "SemiBroken":
                    statusObjects.Add((int)IdolRepairedStatus.semiBroken, objects[i].gameObject);
                    break;
            }
        }

        SetView();
    }

    public void ResetStats()
    {
        status = IdolRepairedStatus.broken;
        myPlayerIndex = null;
        SetView();
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
        SetView();
    }

    public void Broke()
    {
        status = IdolRepairedStatus.broken;
        myPlayerIndex = null;
        SetView();
    }

    private void SetView()
    {
        statusObjects[(int)status].GetComponent<MeshRenderer>().enabled = true;
        foreach (var i in statusObjects)
        {
            if (i.Key != (int)status)
            {
                i.Value.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}

public enum IdolRepairedStatus
{
    broken,
    semiBroken,
    repaired
}
