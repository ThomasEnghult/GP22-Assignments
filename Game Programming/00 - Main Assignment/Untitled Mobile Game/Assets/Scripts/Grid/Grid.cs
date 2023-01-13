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

    public List<Node> path;

    public Transform startPos;
    public Transform endPos;

    public Vector3 mousePos;

    Node start;
    Node end;

    void Start()
    {
        GenerateGrid();
        start = GetClosestNode(startPos.position);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        end = GetClosestNode(mousePos);
    }

    void Update()
    {
        if(gridSize != currentSize)
        {
            currentSize = gridSize;
            gridSizeX = gridSize * 2 + 1;
            gridSizeY = gridSize * 2 + 1;
            GenerateGrid();
        }
        
        Node newStart = GetClosestNode(startPos.position);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Node newEnd = GetClosestNode(mousePos);

        if(newStart != start || newEnd != end)
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
                //if we're on top of a building
                if(x % 2 == 1 && y % 2 == 1) { continue; }

                Node newNode = new Node(new Vector2(x, y));
                GridNodes[x, y] = newNode;
                
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
        float size = (gridSizeY - 1) / 2f;
        Camera.main.orthographicSize = size + 0.25f;
        Camera.main.transform.position = new Vector3(size, size, -10);
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

    public Node GetClosestNode(Vector2 position)
    {
        int x = Mathf.RoundToInt(position.x);
        x = Mathf.Clamp(x, 0, gridSizeX - 1);

        int y = Mathf.RoundToInt(position.y);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        if(GridNodes[x, y] == null)
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

        float size = 0.05f;

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
