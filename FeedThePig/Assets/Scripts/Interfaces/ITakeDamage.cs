using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    Transform DamageTextLocation { get; set; }
    void TakeDamage(int damage);
    void Kill();
}
