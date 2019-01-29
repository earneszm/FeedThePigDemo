using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(UICustomButton))]
public class UIButton_MenuNav : MonoBehaviour
{
    public RectTransform menuContent;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        UIManager.Instance.OnNavButtonClicked(this);
    }
}
