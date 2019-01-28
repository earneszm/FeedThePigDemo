using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIDialog : MonoBehaviour, IIntializeInactive
{
    protected UICustomButton confirmButton;

    [SerializeField]
    private DialogTypeEnum dialogType;
    public DialogTypeEnum DialogType { get { return dialogType; } }

    public virtual void ForceAwake()
    {        
    }

    public virtual void ForceStart()
    {
        var allCustomButtons = GetComponentsInChildren<UICustomButton>(true);
        confirmButton = allCustomButtons.FirstOrDefault(x => x.ButtonType == CustomButtonTypeEnum.Confirm);

        if (confirmButton == null)
            Debug.LogError("Could not find Confirm Button on Dialog: " + gameObject.name);
        else
            confirmButton.Button.onClick.AddListener(OnConfirmClick);
    }

    public virtual void OnDialogOpen(object data)
    {
        ToggleActive(true);
    }

    public virtual void OnDialogOpen(params string[] values)
    {
        ToggleActive(true);
    }

    private void ToggleActive(bool value)
    {
        gameObject.SetActive(value);
        
    }

    protected virtual void OnConfirmClick()
    {
        UIManager.Instance.CloseDialog();
    }
}
