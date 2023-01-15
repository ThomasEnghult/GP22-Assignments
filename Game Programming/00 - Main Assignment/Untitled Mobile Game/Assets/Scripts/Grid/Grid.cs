using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node[,] GridNodes;

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

    Node start;
    Node end;

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
        Node newEnd = GetClosestNode(endPos.position);
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Node newEnd = GetClosestNode(mousePos);

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
        GridNodes = new Node[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Node newNode = new Node(new Vector2(x * distanceBetweenNodes, y * distanceBetweenNodes));
                GridNodes[x, y] = newNode;
                //if we're on top of a building
                if(x % 2 == 1 && y % 2 == 1) { continue; }

                //if we're inbetween two intersections and not on leftmost column
                if(x % 2 != 0 && x != 0)
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

    void SetCamera()
    {
        float size = gridSize * distanceBetweenNodes;
        Camera.main.orthographicSize = size + 0.25f;
        Camera.main.transform.position = new Vector3(size, size, -size*2);
    }

    void ConnectNodeLeftRight(Node node, int x, int y)
    {
        Node other = GridNodes[x - 1, y];
        node.SetNeighbour(directions.left, other);
        other.SetNeighbour(directions.right, node);
    }

    void ConnectNodeUpDown(Node node, int x, int y)
    {
        Node other = GridNodes[x, y - 1];
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
        foreach (Node node in GridNodes)
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

    void CreateLineRoad(directions direction)
    {

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
        foreach(Node node in GridNodes)
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

        Debug.Log(x + " " + y);

        if(GridNodes[x, y].neighbours.Length == 0)
        {
            //Distance to an existing node
            float xDistance = 0.5f - position.x % 1;
            float yDistance = 0.5f - position.y % 1;

            if(Mathf.Abs(xDistance) < Mathf.Abs(yDistance))
            {
                if (xDistance < 0)
                    return GridNodes[x - 1, y];
                else
                    return GridNodes[x + 1, y];
            }
            else
            {
                if (yDistance < 0)
                    return GridNodes[x, y - 1];
                else
                    return GridNodes[x, y + 1];
            }
        }

        return GridNodes[x, y];
    }

    private void OnDrawGizmos()
    {
        if(GridNodes == null) { return; }

        float size = 0.05f * distanceBetweenNodes;

        foreach (var node in GridNodes)
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
                Vector2 vOffset = new Vector2(0, offset);
                if (i < 2)
                    vOffset = new Vector2(offset, 0);

                Gizmos.DrawLine(neighbour.position + vOffset, node.position + vOffset);
            }
        }

        if(path != null)
        {
            if(path.Count != 0)
            {
                Gizmos.color = Color.red;
                Debug.Log("Drawing Path");
                Gizmos.DrawLine(GetClosestNode(startPos.position).position, path[0].position);
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                
                Gizmos.color = Color.green;
                Gizmos.DrawLine(path[i].position, path[i + 1].position);
            }
        }
    }
}
