using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Guirao.UltimateTextDamage;

public class EffectsManager : MonoBehaviour
{

    [SerializeField]
    private Canvas effectsCanvas;
    [SerializeField]
    private TextMeshProUGUI overlayText;
    [SerializeField]
    private UltimateTextDamageManager textDamageManager;

    [Header("Prefabs")]
    [SerializeField]
    private MoveToTargetAndFade flyingItemPrefab;
    

    [Header("Targets")]
    [SerializeField]
    private RectTransform centerScreenTarget;
    [SerializeField]
    private RectTransform goldTarget;
    [SerializeField]
    private RectTransform weightTarget;

    private static List<EffectBase> activeEffects = new List<EffectBase>();

    private void Start()
    {
        Events.Register<LootItem>(GameEventsEnum.EventLootGained, (item) => { SetOverlayText(string.Format("Found item: {0}", item.Description), 2f); });
        Events.Register<int>(GameEventsEnum.EventStartLevel, (number) => { SetOverlayText("Level " + number); });
        Events.Register<ShopItem, Transform>(GameEventsEnum.EventShopItemPurchased, (item, location) => { LaunchItem(location.position, item.Icon); });


        Events.Register<Vector3, Sprite, LaunchTargetLocationEnum, Action>(GameEventsEnum.EventLaunchItem, LaunchItem);        
        
        Events.Register(GameEventsEnum.EventGameOver, () => { RemoveAllActiveEffects(); });
        Events.Register<string, Transform, string>(GameEventsEnum.EventCreateDamageText, AddDamage);
    }

    public void LaunchItem(Vector3 startingLocation, Sprite icon, LaunchTargetLocationEnum target = LaunchTargetLocationEnum.Default, Action onFinishCallback = null)
    {
        var launchTarget = GetLaunchTarget(target);
        // animate icon going to animal
        var newEffect = flyingItemPrefab.Get<MoveToTargetAndFade>(startingLocation, Quaternion.identity, effectsCanvas.gameObject);
        newEffect.GetComponentInChildren<Image>().sprite = icon;
        newEffect.SetTarget(launchTarget.transform.position);
        newEffect.StartMoving(onFinishCallback);

        activeEffects.Add(newEffect);
    }
    
    public void SetOverlayText(string text, float fadeDuration = 3f)
    {
        overlayText.text = text;
        overlayText.gameObject.SetActive(true);

        Events.StartCoroutine(Utils.FadeObjectAfter(overlayText.gameObject, fadeDuration));
    }

    public void AddDamage(string text, Transform position, string key)
    {
        if (string.IsNullOrEmpty(key))
            key = "default";

        textDamageManager.Add(text, position, key);
    }

    public static void AddEffect(EffectBase effect)
    {
        if (activeEffects.Contains(effect) == false)
            activeEffects.Add(effect);
    }

    public static void RemoveEffect(EffectBase effect)
    {
        if (activeEffects.Contains(effect))
            activeEffects.Remove(effect);
    }

    private void RemoveAllActiveEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            if (activeEffects[i] != null)
                activeEffects[i].KillEffect();
        }

        activeEffects.Clear();
    }


    

    private RectTransform GetLaunchTarget(LaunchTargetLocationEnum target)
    {
        switch (target)
        {
            case LaunchTargetLocationEnum.Gold:
                return goldTarget;
            case LaunchTargetLocationEnum.Weight:
                return weightTarget;
            case LaunchTargetLocationEnum.Default:
            case LaunchTargetLocationEnum.Animal:
            default:
                return centerScreenTarget;
        }
    }
}
