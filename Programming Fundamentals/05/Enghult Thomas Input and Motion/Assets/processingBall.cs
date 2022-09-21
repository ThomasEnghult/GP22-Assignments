using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class processingBall : ProcessingLite.GP21
{
    public float ballRadius = 0.5f;
    public float ballMaxSpeed = 20;
    public float ballSpeed1 = 5f;
    public float ballSpeed2 = 50;

    //Should be positive to slow the ball down
    public float ballDrag = 0.2f;
    //Should be a value between 0-1;
    public float ballBounce = 0.8f;
    public float gravity = 10;
    public bool haveGravity = false;

    Vector3 ballPosition1, ballPosition2;
    Vector3 ballForce2;
    Vector3 gravityForce;



    // Start is called before the first frame update
    void Start()
    {
        //Set color to black
        Stroke(0);
        ballPosition1 = new Vector3(Width / 2, Height / 2);
        ballPosition2 = new Vector3(Width / 2, Height / 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Set background to white
        Background(255);

        //Toggle gravity
        if (Input.GetKeyDown(KeyCode.G))
            haveGravity = !haveGravity;

        if (haveGravity)
            gravityForce = gravity * Vector3.down;
        else
            gravityForce = Vector3.zero;

        //Get player movement input
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        Vector3 ballDirection = new Vector3(xMove, yMove);

        // Add players input to ball2 force vector
        ballForce2 += ballDirection * ballSpeed2 * Time.deltaTime;

        // Restrict ball max speed
        ballForce2 = Vector3.ClampMagnitude(ballForce2, ballMaxSpeed);
        if (ballSpeed1 > ballMaxSpeed)
            ballSpeed1 = ballMaxSpeed;

        //Set ball position with input direction
        ballPosition1 += ballDirection * ballSpeed1 * Time.deltaTime;
        //Warp it to the other side if it's outside the horizontal edge
        ballPosition1 = WrapHorizontalEdge(ballPosition1);
        //Stop the ball from leaving the area
        ballPosition1 = StopVerticalEdge(ballPosition1);
        DrawBall(ballPosition1, 0, 0, 255);

        //Add airdrag
        ballForce2 -= ballForce2 * (ballDrag * Time.deltaTime);
        //Add gravity
        ballForce2 += gravityForce * Time.deltaTime;
        //Invert force if it collided with vertical edge
        ballPosition2 = StopVerticalEdge(ballPosition2);
        ballForce2 = InvertVerticalEdge(ballPosition2, ballForce2, ballBounce);

        //Set ball position with added force
        ballPosition2 += ballForce2 * Time.deltaTime;
        //Warp it to the other side if it's outside the horizontal edge
        ballPosition2 = StopVerticalEdge(ballPosition2);
        ballPosition2 = WrapHorizontalEdge(ballPosition2);

        DrawBall(ballPosition2, 255, 0, 0);

    }


    void DrawBall(Vector3 ballPosition, int r, int g, int b)
    {
        //Set color
        Stroke(r, g, b);
        //Draw circle
        Circle(ballPosition.x, ballPosition.y, ballRadius * 2);

        //Draw additional circles for the wrap around
        //Right side
        Circle(ballPosition.x + Width, ballPosition.y, ballRadius * 2);
        //Left side
        Circle(ballPosition.x - Width, ballPosition.y, ballRadius * 2);
        //Top side
        //Circle(ballPosition.x, ballPosition.y + Height, ballRadius * 2);
        //Bottom side
        //Circle(ballPosition.x, ballPosition.y - Height, ballRadius * 2);
    }


    Vector3 WrapHorizontalEdge(Vector3 ballPosition)
    {
        //Handles wrap around on right side
        if (ballPosition.x > Width)
            ballPosition.x = 0;
        //Handles wrap around on left side
        if (ballPosition.x < 0)
            ballPosition.x = Width;
        return ballPosition;
    }

    Vector3 StopVerticalEdge(Vector3 ballPosition)
    {
        //Handles stop on top side
        if (ballPosition.y > Height - ballRadius)
            ballPosition.y = Height - ballRadius;
        //Handles wrap around on left side
        else if (ballPosition.y < 0 + ballRadius)
            ballPosition.y = 0 + ballRadius;
        return ballPosition;
    }
    Vector3 InvertVerticalEdge(Vector3 ballPosition, Vector3 ballForce, float ballBounce)
    {
        //Invert the force if it hits top side
        if (ballPosition.y >= Height - ballRadius)
            ballForce.y *= -ballBounce;
        //Invert the force if it hits bottom side
        if (ballPosition.y <= 0 + ballRadius)
            ballForce.y *= -ballBounce;
        return ballForce;
    }

}
