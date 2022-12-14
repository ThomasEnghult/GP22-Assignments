using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : ProcessingLite.GP21
{
    int numOfBalls = 0;
    public float maxRandomSpeed = 4;
    public float minSize = 0.5f;
    public float maxSize = 1;

    public float playerSize = 1;
    public float playerSpeed = 10;

    string playerHighscore;
    int highscore = 0;
    string strBalls;
    bool cooldown = false;

    BallManager ballManager;

    // Start is called before the first frame update
    void Start()
    {
        ballManager = new BallManager(playerSize);
        Stroke(0);
        TextSize(40);

        ballManager.playerBall.speed = playerSpeed;
        ballManager.playerBall.maxSpeed = playerSpeed;
        ballManager.playerBall.setColor(0, 255, 0);

        StartCoroutine(spawnBall(3f));
    }

    // Update is called once per frame
    void Update()
    {
        //Set background to white
        Background(255);
        strBalls = "Balls: " + numOfBalls;
        Fill(0);
        TextSize(40);
        Text(strBalls, Width / 2, (Height * 0.75f) + 1);
        Text(playerHighscore, Width / 2, Height * 0.75f);
        TextSize(20);
        Text("R to respawn       ", 2, Height - 1);
        Text("G to toggle gravity: " + ballManager.gravity, 3, Height - 2);
        Text("C to toggle collision: " + ballManager.collision, 3, Height - 3);

        //Get player movement input
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        Vector2 ballDirection = new Vector2(xMove, yMove);
        ballManager.updatePlayer(ballDirection);


        //Toggle gravity
        if (Input.GetKeyDown(KeyCode.G))
            ballManager.gravity = !ballManager.gravity;

        //Toggle collision
        if (Input.GetKeyDown(KeyCode.C))
            ballManager.collision = !ballManager.collision;

        //Reset if you died
        if (Input.GetKeyDown(KeyCode.R) && !ballManager.isAlive)
        {
            Debug.Log("Respawn");
            if (numOfBalls > highscore)
                highscore = numOfBalls;

            playerHighscore = "Highscore: " + highscore;
            ballManager.playerBall.setColor(0, 255, 0);
            ballManager.isAlive = true;
            ballManager.resetBalls();
            numOfBalls = 0;
        }
        //Spawn new balls
        if (!cooldown && ballManager.isAlive)
        {
            cooldown = true;
            ballManager.Instantiate(minSize, maxSize, maxRandomSpeed);
            numOfBalls++;
            strBalls = "Balls: " + numOfBalls;
        }
        //Update position of all balls
        ballManager.updateBalls();
    }

    private IEnumerator spawnBall(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            cooldown = false;
        }
    }
}
