using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float xMove, yMove;
    public float movementspeed = 5;
    public float sprint = 2;
    public Transform elbow_left, elbow_right;
    Vector3 currentEulerAngles;
    public float rotationSpeed;
    public float x, y, z;

    // Start is called before the first frame update
    void Start()
    {
        currentEulerAngles = elbow_left.localEulerAngles;
        // rotation of arms
        x = 360;
        rotationSpeed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        //Get movement input
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");

        if (Input.GetMouseButton(0))
        {
            Vector3 aim = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            aim = aim.normalized;
            xMove = aim.x;
            yMove = aim.y;
        }

        //Set movespeed
        float movement = movementspeed * Time.deltaTime;

        //If sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement = movement * sprint;
        }

        // If moving
        if (yMove != 0 || xMove != 0)
        {
            //Move player
            transform.position += new Vector3(xMove, yMove) * movement;
            transform.up = new Vector3(xMove, yMove);

            //Rotate arms
            currentEulerAngles += new Vector3(x, y, z) * movement * rotationSpeed;

        }
        else
        {
            //Set arms to idle
            currentEulerAngles = new (90, currentEulerAngles.y, currentEulerAngles.z);
        }

        //Set the arms position
        elbow_left.localEulerAngles = currentEulerAngles;
        elbow_right.localEulerAngles = currentEulerAngles;
    }
}


