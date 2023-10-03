using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
	public int health = 1;
	public float invincibile = 0.1f;
	public float speed = 1;
    public GameObject split;
    public GameObject Hit;
	public GameObject Explosion;
	public GameObject Gem;

	private PowerUpSpawner powerUpSpawner;
	private HealthBar playerHealthBar;
	private ScoreController scoreController;
	private bool isInvincible = true;

    void Start()
    {
		//Set a start speed
		GetComponent<Rigidbody2D>().velocity = -transform.up * speed;
		StartCoroutine(SpawnProtection(invincibile));
		powerUpSpawner = GameObject.Find("GameControllers").GetComponent<PowerUpSpawner>();
		scoreController = GameObject.Find("GameControllers").GetComponent<ScoreController>();
		playerHealthBar = GameObject.Find("HealthBarHolder").GetComponent<HealthBar>();
	}

	IEnumerator SpawnProtection(float time)
    {
		yield return new WaitForSeconds(time);
		isInvincible = false;
	}


    private void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.CompareTag("Player"))
		{
			playerHealthBar.LoseHealth();
		}
	}

    void OnTriggerEnter2D(Collider2D other)
	{

		//bullet hit the asteroid
		if (other.gameObject.CompareTag("Bullet"))
		{
			//Spawn hit effect
			Vector3 hitPoint = (other.transform.position + transform.position) / 2;
			var newHit = Instantiate(Hit, hitPoint, Quaternion.identity);
			Destroy(newHit, 0.5f);

			//Remove bullet
			Destroy(other.gameObject);

			//Lose health
			health--;


			//Asteroid was destroyed
			bool isDestroyed = !isInvincible && health <= 0;

			Debug.Log(gameObject.name);

            if (gameObject.name == "MeteorMedium(Clone)" && isDestroyed)
            {
				powerUpSpawner.mediumDestroyCounter++;
				if (powerUpSpawner.mediumDestroyCounter % 5 == 0)
				{
					powerUpSpawner.SpawnPowerUp(transform);
				}
            }
			//Spawn new asteroids
			if (split != null && isDestroyed)
			{
				Instantiate(split, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 30), transform.parent);
				Instantiate(split, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 30), transform.parent);
			}

            if (Gem != null && isDestroyed)
            {
				Instantiate(Gem, transform.position, Quaternion.identity);
			}

			//Spawn explostion effect
			var newExplosion= Instantiate(Explosion, transform.position, Quaternion.identity);
			Destroy(newExplosion, 0.5f);

            //Destroy asteroid
            if (isDestroyed)
            {
				scoreController.AddScore(100);
				Destroy(gameObject);
            }
		}
	}
}
