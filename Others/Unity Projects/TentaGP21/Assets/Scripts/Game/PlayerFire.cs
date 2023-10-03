using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bullet;
    public float roundsPerSecond;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;

		if (Input.GetButton("Jump") && timer > 1/roundsPerSecond)
		{
            Instantiate(bullet, transform.position, transform.rotation);
            timer = 0;
		}
    }
}
