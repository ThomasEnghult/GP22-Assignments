using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMediator : MonoBehaviour
{
    
}

abstract class Mediator
{
    public abstract void SendMessage(string message);
}
