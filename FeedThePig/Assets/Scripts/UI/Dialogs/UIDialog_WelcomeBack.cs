using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog_WelcomeBack : UIDialog
{
    [SerializeField]
    private UIDynamicText activityText;
    [SerializeField]
    private UIDynamicText earnedText;

    public override void OnDialogOpen(params string[] values)
    {
        activityText.UpdateContent(values[0]);
        earnedText.UpdateContent(values[1]);

        base.OnDialogOpen(values);
    }
}
