using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{

    private float xPos = 0;
    private float yPos = 0;

    public float jump = 3f;
    public float height = 1f;
    public float speed = 8f;
    //public float rotationspeed = 5f;

    private float relativespeed;
    //private float relativerotation;
    private bool jumping = false;
    private bool grounded = false;

    Rigidbody2D playerRB;
    public GameObject player;
    void respawn()
    {
        //Instantiate(player, Vector2.zero, Quaternion.Euler(Vector3.zero));
        transform.position = Vector2.zero;
        playerRB.velocity = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        jumping = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        grounded = true;
        Debug.Log("Grounded");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Deadly"))
        {
            respawn();
            Debug.Log("You died!");
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            
            Debug.Log("You Collected a coin!");
            Destroy(other.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        relativespeed = speed * Time.deltaTime;

        //Jump
        if (Input.GetButton("Jump") && !jumping && grounded)
        {
            playerRB.AddForce(Vector2.up * playerRB.gravityScale * jump, ForceMode2D.Impulse);
            Debug.Log("Jumping");
            jumping = true;
            grounded = false;
        }


        //Move Player
        xPos = Input.GetAxis("Horizontal");
        //yPos = Input.GetAxis("Vertical");

       
        //relativerotation = rotationspeed * relativespeed * 100 * Time.deltaTime / size * 10;

        transform.position += new Vector3(xPos * relativespeed, yPos * relativespeed);
        //Adds rotation while moving
        //transform.eulerAngles += new Vector3(0, 0, -xPos * relativerotation);
    }
}
