using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceLevelOnDeath : MonoBehaviour, IDeathEvent
{
    public void Raise()
    {
        Events.StartCoroutine(DoAdvance(1.5f));
    }

    private IEnumerator DoAdvance(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);

        Events.Raise(GameEventsEnum.EventAdvanceLevel);
    }
}
