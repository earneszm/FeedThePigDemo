using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToWeightChanged : MonoBehaviour
{

    void Start()
    {
        Events.Register<float>(GameEventsEnum.AnimalWeight, OnWeightChanged);
    }

    private void OnWeightChanged(float weight)
    {
        var newScale = weight / GameConstants.StartingAnimalWeight;
        gameObject.transform.localScale = new Vector3(newScale, newScale, 1);
    }
}
