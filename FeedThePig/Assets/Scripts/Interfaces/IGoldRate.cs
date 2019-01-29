using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoldRate
{
    float GoldRatePerMinute { get; }
    void AddGold(int amount);
}
