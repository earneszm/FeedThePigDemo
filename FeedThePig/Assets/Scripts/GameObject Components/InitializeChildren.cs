using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeChildren : MonoBehaviour
{
    void Awake()
    {
        var objectsToIniatlize = GetComponentsInChildren<IIntializeInactive>(true);

        foreach (var item in objectsToIniatlize)
        {
            item.ForceAwake();
        }
    }

    void Start()
    {
        var objectsToIniatlize = GetComponentsInChildren<IIntializeInactive>(true);

        foreach (var item in objectsToIniatlize)
        {
            item.ForceStart();
        }
    }
}
