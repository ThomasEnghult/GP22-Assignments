using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    [Range(0, 25)][SerializeField] int moveSpeed = 8;
    [Range(0, 25)][SerializeField] int jumpHeight = 8;
    AudioSource jumpSound;
    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        jumpSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");

        if (x == 1)
            transform.eulerAngles = new (transform.rotation.x, 0);

        if (x == -1)
            transform.eulerAngles = new(transform.rotation.x, 180);

        transform.position += new Vector3(x*moveSpeed, 0) * Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Jump()
    {
        rb2d.AddForce(Vector2.up * jumpHeight * 2, ForceMode2D.Impulse);
        jumpSound.Play(0);
    }

}
