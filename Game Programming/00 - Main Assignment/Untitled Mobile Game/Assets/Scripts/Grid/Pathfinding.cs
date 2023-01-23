using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Node[,] GridNodes;

    private void Awake()
    {
        GridNodes = GetComponent<Grid>().gridNodes;
    }

    public void FindPath(Node start, Node end)
    {
        Debug.Log("Finding Path from:" + start.position + " to " + end.position);
        Node inputEnd = end;
        bool endIsIntersection = !(end.GetNeighbour(directions.up) == null && end.GetNeighbour(directions.down) == null ||
                                end.GetNeighbour(directions.left) == null && end.GetNeighbour(directions.right) == null);

        if (!endIsIntersection)
        {
            end = AlignToManhattanGrid(start, end);
        }

        GridNodes = GetComponent<Grid>().gridNodes;
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(start);

        while(openSet.Count != 0)
        {
            //Debug.Log(openSet.Count);
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                Node nextNode = openSet[i];
                if(currentNode.fCost > nextNode.fCost || currentNode.fCost == nextNode.fCost && currentNode.hCost > nextNode.hCost)
                {
                    currentNode = nextNode;
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            Debug.Log("Checking: " + currentNode.position + " - End node:" + end.position);

            if(currentNode == end)
            {
                RetracePath(start, inputEnd);
                return;
            }

            //Node[] neighbours = ShuffleNeighbours(currentNode.neighbours);

            foreach(Node neighbour in currentNode.ShuffleNeighbours())
            {
                if (neighbour == null) { continue; }
                if (closedSet.Contains(neighbour)) { continue; }

                int movementCostToNeighbour = currentNode.gCost + HeuristicCost(currentNode, neighbour);
                
                if(movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.fCost = HeuristicCost(neighbour, end);
                    neighbour.parent = currentNode;
                    //Debug.Log("Setting " + neighbour.position + " parent to " + currentNode.position);

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        //Debug.Log(start.position + "  before:" + currentNode.position);

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;

            //Debug.Log(start.position + "  parent:" + currentNode.position);
        }
        path.Reverse();
        Debug.Log("Retraced Path");
        GetComponent<Grid>().path = path;
    }

    Node AlignToManhattanGrid(Node start, Node end)
    {
        Node closest = end;
        Node previous = end;
        int currentCost = HeuristicCost(start, end);
        foreach(Node neighbour in end.neighbours)
        {
            if(neighbour == null) { continue; }
            int cost = HeuristicCost(start, neighbour);
            if(cost < currentCost)
            {
                closest = neighbour;
                break;
            }
            if(cost == currentCost)
            {
                if(Random.Range(0,2) == 0)
                {
                    closest = neighbour;
                }
                else
                {
                    closest = previous;
                }
            }
            else
            {
                previous = neighbour;
                currentCost = cost;
            }
        }
        end.parent = closest;
        return closest;
    }

    int HeuristicCost(Node start, Node end)
    {
        int deltaX = (int)Mathf.Abs(start.position.x - end.position.x);
        int deltaY = (int)Mathf.Abs(start.position.y - end.position.y);
        return deltaX + deltaY;
    }

    public Node[] ShuffleNeighbours(Node[] neighbours)
    {
        Node[] copy = (Node[])neighbours.Clone();
        for (int i = 0; i < copy.Length - 1; i++)
        {
            int rnd = Random.Range(i, copy.Length);
            var tempGO = copy[rnd];
            copy[rnd] = copy[i];
            copy[i] = tempGO;
        }
        return copy;
    }
}
