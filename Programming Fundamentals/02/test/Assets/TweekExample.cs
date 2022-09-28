using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweekExample : MonoBehaviour
{
    public GameObject laserprefab;
    public Transform gun;

    private float xPos = 0;
    private float yPos = 0;
    public float size = 1f;
    public float jump = 5f;
    //public float xRot = 0;
    //public float yRot = 0;
    //public float zRot = 0;
    public float speed = 5f;
    public float rotationspeed = 5f;
    private float relativespeed;
    private float relativerotation;
    private bool canjump = true;
    public float fireRate = 0.2f;
    private float timeFromShot = 0;


Rigidbody2D cube;


    void Start()
    {
        cube = GetComponent<Rigidbody2D>();
    }

    //Reset jump on collision
    void OnCollisionEnter2D(Collision2D other)
    {
        canjump = true;
    }

    // Update is called once per frame
    void Update()
    {

        timeFromShot += Time.deltaTime;
        //Shoot laser, works on 2D
        Debug.Log("mouse pos: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2 aim = Camera.main.ScreenToWorldPoint(Input.mousePosition)- transform.position;
        float aimAngle = Vector2.SignedAngle(Vector2.up, aim);
        if (Input.GetMouseButton(0) && timeFromShot > fireRate)
        {
            Vector3 aim3D = new Vector3(aim.x, aim.y, 0); //Used as offset from player
            Instantiate(laserprefab, transform.position + aim3D.normalized, Quaternion.Euler(0, 0, aimAngle));
            timeFromShot = 0;
        }

        gun.rotation = Quaternion.Euler(0, 0, aimAngle + 90);


        //Jump
        if (Input.GetButtonDown("Jump") && canjump)
        {
            cube.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            canjump = false;
        }

        //Change Size
        if (Input.GetKey(KeyCode.Q))
        {
            size += 1 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            size += -1 * Time.deltaTime;
        }
        transform.localScale = Vector3.one * size;


        //Move Player
        xPos = Input.GetAxis("Horizontal");
        yPos = Input.GetAxis("Vertical");

        relativespeed = speed * Time.deltaTime;
        relativerotation = rotationspeed * relativespeed*100 *Time.deltaTime /size*10;

        transform.position += new Vector3(xPos * relativespeed, yPos * relativespeed);
        //Adds rotation while moving
        transform.eulerAngles += new Vector3(0, 0, -xPos* relativerotation);


        //Vector2 aim = transform.position - Input.mousePosition;
        //transform.up = aim;

    }

}
