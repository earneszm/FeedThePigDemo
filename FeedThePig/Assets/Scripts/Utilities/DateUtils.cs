using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DateUtils
{
    public static string GetFriendlyDateDifference(DateTime start, DateTime end)
    {
        return GetFriendlyTimeSpan(start - end);
    }

    public static string GetFriendlyTimeSpan(TimeSpan value)
    {
        string returnString = "";

        if (value.Days > 0)
            returnString += value.Days + " days, ";

        if (value.Minutes > 0)
            returnString += value.Minutes + " minutes, ";

        if (value.Seconds > 0)
            returnString += value.Seconds + " seconds, ";

        return returnString.Substring(0, returnString.Length - 2);
    }
}
