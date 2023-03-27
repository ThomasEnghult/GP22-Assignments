using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CarStatus
{
    Driving,
    Idle,
    Turning
}

public class CarController : MonoBehaviour
{
    CityGrid cityGrid;
    public List<Node> path;

    public Node moveFrom;
    public Node moveTo;

    Vector3 move;
    float counter = 0;

    CarStatus status = CarStatus.Driving;

    bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Init), 0.05f);
    }

    void Init()
    {
        cityGrid = GameObject.Find("Grid").GetComponent<CityGrid>();
        path = cityGrid.path;
        moveFrom = cityGrid.GetClosestNode(transform.position);
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) { return; }

        counter += Time.deltaTime;

        if (moveTo != null)
        {
            MoveToNode();
        }

        if (counter > 1)
        {
            counter = 0;

            if (path.Count != 0)
                GetNewDestination();
        }
    }

    void GetNewDestination()
    {
        //Node newDirection = moveTo.GetRandomDirection(moveFrom);

        //Pop first element from list, current position
        moveFrom = path[0];
        path.RemoveAt(0);

        Debug.Log("Popped node at: " + moveFrom.position);

        transform.position = moveFrom.position;
        //If we have a path to go to
        if (path.Count != 0)
        {
            moveTo = path[0];

            if (!moveTo.isOpen)
            {
                path = cityGrid.GetPath(moveFrom, path[path.Count - 1]);
                GetNewDestination();
                return;
            }

            move = moveTo.position - moveFrom.position;
            Debug.Log("Moving towards: " + moveTo.position);

            transform.forward = move;
        }
        else
            moveTo = null;
    }

    void MoveToNode()
    {
        transform.position += move * Time.deltaTime;
    }
}
