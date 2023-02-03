using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    float size;

    public void SetCamera(float size)
    {
        this.size = size;
        //Camera.main.orthographicSize = size + 0.25f;
        Camera.main.transform.position = new Vector3(size, size * 4, size);
    }

    public void MoveCamera(Vector3 direction)
    {
        Vector3 newCameraPosition = Camera.main.transform.position - direction;
        Debug.Log("direction;" + direction);
        Debug.Log("before:" + transform.position);
        newCameraPosition.x = Mathf.Clamp(newCameraPosition.x ,0, size * 2);
        newCameraPosition.z = Mathf.Clamp(newCameraPosition.z, 0, size * 2);
        Debug.Log("after" + newCameraPosition);
        newCameraPosition.y = Camera.main.transform.position.y;
        Camera.main.transform.position = newCameraPosition;
    }

    public void ZoomCamera(float increment)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float minZoom = size;
        float maxZoom = size * 4;

        float distance = cameraPosition.y;

        float zoom = Mathf.Clamp(distance - increment, minZoom, maxZoom);
        cameraPosition.y = zoom;
        Camera.main.transform.position = cameraPosition;
    }
}
