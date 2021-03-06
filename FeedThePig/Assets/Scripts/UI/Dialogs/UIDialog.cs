﻿using System.Collections;
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

    [SerializeField]
    private bool pauseGameOnOpen = true;
    public bool PauseGameOnOpen { get { return pauseGameOnOpen; } }

    [SerializeField]
    private GameEventsEnum onConfirmEvent;

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
        OnDialogOpenBase();
    }

    public virtual void OnDialogOpen(params string[] values)
    {
        ToggleActive(true);
        OnDialogOpenBase();
    }

    private void OnDialogOpenBase()
    {
        if (pauseGameOnOpen)
            Events.Raise(true, GameEventsEnum.EventGamePauseToggle);
    }

    private void ToggleActive(bool value)
    {
        gameObject.SetActive(value);
        
    }

    protected virtual void OnConfirmClick()
    {
        if (onConfirmEvent != 0)
            Events.Raise(onConfirmEvent);

        UIManager.Instance.CloseDialog();        
    }
}
