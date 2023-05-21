using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingWall : MonoBehaviour
{
    public float lifetime = 20;
    float speed = 5;
    Vector3 moveDirection;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetStats(float width, Vector3 moveDirection, float height, float speed)
    {
        this.moveDirection = moveDirection;
        this.speed = speed;
        transform.position += new Vector3(0,height / 2, 0);

        float xWidth = moveDirection.z == 0 ? 1 : Mathf.Abs(moveDirection.z) * width;
        float zWidth = moveDirection.x == 0 ? 1 : Mathf.Abs(moveDirection.x) * width;

        transform.localScale = new Vector3(xWidth, height,zWidth);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * moveDirection;
    }
}
