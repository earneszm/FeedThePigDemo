using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptableObjectUtils
{
    public static T Copy<T>(T source) where T : ScriptableObject
    {
        return (T)ScriptableObject.CreateInstance(typeof(T));
        //if (resetSource != null)
        //{
        //    var output = JsonUtility.ToJson(resetSource);
        //    JsonUtility.FromJsonOverwrite(output, this);
        //}
    }

    public static void Reset<T>(T objectToReset) where T : ScriptableObject
    {
        var resetSource = Copy<T>(objectToReset);
        var output = JsonUtility.ToJson(resetSource);
        JsonUtility.FromJsonOverwrite(output, objectToReset);
    }
}
