using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceLevelOnDeath : MonoBehaviour, IDeathEvent
{
    public void Raise()
    {
        GameManager.Instance.AdvanceLevel();
    }
}
