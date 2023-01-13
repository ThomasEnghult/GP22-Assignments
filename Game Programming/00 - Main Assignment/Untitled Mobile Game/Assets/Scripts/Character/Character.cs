using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IAction
{
    List<StatusEffectSO> statusEffects;
    bool isAlive { get; set; }

    public Character()
    {
        this.statusEffects = new List<StatusEffectSO>();
        this.isAlive = true;
        
    }

    public void TargetAction(Character target)
    {

    }
}
