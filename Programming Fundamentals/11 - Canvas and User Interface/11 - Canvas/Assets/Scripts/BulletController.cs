using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public AudioSource ricochet;
    public AudioSource destroyed;

    public float lifeTime = 5;


    void Start()
    {
        StartCoroutine(bulletLifeTime(lifeTime));

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * 10;
        gameObject.GetComponent<Rigidbody2D>().mass += gameObject.transform.localScale.magnitude / 2;

    }

    IEnumerator bulletLifeTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DestroyBullet();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(this.tag))
            DestroyBullet();
        else
        {
            randomPitch(ricochet);
            ricochet.Play(0);
        }
    }

    private void DestroyBullet()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, 1);
        randomPitch(destroyed);
        destroyed.Play(0);
    }

    private void randomPitch(AudioSource audio)
    {
        float current = audio.pitch;
        audio.pitch = current + Random.Range(-0.25f, 0.25f);
    }
}
