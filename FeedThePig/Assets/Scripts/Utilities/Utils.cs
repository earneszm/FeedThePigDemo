using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static IEnumerator IncrementOverTime(int amountToAdd, Action<int> callback, float duration = .4f, int totalSteps = 5)
    {
        if (duration == 0f)
            callback(amountToAdd);

        var stepsToGo = totalSteps;
        var stepDistance = duration / totalSteps;

        var amountStep = amountToAdd / totalSteps;
        var remainder = amountToAdd % totalSteps;

        var total = remainder + amountStep;
        callback(remainder + amountStep);
        stepsToGo--;

        var timePassed = 0f;
        var timeStep = 0f;
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            timeStep += Time.deltaTime;

            if (timeStep >= stepDistance && stepsToGo > 0)
            {
                stepsToGo--;
                timeStep = 0f;
                callback(amountStep);
                total += amountStep;
            }
            yield return null;
        }


        // ensure the amount is correct
        if (total < amountToAdd)
        {
            callback(amountToAdd - total);
            total += amountToAdd - total;
        }

        //    Debug.Log(string.Format("AddGoldOverTime: Try to add: {0}. Total Added: {1}. Amount Per Step: {2}. Remainder: {3}. StartingGold: {4}. EndingGold: {5}", amountToAdd, total, amountStep, remainder, startingGold, Gold));
    }

    public static IEnumerator FadeObjectAfter(GameObject go, float fadeDuration)
    {
        yield return new WaitForSeconds(fadeDuration);

        go.SetActive(false);
    }
}
