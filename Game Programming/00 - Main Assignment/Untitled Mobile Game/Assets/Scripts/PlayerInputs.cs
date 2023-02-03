using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputs : MonoBehaviour
{
    CityGrid grid;

    int canMove = 0;

    Node selected;
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
            if(GetTouchPosition(out firstTouchPosition))
            {
                selected = grid.GetClosestNode(firstTouchPosition);
            }
            else
            {
                selected = grid.GetClosestNode(firstTouchPosition);
                selected.Interact();
                canMove = 1;
            }
        }

        switch (Input.touchCount)
        {
            case 0:
            {
                canMove = 2;
                break;
            }
            case 1:
            {
                if (canMove == 2)
                {
                    if (GetTouchPosition(out Vector3 pos))
                    {
                        grid.UpdateTouchPosition(pos);
                        camera.MoveCamera(pos - firstTouchPosition);
                    }
                }
                else if (canMove == 0)
                {
                    canMove = 1;
                    Invoke(nameof(UnlockMovement), 0.1f);
                }
                break;
            }
            case 2:
            {
                canMove = 0;
                PinchZoom();
                break;
            }
        }
    }

    bool GetTouchPosition(out Vector3 position)
    {
        bool touchedGround = true;
        position = firstTouchPosition;
        if (Input.touchCount == 0) { return false; }
        // create ray from the camera and passing through the touch position:
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        Plane plane = new Plane(Vector3.up, transform.position);


        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            touchedGround = false;
            Debug.Log("Touched the UI");
        }

        if (plane.Raycast(ray, out float distance))
        { // if plane $$anonymous$$t...
            position = ray.GetPoint(distance);
            return touchedGround; // get the point
                                           // pos has the position in the plane you've touched
        }
        return false;
    }

    void UnlockMovement()
    {
        canMove = 2;
        GetTouchPosition(out firstTouchPosition);
    }

    void PinchZoom()
    {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        Vector2 previousTouch1 = (touch1.position - touch1.deltaPosition);
        Vector2 previousTouch2 = (touch2.position - touch2.deltaPosition);

        float previousMagnitude = (previousTouch1 - previousTouch2).magnitude;
        float currentMagnitude = (touch1.position - touch2.position).magnitude;

        float difference = currentMagnitude - previousMagnitude;

        camera.ZoomCamera(difference * 0.1f);
    }
}
