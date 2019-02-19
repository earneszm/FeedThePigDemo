using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToWeightChanged : MonoBehaviour
{
    [SerializeField]
    private float minScaleAmount = .25f;

    [SerializeField]
    private float maxScaleAmount = 2f;

    void Start()
    {
        Events.Register<float>(GameEventsEnum.AnimalWeight, OnWeightChanged);
    }

    private void OnWeightChanged(float weight)
    {
        var newScale = Mathf.Clamp(weight / GameConstants.StartingAnimalWeight, minScaleAmount, maxScaleAmount);
        gameObject.transform.localScale = new Vector3(newScale, newScale, 1);
    }
}
