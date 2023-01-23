using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    CityGrid grid;

    int canMove = 0;

    public Node start;
    public Node end;
    CameraController camera;

    Vector3 firstTouchPosition;

    void Start()
    {
        grid = GetComponent<CityGrid>();
        camera = Camera.main.GetComponent<CameraController>();
        //start = grid.GetClosestNode(Vector2.zero);
        //end = grid.GetClosestNode(Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = GetTouchPosition();
        }

        if (Input.touchCount == 1)
        {
            if (canMove == 2)
            {
                Vector3 pos = GetTouchPosition();
                grid.UpdateTouchPosition(pos);
                Debug.Log("Delta distance = " + (pos - firstTouchPosition));
                camera.MoveCamera(pos - firstTouchPosition);
            }
            else if(canMove == 0)
            {
                canMove = 1;
                Invoke(nameof(UnlockMovement), 0.1f);
            }

        }
        if (Input.touchCount == 2)
        {
            canMove = 0;
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 previousTouch1 = (touch1.position - touch1.deltaPosition);
            Vector2 previousTouch2 = (touch2.position - touch2.deltaPosition);

            float previousMagnitude = (previousTouch1 - previousTouch2).magnitude;
            float currentMagnitude = (touch1.position - touch2.position).magnitude;

            float difference = currentMagnitude - previousMagnitude;
            Debug.Log(difference);

            camera.ZoomCamera(difference * 0.01f);
        }
    }

    void UnlockMovement()
    {
        canMove = 2;
        firstTouchPosition = GetTouchPosition();
    }

    Vector3 GetTouchPosition()
    {
        if(Input.touchCount == 0) { return Vector3.zero; }
        // create ray from the camera and passing through the touch position:
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0; // t$$anonymous$$s will return the distance from the camera
        if (plane.Raycast(ray, out distance))
        { // if plane $$anonymous$$t...
            return ray.GetPoint(distance); // get the point
                           // pos has the position in the plane you've touched
        }
        return Vector3.zero;
    }
}
