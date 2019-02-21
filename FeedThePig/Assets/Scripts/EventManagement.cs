using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Events
{
    private static Dictionary<GameEventsEnum, EventControllerBase> actionList = new Dictionary<GameEventsEnum, EventControllerBase>();

    public static void Raise(GameEventsEnum name)// where T : struct
    {
        AddIfNotExists(name);

        actionList[name].Raise(null);
    }

    public static void Raise<T>(T data, GameEventsEnum name)// where T : struct
    {
        AddIfNotExists<T>(name);

        actionList[name].Raise(data);
    }

    public static void Raise<T, V>(T data, V data2, GameEventsEnum name)// where T : struct
    {
        AddIfNotExists<T, V>(name);

        actionList[name].Raise(data);
    }

    public static void Register(GameEventsEnum name, Action callback)// where T : struct
    {
        AddIfNotExists(name);
        actionList[name].Add(callback);
    }

    public static void Register<T>(GameEventsEnum name, Action<T> callback)// where T : struct
    {
        AddIfNotExists<T>(name);
        actionList[name].Add(callback);
    }

    public static void Register<T, V>(GameEventsEnum name, Action<T, V> callback)// where T : struct
    {
        AddIfNotExists<T, V>(name);
        actionList[name].Add(callback);
    }

    private static void AddIfNotExists(GameEventsEnum name)
    {
        if (actionList.ContainsKey(name) == false)
            actionList[name] = new EventController();
    }

    private static void AddIfNotExists<T>(GameEventsEnum name)
    {
        if (actionList.ContainsKey(name) == false)
            actionList[name] = new EventController<T>();
    }

    private static void AddIfNotExists<T, V>(GameEventsEnum name)
    {
        if (actionList.ContainsKey(name) == false)
            actionList[name] = new EventController<T, V>();
    }
}

public abstract class EventControllerBase
{
    public abstract void Raise(object data, object data2 = null);
    public abstract void Add(Delegate callback);
}

public class EventController : EventControllerBase
{
    protected List<Delegate> subscribers = new List<Delegate>();
    protected event Action OnRaise;

    public override void Raise(object data, object data2 = null)
    {
        OnRaise?.Invoke();
    }

    public override void Add(Delegate callback)
    {
            subscribers.Add((Action)callback);
            OnRaise += (Action)callback;
    }

    public virtual void RemoveSubscriber(Action d)
    {
        subscribers.Remove(d);
        OnRaise -= d;
    }

    public void UnhookSubscribers()
    {
        foreach (var subscriber in subscribers)
        {
            OnRaise -= (Action)subscriber;
        }
    }
}

public class EventController<T> : EventControllerBase
{
    private List<Action<T>> subscribers = new List<Action<T>>();
    private event Action<T> OnRaise;

    public override void Raise(object data, object data2 = null)
    {
        if (data is T == false)
            Debug.LogError(string.Format("Invalid cast in events. Expected type: {0}. Received Type: {1}", typeof(T).Name, data.GetType().Name));
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

    public virtual void RemoveSubscriber(Action<T> d)
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

public class EventController<T, V> : EventControllerBase
{
    private List<Action<T, V>> subscribers = new List<Action<T, V>>();
    private event Action<T, V> OnRaise;

    public override void Raise(object data1, object data2)
    {
        if (data1 is T == false || data2 is V == false)
            Debug.LogError(string.Format("Invalid cast in events. Expected typse: {0} and {1}. Received Types: {2} and {3}", typeof(T).Name, typeof(V).Name, data1.GetType().Name, data2.GetType().Name));
        OnRaise?.Invoke((T)data1, (V)data2);
    }

    public override void Add(Delegate callback)
    {
        if (callback is Action<T, V>)
        {
            subscribers.Add((Action<T, V>)callback);
            OnRaise += (Action<T, V>)callback;
        }
        else
        {
            Debug.LogError("Cannot register with this callback");
        }
    }

    public virtual void RemoveSubscriber(Action<T, V> d)
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
    // game stats
    DataGoldChanged = 1,
    DataAnimalWeightChanged,
    DataGoldCostModifier,
    DataWeightModifier,
    DataGoldProduction,
    DataAnimalSoldChanged,
    DataTotalFoodBought,
    DataTotalWeightAcquired,
    DataWeightProduction,

    // animal stats
    DataAnimalDamageChanged,
    DataAnimalArmorChanged,
    DataAnimalSpeedChanged,
    DataAnimalCritChanceChanged,
    DataAnimalCritDamageChanged,
    
    // game events
    EventUpgrade,
    EventAnimalDeath,    
    EventLootDropped,
    EventGoldDropped,
    EventShopItemPurchased,
    EventAnimalSold,

    // level management
    EventGamePauseToggle,
    EventStartLevel,
    EventAdvanceLevel
}