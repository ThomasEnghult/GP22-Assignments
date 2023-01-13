using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatusEffect
{
    None,
    Healed,
    Poisoned,
    Bleeding,
    Stunned
}

[CreateAssetMenu(fileName = "New StatusEffect", menuName = "Abilites/New StatusEffect")]
public class StatusEffectSO : ScriptableObject
{
    StatusEffect effect;
    int duration;

    public void ApplyStatus(Character other)
    {

    }
}
