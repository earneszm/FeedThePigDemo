using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog_WelcomeBack : UIDialog
{
    [SerializeField]
    private UIDynamicText activityText;
    [SerializeField]
    private UIDynamicText earnedText;
    [SerializeField]
    private UIDynamicText weightText;

    public override void OnDialogOpen(params string[] values)
    {
        activityText.UpdateContent(values[0]);
        earnedText.UpdateContent(values[1]);
        weightText.UpdateContent(values[2]);

        base.OnDialogOpen(values);
    }
}
