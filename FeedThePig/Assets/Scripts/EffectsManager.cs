using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject flyingItemPrefab;
    [SerializeField]
    private RectTransform feedTarget;
    [SerializeField]
    private Canvas effectsCanvas;


    public void LaunchItem(Transform startingLocation, Sprite icon)
    {
        // animate icon going to animal
        var newEffect = Instantiate(flyingItemPrefab, startingLocation.transform.position, Quaternion.identity, effectsCanvas.transform);
        var move = newEffect.GetComponent<MoveToTargetAndFade>();
        newEffect.GetComponentInChildren<Image>().sprite = icon;
        move.target = feedTarget;
        move.StartMoving();
    }
}
