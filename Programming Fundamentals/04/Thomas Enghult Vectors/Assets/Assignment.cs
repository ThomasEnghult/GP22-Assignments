using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment : ProcessingLite.GP21
{

    Vector2 start, move, deltaV;
    public bool existingCircle = false;
    public bool keepVelocity = false;
    public float mouseGravity = 1;
    public float drag = 0;
    public float ballRadius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Set color to black
        Stroke(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Set background to white
        Background(255);

        if (Input.GetMouseButtonDown(0) && !existingCircle)
        {
            existingCircle = true;
            //Save starting position
            start = new Vector2(MouseX, MouseY);
            Debug.Log("Starting vector: " + start);
        }

        if (Input.GetMouseButton(0))
        {
            //Distance between circle and mouse
            deltaV = new Vector2(MouseX, MouseY) - start;

            if(move == Vector2.zero || !keepVelocity)
            {
                //Move circle to mouse
                move = deltaV * mouseGravity;
            }
            else
            {
                //Add deltaV vector to existing moving direction
                move += mouseGravity * Time.deltaTime * deltaV;
            }

        }

        //Handles bounce on right side
        if (start.x > Width - ballRadius)
        {
            start.x = Width - ballRadius;
            move = new Vector2(-move.x, move.y);
        }

        //Handles bounce on left side
        if (start.x < 0 + ballRadius)
        {
            start.x = 0 + ballRadius;
            move = new Vector2(-move.x, move.y);
        }

        //Handles bounce on top side
        if (start.y > Height - ballRadius)
        {
            start.y = Height - ballRadius;
            move = new Vector2(move.x, -move.y);
        }

        //Handles bounce on bottom side
        if (start.y < 0 + ballRadius)
        {
            start.y = 0 + ballRadius;
            move = new Vector2(move.x, -move.y);
        }

        //changes start position of circle depending on move
        start += move * Time.deltaTime;

        //Adds drag to the circle, slowing it down
        move = move - (move * Time.deltaTime)*drag;

        if (existingCircle)
        {
            if (Input.GetMouseButton(0))
            {
                // Draws line from cicle to mouse
                Line(start.x, start.y, MouseX, MouseY);
            }

            //Draw circle
            Circle(start.x, start.y, ballRadius*2);
        }
    }
}
