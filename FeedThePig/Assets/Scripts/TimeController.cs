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
}
