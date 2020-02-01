using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Altar : MonoBehaviour, IIterable
{
    private List<ItemOnAltar> items = new List<ItemOnAltar>();
    private Timer pointsStepTimer;

    [Tooltip("In seconds")]
    public int timerTick;

    private void Start()
    {
        SetUpTimer();
    }

    private void SetUpTimer()
    {
        pointsStepTimer = new Timer(timerTick * 1000);
        pointsStepTimer.Elapsed += OnTimerTick;
        pointsStepTimer.AutoReset = true;
        pointsStepTimer.Enabled = true;
    }

    private void OnTimerTick(object sender, ElapsedEventArgs e)
    {
        print("Tick");
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].steps < 5)
            {
                items[i].steps++;
                items[i].playerInfo.points += items[i].item.point;
            }
        }
    }

    public void SetItem(Item item, Player player)
    {
        item.transform.parent = null;
        item.transform.position = transform.position;
        items.Add(new ItemOnAltar(item, player.playerInfo));
    }
}
