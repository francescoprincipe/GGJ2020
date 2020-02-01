using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdolsManager : MonoBehaviour
{
    public int lv1idolsNumber;
    public int lv2idolsNumber;

    public Idol lv1Idol;
    public Idol lv2Idol;

    public List<Pedestal> pedestals = new List<Pedestal>();

    private void Start()
    {
        for (int i = 0; i < lv1idolsNumber; i++)
        {
            SpawnIdol(Instantiate(lv1Idol));
        }
        for (int i = 0; i < lv2idolsNumber; i++)
        {
            SpawnIdol(Instantiate(lv2Idol));
        }
    }

    private void OnValidate()
    {
        if(pedestals.Count != lv1idolsNumber + lv2idolsNumber)
        {
            Debug.LogWarning("Piedistalli e idoli non corrispondono");
        }
    }

    public void SpawnIdol(Idol idol)
    {
        idol.ResetStats();
        List<Pedestal> freePedestals = pedestals.FindAll(x => !x.isOccupied);
        freePedestals[Random.Range(0, freePedestals.Count)].SetIdol(idol, null);
    }
}
