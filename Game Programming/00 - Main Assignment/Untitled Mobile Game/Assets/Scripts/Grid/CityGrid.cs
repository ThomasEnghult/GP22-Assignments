using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CityGrid : MonoBehaviour
{
    public static CityGrid Instance;
    public Node[,] gridNodes;

    public int gridSize = 5;
    private int currentSize;
    int gridSizeX = 5;
    int gridSizeZ = 5;

    public float distanceBetweenNodes = 8.5f;

    public List<Node> path = new List<Node>();

    public Transform startPos;
    public Transform endPos;

    public GameObject cityBlock;
    public GameObject roadLine;
    public GameObject roadL;
    public GameObject roadT;
    public GameObject roadIntersection;

    private GameObject roadHolder;

    Vector3 roadScale = Vector3.one;

    public Vector3 mousePos;

    Pathfinding pathfinder;

    public Node start;
    public Node end;

    public GameObject selectedCar;
    CarController carController;

    void Start()
    {
        pathfinder = GetComponent<Pathfinding>();
        carController = selectedCar.GetComponent<CarController>();

        Instance = this;
        gridSizeX = gridSize * 2 + 1;
        gridSizeZ = gridSize * 2 + 1;
        //GenerateGrid();
        //GenerateStructures();
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //end = GetClosestNode(mousePos);
    }

    void Update()
    {
        if (gridSize != currentSize)
        {
            currentSize = gridSize;
            gridSizeX = gridSize * 2 + 1;
            gridSizeZ = gridSize * 2 + 1;
            GenerateGrid();
            GenerateStructures();
            start = GetClosestNode(startPos.position);
            end = GetClosestNode(endPos.position);
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPos = selectedCar.transform;

            if(carController.moveTo != null)
            {
                start = carController.moveTo;
            }
            else
            {
                start = carController.moveFrom;
            }

            //start = GetClosestNode(startPos.position);

            path = GetPath(start, end);
        }
    }

    public List<Node> GetPath(Node start, Node end)
    {
        List<Node> newPath = pathfinder.FindPath(start, end);
        foreach(Node node in newPath)
        {
            Debug.Log(node.position);
        }

        if (newPath != null)
        {
            selectedCar.GetComponent<CarController>().path = newPath;
            return newPath;
        }
        else
        {
            return path;
        }

    }

    public void UpdateTouchPosition(Vector3 touchPosition)
    {
        Node newEnd = GetClosestNode(touchPosition);

        if (newEnd != end)
        {
            end = newEnd;
            path = GetPath(start, end);
        }
    }

    public void GenerateGrid()
    {
        Camera.main.GetComponent<CameraController>().SetCamera(gridSize * distanceBetweenNodes);
        gridNodes = new Node[gridSizeX, gridSizeZ];

        for (int z = 0; z < gridSizeZ; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Node newNode = new Node(new Vector3(x * distanceBetweenNodes, 0, z * distanceBetweenNodes));
                gridNodes[x, z] = newNode;

                if (Random.Range(0f, 1f) > 0.9f)
                    newNode.isOpen = false;
                //if we're on top of a building
                if(x % 2 == 1 && z % 2 == 1) 
                {
                    gridNodes[x, z].snapToGrid = false; 
                }
                //if we're inbetween two intersections and not on leftmost column
                else if(x % 2 != 0 && x != 0)
                {
                    ConnectNodeLeftRight(newNode, x, z);
                }
                //if we're inbetween two intersections and not on bottom row
                else if(z % 2 != 0 && z != 0)
                {
                    ConnectNodeUpDown(newNode, x, z);
                }
                else
                {
                    ConnectCorner(newNode, x, z);
                }
            }
        }
    }



    void ConnectNodeLeftRight(Node node, int x, int y)
    {
        Node other = gridNodes[x - 1, y];
        node.SetNeighbour(Directions.left, other);
        other.SetNeighbour(Directions.right, node);
    }

    void ConnectNodeUpDown(Node node, int x, int y)
    {
        Node other = gridNodes[x, y - 1];
        node.SetNeighbour(Directions.down, other);
        other.SetNeighbour(Directions.up, node);
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
        if(roadHolder != null)
        {
            DestroyImmediate(roadHolder);
        }
        roadHolder = new GameObject("RoadHolder");
        foreach (Node node in gridNodes)
        {
            GenerateStructure(node);
        }
    }

    void GenerateStructure(Node node)
    {
        

        int numOfNeighbours = 0;
        List<Directions> closedDirections = new List<Directions>();
        bool[] OpenDirections = { true, true, true, true };
        for (Directions direction = 0; (int)direction < node.neighbours.Length; direction++)
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
                int rotation = Random.Range(0, 5) * 90;
                CreateStructure(node, cityBlock, Quaternion.Euler(0, rotation, 0));

                break;
            case 2:
                //Insert Line Road or L road
                if (OpenDirections[(int)Directions.up] && OpenDirections[(int)Directions.down])
                    CreateStructure(node, roadLine, Quaternion.identity);

                else if (OpenDirections[(int)Directions.left] && OpenDirections[(int)Directions.right])
                    CreateStructure(node, roadLine, Quaternion.Euler(0, 90, 0));

                else
                    CreateCornerRoad(node, OpenDirections);

                break;
            case 3:
                //Insert T Road
                int[] rotations = { -90, 90, 180, 0 };
                int setRotation = rotations[(int)closedDirections[0]];
                CreateStructure(node, roadT, Quaternion.Euler(0, setRotation, 0));
                break;
            case 4:
                //Insert Intersection
                CreateStructure(node, roadIntersection, Quaternion.Euler(0, 0, 0));
                break;

            default:
                Debug.Log("No structure was found for " + numOfNeighbours.ToString() + " neighbours");
                break;
        }
    }

    void CreateStructure(Node node, GameObject road, Quaternion rotation)
    {
        GameObject newRoad = Instantiate(road, node.position, rotation);
        newRoad.transform.parent = roadHolder.transform;
        newRoad.transform.localScale = roadScale;
        node.structure = newRoad;
    }

    void CreateCornerRoad(Node node,bool[] openDirections)
    {
        if (openDirections[(int)Directions.up])
        {
            if (openDirections[(int)Directions.left])
                CreateStructure(node, roadL, Quaternion.Euler(0, 0, 0));

            else if (openDirections[(int)Directions.right])
                CreateStructure(node, roadL, Quaternion.Euler(0, 90, 0));
        }
        else if (openDirections[(int)Directions.down])
        {
            if (openDirections[(int)Directions.left])
                CreateStructure(node, roadL, Quaternion.Euler(0, -90, 0));

            if (openDirections[(int)Directions.right])
                CreateStructure(node, roadL, Quaternion.Euler(0, 180, 0));
        }
    }

    public Node GetClosestNode(Vector3 position)
    {
        position /= distanceBetweenNodes;
        int x = Mathf.RoundToInt(position.x);
        x = Mathf.Clamp(x, 0, gridSizeX - 1);

        int z = Mathf.RoundToInt(position.z);
        z = Mathf.Clamp(z, 0, gridSizeZ - 1);

        if(!gridNodes[x, z].snapToGrid)
        {
            Debug.Log(x + " " + z);
            //Distance to an existing node
            float xDistance = 0.5f - position.x % 1;
            float zDistance = 0.5f - position.z % 1;

            if(Mathf.Abs(xDistance) < Mathf.Abs(zDistance))
            {
                if (xDistance < 0)
                    return gridNodes[x - 1, z];
                else
                    return gridNodes[x + 1, z];
            }
            else
            {
                if (zDistance < 0)
                    return gridNodes[x, z - 1];
                else
                    return gridNodes[x, z + 1];
            }
        }

        return gridNodes[x, z];
    }

    private void OnDrawGizmos()
    {
        if(gridNodes == null) { return; }

        float size = 0.05f * distanceBetweenNodes;

        foreach (var node in gridNodes)
        {
            if(node == null) { continue; }

            if(node.isOpen)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector3)node.position, size);

            if(node == end)
            {
                Handles.Label((Vector3)node.position + Vector3.up, "   Selected");
            }
            else
                Handles.Label((Vector3)node.position, "   " + node.fCost.ToString());

            for (int i = 0; i < node.neighbours.Length; i ++)
            {
                Node neighbour = node.neighbours[i];
                if(neighbour == null) { continue; }
                Gizmos.color = new Color(i/2f % 1, 0, 1);
                //float offset = size - (size*2) * (i % 2);
                float offset = 0.5f - 1 * (i % 2);
                Vector3 vOffset = new Vector3(0, 0.5f, offset);
                if (i < 2)
                    vOffset = new Vector3(offset, 0.5f, 0);

                Gizmos.DrawLine((Vector3)neighbour.position + vOffset, (Vector3)node.position + vOffset);
            }
        }

        if(path != null)
        {
            Vector3 vOffset = new Vector3(0,0.5f, 0);
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
