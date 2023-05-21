using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{

    public GameObject platform;
    float speed = 0;
    float stopAtHeight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(float spawnHeight, float speed)
    {
        if(stopAtHeight > spawnHeight)
        {
            return;
        }

        this.speed = speed;
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = spawnHeight;
        GameObject newPlatform = Instantiate(platform, spawnPosition, Quaternion.identity, transform);

        newPlatform.GetComponent<Platform>().speed = speed;
        newPlatform.GetComponent<Platform>().stopY = stopAtHeight;
        stopAtHeight += newPlatform.transform.lossyScale.y;
    }
}
