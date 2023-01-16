using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    Grid grid;

    public Node start;
    public Node end;

    void Start()
    {
        grid = GetComponent<Grid>();
        start = grid.GetClosestNode(Vector2.zero);
        end = grid.GetClosestNode(Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            // create ray from the camera and passing through the touch position:
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Plane plane = new Plane(Vector3.forward, transform.position);
            float distance = 0; // t$$anonymous$$s will return the distance from the camera
            if (plane.Raycast(ray, out distance))
            { // if plane $$anonymous$$t...
                Vector3 pos = ray.GetPoint(distance); // get the point
                                                      // pos has the position in the plane you've touched
                Node newEnd = grid.GetClosestNode(pos);

                if (newEnd != end)
                {
                    end = newEnd;
                    //GetComponent<Pathfinding>().FindPath(start, end);
                }
            }
        }
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 previousTouch1 = (touch1.position - touch1.deltaPosition);
            Vector2 previousTouch2 = (touch2.position - touch2.deltaPosition);

            float previousMagnitude = (previousTouch1 - previousTouch2).magnitude;
            float currentMagnitude = (touch1.position - touch2.position).magnitude;

            float difference = currentMagnitude - previousMagnitude;
            Debug.Log(difference);

            //grid.ZoomCamera(difference * 0.01f);
        }
    }

    void OnTouch()
    {

    }
}
