using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : ProcessingLite.GP21
{
    public float ballAcceleration = 5;
    public float ballMaxSpeed = 25;
    public float ballDrag = 0.9f;
    float ballSpeed;
    float xPos, yPos;

    Vector3 ballDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xPos = Input.GetAxis("Horizontal");
        yPos = Input.GetAxis("Vertical");
        ballDirection = new Vector3(xPos, yPos);

        if (ballDirection == Vector3.zero)
            ballSpeed *= ballDrag;
        else
        {
            ballSpeed += ballAcceleration;
        }

        if (ballSpeed > ballMaxSpeed)
        {
            Debug.Log("Max Speed reached!");
            ballSpeed = ballMaxSpeed;
        }

        transform.position += ballDirection * ballSpeed * Time.deltaTime;
    }
}
