using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    public float speed = 2;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.drag = 0.5f;
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 aim = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        aim.z = 0;
        aim.Normalize();
        transform.up = aim;

        //Move with mouse
        if (Input.GetMouseButton(0))
        {
            x = aim.x;
            y = aim.y;
        }

        Vector2 movement = new Vector2(x, y) * speed;

        body.AddForce(movement / 5, ForceMode2D.Impulse);

        //body.velocity = movement*5;
        //transform.Translate(movement);
        //transform.position += movement;
        
    }
}
