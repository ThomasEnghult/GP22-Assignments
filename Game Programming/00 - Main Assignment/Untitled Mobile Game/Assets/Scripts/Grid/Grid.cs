using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node[,] gridNodes;

    public int gridSize = 5;
    private int currentSize = 5;
    int gridSizeX = 5;
    int gridSizeY = 5;

    public float distanceBetweenNodes = 8.5f;

    public List<Node> path;

    public Transform startPos;
    public Transform endPos;

    public GameObject roadLine;
    public GameObject roadL;
    public GameObject roadT;
    public GameObject roadIntersection;

    private GameObject roadHolder;

    Vector3 roadScale = Vector3.one;

    public Vector3 mousePos;

    public Node start;
    public Node end;

    void Start()
    {
        gridSizeX = gridSize * 2 + 1;
        gridSizeY = gridSize * 2 + 1;
        GenerateGrid();
        GenerateStructures();
        start = GetClosestNode(startPos.position);
        end = GetClosestNode(endPos.position);
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //end = GetClosestNode(mousePos);
    }

    void Update()
    {
        if(gridSize != currentSize)
        {
            currentSize = gridSize;
            gridSizeX = gridSize * 2 + 1;
            gridSizeY = gridSize * 2 + 1;
            GenerateGrid();
            GenerateStructures();
        }
        
        Node newStart = GetClosestNode(startPos.position);
        Node newEnd = end;

        if(Input.touchCount == 1)
        {
            // create ray from the camera and passing through the touch position:
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Plane plane = new Plane(Vector3.forward, transform.position);
            float distance = 0; // t$$anonymous$$s will return the distance from the camera
            if (plane.Raycast(ray, out distance))
            { // if plane $$anonymous$$t...
                Vector3 pos = ray.GetPoint(distance); // get the point
                                                      // pos has the position in the plane you've touched
                newEnd = GetClosestNode(pos);
            }
        }
        if(Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 previousTouch1 = (touch1.position - touch1.deltaPosition);
            Vector2 previousTouch2 = (touch2.position - touch2.deltaPosition);

            float previousMagnitude = (previousTouch1 - previousTouch2).magnitude;
            float currentMagnitude = (touch1.position - touch2.position).magnitude;

            float difference = currentMagnitude - previousMagnitude;
            Debug.Log(difference);

            ZoomCamera(difference * 0.01f);
        }

        if (newStart != start || newEnd != end)
        {
            start = newStart;
            end = newEnd;
            GetComponent<Pathfinding>().FindPath(start, end);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Pathfinding>().FindPath(start, end);
        }
    }

    public void GenerateGrid()
    {
        SetCamera();
        gridNodes = new Node[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Node newNode = new Node(new Vector2(x * distanceBetweenNodes, y * distanceBetweenNodes));
                gridNodes[x, y] = newNode;
                //if we're on top of a building
                if(x % 2 == 1 && y % 2 == 1) 
                {
                    gridNodes[x, y].snapToGrid = false; 
                }
                //if we're inbetween two intersections and not on leftmost column
                else if(x % 2 != 0 && x != 0)
                {
                    ConnectNodeLeftRight(newNode, x, y);
                }
                //if we're inbetween two intersections and not on bottom row
                else if(y % 2 != 0 && y != 0)
                {
                    ConnectNodeUpDown(newNode, x, y);
                }
                else
                {
                    ConnectCorner(newNode, x, y);
                }
            }
        }
    }

    public void SetCamera()
    {
        float size = gridSize * distanceBetweenNodes;
        //Camera.main.orthographicSize = size + 0.25f;
        Camera.main.transform.position = new Vector3(size, size, -size*4);
    }

    public void MoveCamera()
    {

    }

    public void ZoomCamera(float increment)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        float size = gridSize * distanceBetweenNodes;
        float minZoom = -size;
        float maxZoom = -size * 4;

        float distance = cameraPosition.z;

        float zoom = Mathf.Clamp(distance - increment, maxZoom, minZoom);
        cameraPosition.z = zoom;
        Camera.main.transform.position = cameraPosition;
    }

    void ConnectNodeLeftRight(Node node, int x, int y)
    {
        Node other = gridNodes[x - 1, y];
        node.SetNeighbour(directions.left, other);
        other.SetNeighbour(directions.right, node);
    }

    void ConnectNodeUpDown(Node node, int x, int y)
    {
        Node other = gridNodes[x, y - 1];
        node.SetNeighbour(directions.down, other);
        other.SetNeighbour(directions.up, node);
    }
    void ConnectCorner(Node node, int x, int y)
    {
        if(y - 1 >= 0) //If not on bottom row
        {
            ConnectNodeUpDown(node, x, y);
        }
        if (x - 1 >= 0) //If not on leftmost column
        {
            ConnectNodeLeftRight(node, x, y);
        }
    }

    void GenerateStructures()
    {
        roadHolder = new GameObject("RoadHolder");
        foreach (Node node in gridNodes)
        {
            GenerateStructure(node);
        }
    }


    void GenerateStructure(Node node)
    {
        DestroyAllStructures();

        int numOfNeighbours = 0;
        List<directions> closedDirections = new List<directions>();
        bool[] OpenDirections = { true, true, true, true };
        for (directions direction = 0; (int)direction < node.neighbours.Length; direction++)
        {
            if (node.neighbours[(int)direction] != null)
            {
                numOfNeighbours++;
            }
            else
            {
                OpenDirections[(int)direction] = false;
                closedDirections.Add(direction);
            }
        }

        switch (numOfNeighbours)
        {
            case 0:
                //Insert City Block
                break;
            case 2:
                //Insert Line Road or L road
                if (OpenDirections[(int)directions.up] && OpenDirections[(int)directions.down])
                    CreateRoad(node, roadLine, Quaternion.Euler(90, -90, 90));
                else if (OpenDirections[(int)directions.left] && OpenDirections[(int)directions.right])
                    CreateRoad(node, roadLine, Quaternion.Euler(0, -90, 90));
                else
                    CreateCornerRoad(node, OpenDirections);

                break;
            case 3:
                //Insert T Road
                int[] rotations = { 0, 180, 90, -90 };
                int setRotation = rotations[(int)closedDirections[0]];
                CreateRoad(node, roadT, Quaternion.Euler(setRotation, -90, 90));
                break;
            case 4:
                //Insert Intersection
                CreateRoad(node, roadIntersection, Quaternion.Euler(90, -90, 90));
                break;

            default:
                Debug.Log("No structure was found for " + numOfNeighbours.ToString() + " neighbours");
                break;
        }
    }

    void CreateRoad(Node node, GameObject road, Quaternion rotation)
    {
        GameObject newRoad = Instantiate(road, node.position, rotation);
        newRoad.transform.parent = roadHolder.transform;
        newRoad.transform.localScale = roadScale;
        node.gameObject = newRoad;
    }

    void CreateCornerRoad(Node node,bool[] openDirections)
    {
        if (openDirections[(int)directions.up])
        {
            if (openDirections[(int)directions.left])
                CreateRoad(node, roadL, Quaternion.Euler(-90, -90, 90));

            else if (openDirections[(int)directions.right])
                CreateRoad(node, roadL, Quaternion.Euler(180, -90, 90));
        }
        else if (openDirections[(int)directions.down])
        {
            if (openDirections[(int)directions.left])
                CreateRoad(node, roadL, Quaternion.Euler(0, -90, 90));

            if (openDirections[(int)directions.right])
                CreateRoad(node, roadL, Quaternion.Euler(90, -90, 90));
        }
    }

    void DestroyAllStructures()
    {
        foreach(Node node in gridNodes)
        {
            //DestroyImmediate(node.gameObject);
        }
    }

    public Node GetClosestNode(Vector2 position)
    {
        position /= distanceBetweenNodes;
        int x = Mathf.RoundToInt(position.x);
        x = Mathf.Clamp(x, 0, gridSizeX - 1);

        int y = Mathf.RoundToInt(position.y);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        

        if(!gridNodes[x, y].snapToGrid)
        {
            Debug.Log(x + " " + y);
            //Distance to an existing node
            float xDistance = 0.5f - position.x % 1;
            float yDistance = 0.5f - position.y % 1;

            if(Mathf.Abs(xDistance) < Mathf.Abs(yDistance))
            {
                if (xDistance < 0)
                    return gridNodes[x - 1, y];
                else
                    return gridNodes[x + 1, y];
            }
            else
            {
                if (yDistance < 0)
                    return gridNodes[x, y - 1];
                else
                    return gridNodes[x, y + 1];
            }
        }

        return gridNodes[x, y];
    }

    private void OnDrawGizmos()
    {
        if(gridNodes == null) { return; }

        float size = 0.05f * distanceBetweenNodes;

        foreach (var node in gridNodes)
        {
            if(node == null) { continue; }
            Gizmos.color = new Color(1, 1, 1);
            Gizmos.DrawSphere((Vector3)node.position, size);

            for (int i = 0; i < node.neighbours.Length; i ++)
            {
                Node neighbour = node.neighbours[i];
                if(neighbour == null) { continue; }
                Gizmos.color = new Color(i/2f % 1, 0, 1);
                float offset = size - (size*2) * (i % 2);
                Vector3 vOffset = new Vector3(0, offset, -0.5f);
                if (i < 2)
                    vOffset = new Vector3(offset, 0, -0.5f);

                Gizmos.DrawLine((Vector3)neighbour.position + vOffset, (Vector3)node.position + vOffset);
            }
        }

        if(path != null)
        {
            Vector3 vOffset = new Vector3(0, 0, -0.5f);
            if (path.Count != 0)
            {
                Gizmos.color = Color.red;
                //Debug.Log("Drawing Path");
                Gizmos.DrawLine((Vector3)GetClosestNode(startPos.position).position + vOffset, (Vector3)path[0].position + vOffset);
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                
                Gizmos.color = Color.green;
                Gizmos.DrawLine((Vector3)path[i].position + vOffset, (Vector3)path[i + 1].position + vOffset);
            }
        }
    }
}
