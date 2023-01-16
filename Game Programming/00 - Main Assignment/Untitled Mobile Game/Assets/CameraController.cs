using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    float size;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCamera(float size)
    {
        this.size = size;
        //Camera.main.orthographicSize = size + 0.25f;
        Camera.main.transform.position = new Vector3(size, size, -size * 4);
    }

    public void MoveCamera(Vector3 direction)
    {
        Vector3 newCameraPosition = Camera.main.transform.position - direction;
        Debug.Log("before:" + newCameraPosition);
        newCameraPosition.x = Mathf.Clamp(newCameraPosition.x ,0, size * 2);
        newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, 0, size * 2);
        Debug.Log("after" + newCameraPosition);
        newCameraPosition.z = Camera.main.transform.position.z;
        Camera.main.transform.position = newCameraPosition;
    }

    public void ZoomCamera(float increment)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float minZoom = -size;
        float maxZoom = -size * 4;

        float distance = cameraPosition.z;

        float zoom = Mathf.Clamp(distance - increment, maxZoom, minZoom);
        cameraPosition.z = zoom;
        Camera.main.transform.position = cameraPosition;
    }
}
