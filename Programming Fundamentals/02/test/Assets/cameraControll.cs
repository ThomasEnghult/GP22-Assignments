using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float size = player.transform.localScale.x;
        transform.position = player.transform.position + new Vector3(0,0,-10 * size);
        //Debug.Log(transform.position.z);
    }
}
