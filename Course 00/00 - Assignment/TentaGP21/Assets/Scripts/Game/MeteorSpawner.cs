using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteor;
    public int amount = 5;

	float width, height;

	private float preventSpawnDist = 2;

    void Start()
	{
		//Get screen size
		width = Camera.main.orthographicSize * Camera.main.aspect;
		height = Camera.main.orthographicSize;

		//Spawn the first wave
		SpawnWave(amount);
	}

	private void SpawnWave(int numberOfAsteroids)
	{
		//Spawn meteors
		for (int i = 0; i < numberOfAsteroids; i++)
		{
			SpawnMeteor();
		}
	}

	private void SpawnMeteor()
	{
		Vector2 randomPosition = new Vector2();
		randomPosition.x = Random.Range(-width, width);
		randomPosition.y = Random.Range(-height, height);

		//Prevent Spawn on top of player
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (Mathf.Abs(player.transform.position.x - randomPosition.x) < preventSpawnDist &&
			Mathf.Abs(player.transform.position.y - randomPosition.y) < preventSpawnDist)
        {
			Debug.Log("Player Located at: " + player.transform.position);
			Debug.Log("Spawn Prevented at: " + randomPosition);
			SpawnMeteor();
        }
		else
		{
			//Spawn meteor
			Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
			Instantiate(meteor, randomPosition, randomRotation, transform);
		}
	}

    void Update()
    {
        if(transform.childCount == 0)
			SpawnWave(amount);
	}
}
