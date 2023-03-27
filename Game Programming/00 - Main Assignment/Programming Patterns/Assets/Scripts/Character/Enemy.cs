using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth, IAction, IReceive
{
    public float maxHealth { get; set; }
    public float health { get; set; }

    public void GainHealth(float healthGained)
    {
        health = maxHealth > healthGained ? health + healthGained : maxHealth;
    }
    public void LoseHealth(float healthLost)
    {
        this.health -= healthLost;
        if(0 > health)
        {
            health = 0;
        }
    }

    public void InterruptAction()
    {
        throw new System.NotImplementedException();
    }


    public void PerformAction()
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveMessage()
    {
        throw new System.NotImplementedException();
    }
}
