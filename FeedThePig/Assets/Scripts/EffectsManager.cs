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
    [SerializeField]
    private Gold goldDroppedPrefab;

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
        Events.Register<LootItem>(GameEventsEnum.EventLootDropped, (item) => { SetOverlayText(string.Format("Found item: {0}", item.Description), 2f); });
        Events.Register<int>(GameEventsEnum.EventStartLevel, (number) => { SetOverlayText("Level " + number); });
        Events.Register<ShopItem, Transform>(GameEventsEnum.EventShopItemPurchased, (item, location) => { LaunchItem(location.position, item.Icon); });

        Events.Register<int, Transform>(GameEventsEnum.EventGoldSpawned, (amount, location) => { CreateGoldAsLoot(amount, location); });
        Events.Register<int, Vector3, Sprite>(GameEventsEnum.EventGoldPickedUp, OnGoldPickedUp);
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

    public void CreateGoldAsLoot(int amount, Transform spawnLocation)
    {
        var gold = goldDroppedPrefab.Get<Gold>(spawnLocation.position, Quaternion.identity);
        gold.SetGold(amount);

        activeEffects.Add(gold);
    }

    private void OnGoldPickedUp(int amount, Vector3 position, Sprite icon)
    {
        Debug.Log("GainGold (OnGoldPickedUp): " + amount);
        LaunchItem(position, icon, LaunchTargetLocationEnum.Gold, 
            () => {
                Debug.Log("GainGold (OnGoldPickedUp: callback): " + amount);
                Events.Raise(amount, GameEventsEnum.EventGoldGained); });
    }

    public void SetOverlayText(string text, float fadeDuration = 3f)
    {
        overlayText.text = text;
        overlayText.gameObject.SetActive(true);

        StartCoroutine(FadeObjectAfter(overlayText.gameObject, fadeDuration));
    }

    public void AddDamage(string text, Transform position, string key)
    {
        if (string.IsNullOrEmpty(key))
            key = "default";

        textDamageManager.Add(text, position, key);
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


    private IEnumerator FadeObjectAfter(GameObject go, float fadeDuration)
    {
        yield return new WaitForSeconds(fadeDuration);

        go.SetActive(false);
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
