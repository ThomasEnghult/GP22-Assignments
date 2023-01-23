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
    Node moveFrom;
    Node moveTo;

    Vector2 move;
    float counter = 0;

    CarStatus status = CarStatus.Driving;
    directions previousDirection = directions.up;
    directions currentDirection;
    float[] rotations = { 270, 90, 180, 360 };

    bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Init), 0.5f);
    }

    void Init()
    {
        moveFrom = Grid.Instance.GetClosestNode(transform.position);
        moveTo = moveFrom.GetRandomDirection(moveFrom);
        move = moveTo.position - moveFrom.position;
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) { return; }

        counter += Time.deltaTime;

        if(counter > 1)
        {
            counter = 0;
            transform.position = moveTo.position;


            if(status == CarStatus.Turning)
            {
                status = CarStatus.Driving;
            }
            else
                status++;

            if (status == CarStatus.Turning)
                GetNewDestination();

        }
        switch (status)
        {
            case CarStatus.Driving:
            {
                MoveToNode();
                break;
            }
            case CarStatus.Turning:
            {
                TurnToDirection(previousDirection, currentDirection, counter);
                break;
            }
        }
    }

    void GetNewDestination()
    {
        Node newDirection = moveTo.GetRandomDirection(moveFrom);
        directions travellingToDirection = directions.up;
        for (directions direction = 0; (int)direction < moveTo.neighbours.Length; direction++)
        {
            if(moveTo.neighbours[(int)direction] == null) { continue; }
            if(moveTo.neighbours[(int)direction] == newDirection)
            {
                travellingToDirection = direction;
            }
        }

        moveFrom = moveTo;
        moveTo = newDirection;
        move = moveTo.position - moveFrom.position;

        previousDirection = currentDirection;
        currentDirection = travellingToDirection;

        float rotation = rotations[(int)travellingToDirection];
        transform.eulerAngles = new Vector3(rotation, 90, -90);
    }

    void MoveToNode()
    {
        transform.position += (Vector3)move * Time.deltaTime;
    }

    //float[] rotations = { 270, 90, 180, 360 };
    void TurnToDirection(directions from, directions to, float counter)
    {
        Debug.Log("From:" + from + " To:" + to);

        //UP > Right 270 -> 360
        //UP > Left 270 -> 180

        float angleFrom = -rotations[(int)from] * Mathf.Deg2Rad;
        float angleTo = -rotations[(int)to] * Mathf.Deg2Rad;
        float angle = (angleTo - angleFrom ) * counter;

        Debug.Log(angle);

        Vector3 center = moveFrom.position;
        var offset = new Vector2(Mathf.Sin(angleFrom + angle), Mathf.Cos(angleFrom + angle)) * 1.25f;
        transform.position = center + (Vector3)offset;

    }
}
