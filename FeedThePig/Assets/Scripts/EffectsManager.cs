using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject flyingItemPrefab;
    [SerializeField]
    private RectTransform pickUpItemFrom;
    [SerializeField]
    private RectTransform feedTarget;
    [SerializeField]
    private RectTransform goldTarget;
    [SerializeField]
    private Canvas effectsCanvas;
    [SerializeField]
    private TextMeshProUGUI overlayText;

    private void Start()
    {
        Events.Register<LootItem>(GameEventsEnum.EventLootDropped, (item) => { SetOverlayText(string.Format("Found item: {0}", item.Description), 2f); });
        Events.Register<int>(GameEventsEnum.EventStartLevel, (number)     => { SetOverlayText("Level " + number); });
        Events.Register<ShopItem, Transform>(GameEventsEnum.EventShopItemPurchased, (item, location) => { LaunchItem(location, item.Icon); });
    }

    public void LaunchItem(Transform startingLocation, Sprite icon, LaunchTargetLocationEnum target = LaunchTargetLocationEnum.Default)
    {
        var launchTarget = GetLaunchTarget(target);
        // animate icon going to animal
        var newEffect = Instantiate(flyingItemPrefab, startingLocation.transform.position, Quaternion.identity, effectsCanvas.transform);
        var move = newEffect.GetComponent<MoveToTargetAndFade>();
        newEffect.GetComponentInChildren<Image>().sprite = icon;
        move.target = launchTarget;
        move.StartMoving();
    }

    public void SetOverlayText(string text, float fadeDuration = 3f)
    {
        overlayText.text = text;
        overlayText.gameObject.SetActive(true);

        StartCoroutine(FadeObjectAfter(overlayText.gameObject, fadeDuration));
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
            case LaunchTargetLocationEnum.Weight:
                return goldTarget;
            case LaunchTargetLocationEnum.Default:
            case LaunchTargetLocationEnum.Animal:
            default:
                return feedTarget;
        }
    }
}
