using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Events
{
    private static Dictionary<GameEventsEnum, EventControllerBase> actionList = new Dictionary<GameEventsEnum, EventControllerBase>();

    public static void OnChange<T>(T data, GameEventsEnum name) where T : struct
    {
        AddIfNotExists<T>(name);

        actionList[name].Raise(data);
    }

    public static void Register<T>(GameEventsEnum name, Action<T> callback) where T : struct
    {
        AddIfNotExists<T>(name);

        actionList[name].Add(callback);
    }

    private static void AddIfNotExists<T>(GameEventsEnum name)
    {
        if (actionList.ContainsKey(name) == false)
            actionList[name] = new EventController<T>();
    }
}

public abstract class EventControllerBase
{
    public abstract void Raise(object data);
    public abstract void Add(Delegate callback);
}

public class EventController<T> : EventControllerBase
{
    private List<Action<T>> subscribers = new List<Action<T>>();
    private event Action<T> OnRaise;

    public override void Raise(object data)
    {
        OnRaise?.Invoke((T)data);
    }

    public override void Add(Delegate callback)
    {
        if (callback is Action<T>)
        {
            subscribers.Add((Action<T>)callback);
            OnRaise += (Action<T>)callback;
        }
        else
        {
            Debug.LogError("Cannot register with this callback");
        }
    }

    public void RemoveSubscriber(Action<T> d)
    {
        subscribers.Remove(d);
        OnRaise -= d;
    }

    public void UnhookSubscribers()
    {
        foreach (var subscriber in subscribers)
        {
            OnRaise -= subscriber;
        }
    }
}

public enum GameEventsEnum
{
    Gold = 1,
    AnimalWeight,
    GoldCostModifier,
    WeightModifier,
    GoldProduction
}