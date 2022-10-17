using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    float speed = 5;
    private Rigidbody2D rb2d;
    [Range(1, 60)][SerializeField]int roundsPerMin = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Fire.fireCooldown = 1f / roundsPerMin;

        Vector2 movement = new Vector2(x, y) * speed;

        //rb2d.AddForce(movement / 5, ForceMode2D.Impulse);
        rb2d.velocity = movement;
        transform.up = movement;
        //transform.Translate(movement);
        //transform.position += movement;
    }
}
