using UnityEngine;

public class WalkerTest : ProcessingLite.GP21
{
	//This file is only for testing your movement/behavior.
	//The Walkers will compete in a different program!

	IRandomWalker walker;
	Vector2 walkerPos;
	float scaleFactor = 0.04f;
	[SerializeField][Range(0, 240)] int frameRate = 120;

	void Start()
	{
		Background(0);
		Fill(255, 0, 0);
		Stroke(255, 0, 0);
		//Some adjustments to make testing easier
		//Application.targetFrameRate = frameRate;
		QualitySettings.vSyncCount = 0;

		//Create a walker from the class Example it has the type of WalkerInterface
		walker = new ThoEng();

		//Get the start position for our walker.
		walkerPos = walker.GetStartPosition((int)(Width / scaleFactor), (int)(Height / scaleFactor));
	}

	void Update()
	{
		//Draw the walker
		Point(walkerPos.x * scaleFactor, walkerPos.y * scaleFactor);
		//Get the new movement from the walker.
		walkerPos += walker.Movement();

		if (Input.GetMouseButtonDown(0))
        {
			Start();
        }

	}
}