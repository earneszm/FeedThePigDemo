using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; set; }

    private List<UIButton_MenuNav> navButtons = new List<UIButton_MenuNav>();
    private UIButton_MenuNav currentButton;

    private GameObject dialogCanvas;
    private Dictionary<DialogTypeEnum, UIDialog> dialogList = new Dictionary<DialogTypeEnum, UIDialog>();
    private Stack<UIDialog> dialogStack = new Stack<UIDialog>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    private void Start()
    {
        navButtons = FindObjectsOfType<UIButton_MenuNav>().ToList();
        dialogCanvas = GameObject.FindWithTag("DialogCanvas");

        if (dialogCanvas == null)
            Debug.LogError("Could not find DialogCanvas in scene");
        else
            dialogList = dialogCanvas.GetComponentsInChildren<UIDialog>(true).ToDictionary(x => x.DialogType);
    }

    public void OnNavButtonClicked(UIButton_MenuNav button)
    {
        if (button == currentButton)
            return;
        
        currentButton = button;

        foreach (var item in navButtons)
        {
            item.menuContent.gameObject.SetActive(false);
        }
        
        button.menuContent.gameObject.SetActive(true);
    }

    #region Manage Dialog Stack

    private UIDialog PushDialog(DialogTypeEnum dialog)
    {
        dialogStack.Push(dialogList[dialog]);
        return dialogStack.Peek();
    }

    public void OpenDialog(DialogTypeEnum dialog, object data = null)
    {
        PushDialog(dialog).OnDialogOpen(data);
    }

    public void OpenDialog(DialogTypeEnum dialog, params string [] data)
    {
        PushDialog(dialog).OnDialogOpen(data);
    }

    public void CloseDialog()
    {
        if (dialogStack.Count() > 0)
        {
            var dialog = dialogStack.Pop();
            dialog.gameObject.SetActive(false);
        }
    }

    #endregion  
}

public enum DialogTypeEnum
{
    NotSet,
    AnimalSale,
    WelcomeBack
}

public enum CustomButtonTypeEnum
{
    NotSet,
    MenuNavigation,
    Confirm,
    Cancel
}
