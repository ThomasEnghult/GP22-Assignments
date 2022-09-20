using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0, 0, -10), 5*Time.deltaTime);
        //transform.position = player.transform.position + new Vector3(0, 0, -10);
    }
}
