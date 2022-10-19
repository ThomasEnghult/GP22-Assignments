using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public Text scoreText;
    int score;

    void Start()
    {
        score = 0;
    }

	public void AddScore(int points)
    {
        score += points;
        scoreText.text = string.Format("SCORE: {0:0000000000}", score);
    }
}
