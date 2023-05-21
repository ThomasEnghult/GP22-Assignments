using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformSpawner : MonoBehaviour
{

    SpawnPlatform[] spawners;

    float fallingHeight = 40;
    float fallingSpeed = 5;

    float spawnObstacleTimer = 3;
    float spawnCounter = 0;
    int increaseDifficultyAfterTime = 5;
    float difficultyCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawners = transform.GetComponentsInChildren<SpawnPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter += Time.deltaTime;

        if (spawnCounter > spawnObstacleTimer)
        {
            SpawnObstacle();
            spawnCounter %= spawnObstacleTimer;
        }

        difficultyCounter += Time.deltaTime;
        if (difficultyCounter > increaseDifficultyAfterTime)
        {
            IncreaseDifficulty();
            difficultyCounter %= increaseDifficultyAfterTime;
        }
    }

    private void IncreaseDifficulty()
    {
        fallingSpeed *= 1.05f;
        spawnObstacleTimer *= 0.95f;
    }

    private void SpawnObstacle()
    {
        int randomPlatform = UnityEngine.Random.Range(0, spawners.Length);
        spawners[randomPlatform].Spawn(fallingHeight, fallingSpeed);
    }
}
