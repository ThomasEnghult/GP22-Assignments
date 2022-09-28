using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHuman : MyCharacter
{
    GameObject human;

    public MyHuman(GameObject human)
    {
        this.human = Instantiate(human, Vector3.zero, Quaternion.identity);
    }

    public GameObject GetGameObject()
    {
        return human;
    }

}
