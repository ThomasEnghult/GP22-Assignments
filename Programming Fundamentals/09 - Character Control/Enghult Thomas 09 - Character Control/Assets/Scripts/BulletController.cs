using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
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
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(this.tag))
            Destroy(gameObject);
    }
}
