using UnityEngine;

class ThoEng : IRandomWalker
{
    //Add your own variables here.
    Vector2 playerPosition;
    Vector2 forward = Vector2.up;
    Vector2 left = Vector2.left;
    Vector2 right = Vector2.right;
    Vector2 back = Vector2.down;


    int width, height;
    int totalMovesMade = 0;

    string[] moves = {"Left","Right" ,"Forward" ,"Back" };

    //Makes a improved heart shape
    string[] HeartInputs = { "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right",
        "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Right", "Forward", "Right",
        "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right",
        "Left", "Right", "Left", "Right", "Left", "Forward", "Left", "Right", "Left", "Right", "Left", "Forward", "Left", "Right", "Left", "Right", "Forward",
        "Right", "Left", "Right", "Left", "Forward", "Left", "Right", "Left", "Right", "Forward", "Left", "Left", "Right", "Right", "Left", "Right"};
    // Last input of art


    public string GetName()
    {
        return "Thomas"; //When asked, tell them our walkers name
    }

    public Vector2 GetStartPosition(int playAreaWidth, int playAreaHeight)
    {
        Debug.Log("playWidth: " + playAreaWidth + " playHeight: " + playAreaHeight);
        width = playAreaWidth;
        height = playAreaHeight;


        float x = GetRandomStart(width);
        float y = GetRandomStart(height);
        playerPosition = new Vector2(x, y);
        //playerPosition = new Vector2(playAreaWidth / 2, playAreaHeight / 2);
        Debug.Log("Player spawned at: " + playerPosition);
        return playerPosition;
    }

    public Vector2 Movement()
    {
        if (CloseToEdge())
            return forward;
        SetDirection(HeartInputs[totalMovesMade % HeartInputs.Length]);
        totalMovesMade++;
        playerPosition += forward;
        return forward;
    }

    private float GetRandomStart(int playArea)
    {
        float magnitude;
        float offset;
        // +- 1/8th of player area
        magnitude = playArea / 8f;
        magnitude = Random.Range(-magnitude, magnitude);

        // -1 or 1
        int randomOffset = Random.Range(0, 2) * 2 - 1;
        //1/4th or 3/4th of player area
        offset = playArea * ((2f + randomOffset) / 4f);
        Debug.Log("Random: " + randomOffset + " Offset: " + offset + " Magnitude: " + magnitude);
        return offset + magnitude;
    }

    private void SetDirection(string direction)
    {
        switch (direction)
        {
            case "Left":
                forward = left;
                left = back;
                back = -forward;
                right = -left;
                return;

            case "Right":
                forward = right;
                right = back;
                back = -forward;
                left = -right;
                return;

            case "Back":
                forward = back;
                back = -forward;
                left = right;
                right = -left;
                return;

            case "Random":
                SetDirection(moves[Random.Range(0, 3)]);
                return;

            case "Clockwise":
                if (CloseToEdge())
                {
                    SetDirection("Right");
                    Debug.Log("Rotated Clockwise");
                }
                return;

            case "AvoidEdge":
                float randomOffset = Mathf.PerlinNoise(totalMovesMade * 0.05f, 0);
                if (CloseToEdge(randomOffset))
                    SetDirection("Left");
                else
                    SetDirection("Forward");
                Debug.Log("AvoidEdge");
                return;

            case "Heart of heart":
                int completedHearts = totalMovesMade / HeartInputs.Length;
                int nextDirection = completedHearts % (HeartInputs.Length);
                if (nextDirection == HeartInputs.Length - 1)
                    SetDirection("Random");
                else
                    SetDirection(HeartInputs[nextDirection]);
                return;

            default:
                return;
        }
    }

    private bool CloseToEdge()
    {
        float edgeOffset = (0.5f + 2*Mathf.PerlinNoise(totalMovesMade * 0.05f, 0)) / 10;
        //string[] moves = { "Forward", "Left", "Right", "Back"};
        //Close to top edge
        if (forward == Vector2.up && playerPosition.y > height * (1 - edgeOffset))
        {
            SetDirection(moves[Random.Range(0, 2)]);
            return true;
        }
        //Close to bottom edge
        if (forward == Vector2.down && playerPosition.y < height * edgeOffset)
        {
            SetDirection(moves[Random.Range(0, 2)]);
            return true;
        }
        //Close to right edge
        if (forward == Vector2.right && playerPosition.x > width * (1 - edgeOffset))
        {
            SetDirection(moves[Random.Range(0, 2)]);
            return true;
        }
        //Close to left edge
        if (forward == Vector2.left && playerPosition.x < width * edgeOffset)
        {
            SetDirection(moves[Random.Range(0, 2)]);
            return true;
        }
        return false;
    }

    private bool CloseToEdge(float edgeOffset)
    {
        //float edgeOffset = 0.3f;
        float x = playerPosition.x - (width / 2f);
        float y = playerPosition.y - (height / 2f);
        float maxWidth = (width / 2f) * (1 - edgeOffset);
        float maxHeight = (height / 2f) * (1 - edgeOffset);



        Debug.Log("x: " + x + " maxWidth: " + maxWidth + "   y: " + y + " maxHeight: " + maxHeight);

        //Close to right edge
        if (x > maxWidth)
        {
            return true;
        }
        //Close to top edge
        if (y > maxHeight)
        {
            return true;
        }
        //Close to left edge
        if (-x > maxWidth)
        {
            return true;
        }
        //Close to bottom edge
        if (-y > maxHeight)
        {
            return true;
        }
        return false;
    }
}

//All valid outputs:
// Vector2(-1, 0);
// Vector2(1, 0);
// Vector2(0, 1);
// Vector2(0, -1);

//Any other outputs will kill the walker!