using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweekExample : MonoBehaviour
{
    public float xPos = 2;
    public float yPos = 3;
    public float size = 1f;
    public float xRot = 0;
    public float yRot = 0;
    public float zRot = 0;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(xPos, yPos);
        transform.localScale = Vector3.one * size;
        transform.eulerAngles = new Vector3(xRot, yRot, zRot);
    }
}
