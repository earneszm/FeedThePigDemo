using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICustomButton : MonoBehaviour, IIntializeInactive
{
    [HideInInspector]
    public Button Button;

    [SerializeField]
    private CustomButtonTypeEnum buttonType;
    public CustomButtonTypeEnum ButtonType { get { return buttonType; } }

    [SerializeField]
    private bool inferTextFromName = true;

    private void OnValidate()
    {
        if (inferTextFromName == false)
            return;

        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = gameObject.name.Replace("Button", "");
    }

    public void ForceAwake()
    {
        Button = GetComponent<Button>();

        if (Button == null)
            Debug.LogError("Could not find Button on object: " + gameObject.name);
    }

    public void ForceStart()
    {
        Button.onClick.AddListener(OnClick);
    }

   // private void Start()
   // {
   //     
   // }

    public virtual void OnClick()
    {
    }

    
}
