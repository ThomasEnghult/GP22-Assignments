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
    int count = 0;

	//Makes a heart shape
	//string[] movementInputs = { "Forward", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Forward", "Right", "Left", "Right", "Left", "Right", "Forward", "Right", "Left", "Right", "Left", "Forward", "Left", "Right", "Left", "Right", "Forward", "Right", "Left", "Right", "Left", "Right", "Forward", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Back", "Forward", "Forward", "Forward", "Forward", "Forward", "Forward", "Forward", "Forward", "Forward", "Forward", "Forward", "Forward", "Random" };
	string[] movementInputs = { "Forward", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right","Left", "Right", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Right", "Forward", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right", "Left", "Right","Left", "Forward", "Left", "Right", "Left", "Right", "Left", "Forward", "Left", "Right", "Left", "Right", "Forward", "Right", "Left", "Right", "Left", "Forward","Left", "Right", "Left", "Right","Forward" ,"Left" , "Left", "Right", "Right","Left", "Right", "Random"};

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
		Debug.Log(playerPosition);
		//a PVector holds floats but make sure its whole numbers that are returned!
		return playerPosition;
	}

	public Vector2 Movement()
	{
		//add your own walk behavior for your walker here.
		//Make sure to only use the outputs listed below.
		SetDirection(movementInputs[count % movementInputs.Length]);
		count++;
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
                string[] random = {"Forward", "Left", "Right"};
				SetDirection(random[Random.Range(0, random.Length)]);
				return;
            default:
				return;
        }

    }
}

//All valid outputs:
// Vector2(-1, 0);
// Vector2(1, 0);
// Vector2(0, 1);
// Vector2(0, -1);

//Any other outputs will kill the walker!