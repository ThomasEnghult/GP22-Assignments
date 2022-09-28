using UnityEngine;

public class ModifiedWalkerTest : ProcessingLite.GP21
{
	//This file is only for testing your movement/behavior.
	//The Walkers will compete in a different program!

	IRandomWalker walker;
	Vector2 walkerPos;
	float scaleFactor = 0.05f;

	void Start()
	{
		//Some adjustments to make testing easier
		Application.targetFrameRate = 120;
		QualitySettings.vSyncCount = 0;
		Fill(255, 0, 0);
		Stroke(255, 0, 0);

		//Create a walker from the class Example it has the type of WalkerInterface
		walker = new ThoEng();

		//Get the start position for our walker.
		walkerPos = walker.GetStartPosition((int)(Width / scaleFactor), (int)(Height/ scaleFactor));
		walkerPos = new Vector2(Width / 2, 0);
	}

	void Update()
	{
		//Draw the walker
		Square(walkerPos.x, walkerPos.y, scaleFactor/10);
		//Get the new movement from the walker.
		walkerPos += walker.Movement() * scaleFactor;
	}
}