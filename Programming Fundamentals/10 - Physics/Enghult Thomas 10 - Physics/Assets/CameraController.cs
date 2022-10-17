using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject camera0, camera1, camera2;
    Transform start0, start1, start2;
    GameObject[] cameras;
    Transform[] starts;
    float maxRight, middle, maxLeft;

    // Start is called before the first frame update
    void Start()
    {
        maxRight = GameObject.Find("background_right").transform.position.x;
        middle = GameObject.Find("background_middle").transform.position.x;
        maxLeft = GameObject.Find("background_left").transform.position.x;


        camera0 = GameObject.Find("Background Camera 0");
        camera1 = GameObject.Find("Background Camera 1");
        camera2 = GameObject.Find("Background Camera 2");
        cameras = new GameObject[]{ camera0, camera1, camera2 };
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * 2;
        Vector3 movement = new(x, 0);
        for (int i = 0; i < 3; i++)
        {
            moveCamera(cameras[i], movement*(i+1));
            CameraOutOfBounds(cameras[i]);
        }
    }

    void moveCamera(GameObject camera, Vector3 move)
    {
        camera.transform.position += move * Time.deltaTime;
    }

    void CameraOutOfBounds(GameObject camera)
    {
        float x = middle;
        float y = camera.transform.position.y;
        float z = camera.transform.position.z;
        Vector3 startPos = new Vector3(x, y, z);

        if (camera.transform.position.x > maxRight)
        {
            Debug.Log("OutOfBounds " + camera.name);
            //camera.transform.position = startPos;
            camera.transform.position -= camera.transform.position - startPos;
        }

        if (camera.transform.transform.position.x < maxLeft)
        {
            Debug.Log("OutOfBounds " + camera.name);
            //camera.transform.position = startPos;
            camera.transform.position -= camera.transform.position - startPos;
        }
    }
}
