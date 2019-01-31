using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarketContent : MonoBehaviour
{
    [SerializeField]
    private Button sellAnimalButton;

    private void Start()
    {
        sellAnimalButton.onClick.AddListener(OnSellAnimalClick);
    }

    private void OnSellAnimalClick()
    {
        GameManager.Instance.OnSellAnimal();
    }
}
