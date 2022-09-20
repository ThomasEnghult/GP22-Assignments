using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public float lerpness;
    // Start is called before the first frame update
    void Start()
    {
        lerpness = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //Get player gameobject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;

        Vector3 startCamera = transform.position;
        Vector3 endCamera = playerPos;


        if (Input.GetMouseButton(1))
        {
            Vector3 aim = Camera.main.ScreenToWorldPoint(Input.mousePosition)- playerPos;
            endCamera = playerPos + aim / 2;
        }

        endCamera.z = -10;

        //Move camera towards player
        transform.position = Vector3.Lerp(startCamera, endCamera, lerpness*Time.deltaTime);
    }
}
