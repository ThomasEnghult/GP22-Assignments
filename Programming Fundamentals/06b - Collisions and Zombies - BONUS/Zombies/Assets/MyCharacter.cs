using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MyCharacter : MonoBehaviour
{
    public Vector3 position;
    Vector3 velocity;
    float movespeed = 1;


    public void MoveCharacter(GameObject character, Vector3 velocity)
    {
        character.transform.position += velocity * Time.deltaTime;
    }
    public void updateCharacter(GameObject character)
    {
        character.transform.position += velocity * Time.deltaTime;
    }


}


