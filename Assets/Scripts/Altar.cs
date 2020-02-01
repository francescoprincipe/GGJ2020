using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
    private IdolOnAltar idolOnAltar;

    [SerializeField] private IdolsManager idolManager = null;

    [Tooltip("In seconds")]
    public int timerTick;
    [Tooltip("When number is reached, the item will disappear")]
    public float itemIterations;

    private float time;
    private bool timerEnable = false;

    private void StartTimer()
    {
        timerEnable = true;
        time = 0;
    }

    private void Update()
    {
        if (timerEnable)
        {
            time += Time.deltaTime;

            if (time >= timerTick)
            {
                OnTimerTick();
                time = 0;
            }
        }
    }

    private void StopTimer()
    {
        timerEnable = false;
    }

    private void OnTimerTick()
    {
        print("Tick");
        if (idolOnAltar.steps < itemIterations)
        {
            idolOnAltar.steps++;
            idolOnAltar.playerInfo.points += idolOnAltar.item.point;
        }
        else
            RespawnIdol();
    }

    public bool SetIdol(Idol idol, Player player)
    {
        if (idol.myPlayerIndex == player.playerIndex && idol.status == IdolRepairedStatus.repaired && idolOnAltar?.item.myPlayerIndex != player.playerIndex)
        {
            if(idolOnAltar != null)
            {
                player.GiveIdol(idolOnAltar.item);
            }
            idol.transform.parent = transform;
            idol.transform.position = transform.position;
            idolOnAltar = new IdolOnAltar(idol, player.playerInfo);
            StartTimer();
            return true;
        }
        return false;
    }

    public void RespawnIdol()
    {
        StopTimer();
        var idol = idolOnAltar.item;
        idolOnAltar = null;
        idolManager.SpawnIdol(idol);
    }

    public void ResetAltar()
    {
        StopTimer();
        idolOnAltar = null;
    }
}
