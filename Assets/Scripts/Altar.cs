﻿using System.Collections;
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
        if(timerEnable)
        {
            time += Time.deltaTime;

            if(time >= timerTick)
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

    public void SetIdol(Idol idol, Player player)
    {
        if(idolOnAltar == null)
        {
            idol.transform.parent = transform;
            idol.transform.position = transform.position;
            idolOnAltar = new IdolOnAltar(idol, player.playerInfo);
            StartTimer();
        }
    }

    public void RespawnIdol()
    {
        StopTimer();
        var idol = idolOnAltar.item;
        idolOnAltar = null;
        idolManager.SpawnIdol(idol);
    }
}
