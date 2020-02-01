using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public TextMeshProUGUI timerText;
    private float time = 181;
    bool stopCoroutine = false;

    void Start()
    {
        StartCoundownTimer();
    }

    void StartCoundownTimer()
    {
        if (timerText != null)
        {
            time = 181;
            timerText.text = "3:00";
            InvokeRepeating("UpdateTimer", 1f, 1f);
        }
    }

    void UpdateTimer()
    {
        if (timerText != null && !stopCoroutine)
        {
            time -= Time.deltaTime;
            string minutes = Mathf.Floor(time / 60).ToString("0");
            string seconds = ((time % 60) - 1).ToString("00");
            timerText.text = minutes + ":" + seconds;
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            //EndGame();
            stopCoroutine = true;
            timerText.text = "0:00";
        }
    }
}
