using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class WallSpawner : AttributesSync
{

    public GameObject movingWall;
    public float wallRadius = 10;
    public float wallHeight = 5;
    public float barrierHeight = 1;
    public float maxBarrierHeight = 2;

    public Vector3 moveDirection;
    public float gapMinWidth = 2;
    public float gapMaxWidth = 4;
    float wallSpeed = 5;


    float spawnObstacleTimer = 3;
    float spawnCounter = 0;
    int increaseDifficultyAfterTime = 5;
    float difficultyCounter = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = -Vector3.forward;
        //InvokeRemoteMethod(nameof(SetInitState), UserId.AllInclusive, UnityEngine.Random.state);
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
        if(difficultyCounter > increaseDifficultyAfterTime)
        {
            IncreaseDifficulty();
            difficultyCounter %= increaseDifficultyAfterTime;
        }
    }

    private void IncreaseDifficulty()
    {
        wallSpeed *= 1.1f;
        spawnObstacleTimer *= 0.9f;
        spawnCounter -= 1;
        barrierHeight = barrierHeight < maxBarrierHeight ? barrierHeight + 0.1f : maxBarrierHeight;
    }

    private void SpawnObstacle()
    {
        int randomObstacle = UnityEngine.Random.Range(0, 2);
        switch (randomObstacle)
        {
            case 0:
                SpawnWalls();
                break;
            case 1:
                SpawnWall(0, Vector3.zero, barrierHeight);
                break;

            default:
                break;
        }
    }

    private void SpawnWalls()
    {
        float gapWidth = UnityEngine.Random.Range(gapMinWidth, gapMaxWidth);
        float wallSpawn = UnityEngine.Random.Range(gapWidth, wallRadius);
        Vector3 sidewaysDirection = new Vector3(moveDirection.z, 0, moveDirection.x);

        SpawnWall(wallSpawn, sidewaysDirection, wallHeight);

        wallSpawn = wallRadius - wallSpawn + gapWidth;

        SpawnWall(wallSpawn, -sidewaysDirection, wallHeight);
    }

    private void SpawnWall(float wallSpawn, Vector3 sidewaysDirection, float wallHeight)
    {
        float wallWidth = (wallRadius - wallSpawn) * 2;
        
        GameObject newWall = Instantiate(movingWall, transform.position + sidewaysDirection*wallSpawn, Quaternion.identity, transform);
        newWall.GetComponent<MovingWall>().SetStats(wallWidth, moveDirection, wallHeight, wallSpeed);
    }

    [SynchronizableMethod]
    public void SetInitState(UnityEngine.Random.State seed)
    {
        UnityEngine.Random.state = seed;
        Debug.Log("Set seed to: " + seed);
        spawnCounter = 0;
    }
}
