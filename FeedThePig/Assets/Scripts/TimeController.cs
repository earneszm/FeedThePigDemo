using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController
{
    private IGoldRate rateObject;
    private DateTime startTime;

    private float runningTime;
    private float fractionalGold;

    public TimeController(IGoldRate rateObject, DateTime startTime)
    {
        this.rateObject = rateObject;
        this.startTime = startTime;

        Events.Register<bool>(GameEventsEnum.EventGamePauseToggle, TogglePause);
    }

    public void DoUpdate(float deltaTime)
    {
        runningTime += deltaTime;

        // reset every second
        if(runningTime >= 10)
        {
            runningTime = 0;
            fractionalGold += rateObject.GoldRatePerMinute / 6;

            if (fractionalGold > 1)
            {
                rateObject.AddGold(Mathf.FloorToInt(fractionalGold));
                fractionalGold -= Mathf.FloorToInt(fractionalGold);
            }
        }
    }

    public void TogglePause(bool shouldPause = true)
    {
        Time.timeScale = shouldPause ? 0f : 1f;
    }
}
