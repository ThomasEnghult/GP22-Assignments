using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public float powerUpDuration = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, powerUpDuration + 1);
        UsePowerUp(other.gameObject);

    }

    private void UsePowerUp(GameObject player)
    {
        switch (name)
        {
            case "PowerUp1(Clone)":
                Debug.Log("Powerup 1 used");
                PlayerFire script = player.GetComponent<PlayerFire>();
                StartCoroutine(PowerUp1(script, powerUpDuration));
                return;

            case "PowerUp2":

            case "PowerUp3":

            default:
                return;
        }
    }

    IEnumerator PowerUp1(PlayerFire script, float time)
    {
        float bonus = script.roundsPerSecond;
        script.roundsPerSecond += bonus;
        yield return new WaitForSeconds(time);
        script.roundsPerSecond -= bonus;
    }

}
