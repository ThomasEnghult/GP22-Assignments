using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    ScoreController scoreController;
    void Start()
    {
        scoreController = GameObject.Find("GameControllers").GetComponent<ScoreController>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            scoreController.AddScore(500);
            Destroy(gameObject);
        }
    }
}
