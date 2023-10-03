using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControll : MonoBehaviour
{
    float posX, posY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            posX = player.transform.position.x;
            //posY = player.transform.position.y;
        }
        //transform.position = player.transform.position + new Vector3(0,0,-10 * size);
        transform.position = new Vector3(posX, 0,-10);
        //Debug.Log(transform.position.z);
    }
}
