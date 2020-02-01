using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {

    public TextMeshProUGUI timerText;
    private float time = 181;

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
        if (timerText != null)
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
            timerText.text = "0:00";
        }
    }
}
