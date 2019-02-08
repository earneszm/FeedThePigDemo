using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    int Health { get; }
    Transform transform { get; }
}
