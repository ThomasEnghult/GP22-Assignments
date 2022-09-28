using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : ProcessingLite.GP21
{
    List<myBall> balls;
    public myBall playerBall;
    public bool isAlive;
    public bool gravity = false;
    public bool collision = true;

    public BallManager(float playerSize)
    {
        playerBall = new myBall(Width / 2, Height / 2, playerSize/2);
        isAlive = true;

        balls = new List<myBall>();
    }

    public void updateBalls()
    {
        for (int i = 0; i < balls.Count && isAlive; i++)
        {

            for (int j = i + 1; j < balls.Count && collision; j++)
            {
                if (balls[i].BallCollision(balls[j]))
                {
                    balls[i].BallCollider(balls[j]);
                }
            }
            updateBall(balls[i]);

            //Check if player died
            if (balls[i].BallCollision(playerBall))
            {
                Debug.Log("Game over!");
                playerBall.setColor(255, 0, 0);
                isAlive = false;
            }

        }
    }

    private void updateBall(myBall ball)
    {
        ball.gravity = gravity;
        ball.updateForce(Vector2.zero);
        ball.updatePos();
        ball.CheckWallCollision();
        ball.Draw();
    }

    public void updatePlayer(Vector2 move)
    {
        playerBall.updatePos(move);
        playerBall.CheckWallCollision();
        playerBall.Draw();
    }

    public void resetBalls()
    {
        balls.Clear();
        Debug.Log("Balls reset!");
    }

    public void Instantiate(float minSize, float maxSize, float maxSpeed)
    {
        float randomSize = Random.Range(minSize, maxSize);
        myBall newBall = new myBall(randomSize/2);

        //Prevent new ball to spawn on player
        bool collision = newBall.BallCollision(playerBall);

        //Prevent new ball to spawn on other ball
        for (int i = 0; i < balls.Count && !collision; i++)
        {
            if (newBall.BallCollision(balls[i]))
            {
                collision = true;
            }

        }

        if(collision)
        {
            //Try to spawn a new ball
            Instantiate(minSize, maxSize, maxSpeed);
            Debug.Log("Spawn blocked!");
        }
        else
        {
            spawnBall(newBall, maxSpeed);
        }

    }

    private void spawnBall(myBall newBall, float maxSpeed)
    {
        newBall.setRandomForce(maxSpeed);
        newBall.setColor(Random.Range(0, 255), 0, Random.Range(0, 255));
        balls.Add(newBall);
        Debug.Log("Added a new ball!");
    }
}