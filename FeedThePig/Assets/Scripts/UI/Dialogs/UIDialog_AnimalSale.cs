using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog_AnimalSale : UIDialog
{
    [SerializeField]
    private UIDynamicText weightText;
    [SerializeField]
    private UIDynamicText amountText;

    public override void OnDialogOpen(params string[] values)
    {
        weightText.UpdateContent(values[0]);
        amountText.UpdateContent(values[1]);

        base.OnDialogOpen(values);
    }
}
