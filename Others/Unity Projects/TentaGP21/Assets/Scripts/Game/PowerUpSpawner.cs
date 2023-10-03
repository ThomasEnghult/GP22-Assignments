using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUp1;
    public GameObject powerUp2;
    public GameObject powerUp3;

    public int mediumDestroyCounter = 0;

    public void SpawnPowerUp(Transform parent)
    {
        Instantiate(powerUp1, parent.position, Quaternion.identity);
    }
}
