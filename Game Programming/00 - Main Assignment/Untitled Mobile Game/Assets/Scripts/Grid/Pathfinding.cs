using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Node[,] GridNodes;

    private void Awake()
    {
        GridNodes = GetComponent<CityGrid>().gridNodes;
    }

    public List<Node> FindPath(Node start, Node end)
    {
        Debug.Log("Finding Path from:" + start.position + " to " + end.position);

        if (!end.isOpen)
        {
            Debug.Log("End node is closed!");
            return null;
        }
        //else
        //{
        //    foreach(Node neighbour in end.neighbours)
        //    {

        //    }
        //}

        Node inputEnd = end;
        bool endIsIntersection = !(end.GetNeighbour(Directions.up) == null && end.GetNeighbour(Directions.down) == null ||
                                end.GetNeighbour(Directions.left) == null && end.GetNeighbour(Directions.right) == null);

        if (!endIsIntersection)
        {
            end = AlignToManhattanGrid(start, end);
        }

        GridNodes = GetComponent<CityGrid>().gridNodes;
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
                if(currentNode.fCost > nextNode.fCost || (currentNode.fCost == nextNode.fCost && currentNode.hCost > nextNode.hCost))
                {
                    currentNode = nextNode;
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //Debug.Log("Checking: " + currentNode.position + " - End node:" + end.position);

            if(currentNode == end)
            {
                return RetracePath(start, inputEnd);
            }

            //Node[] neighbours = ShuffleNeighbours(currentNode.neighbours);

            foreach(Node neighbour in currentNode.ShuffleNeighbours())
            {
                if (neighbour == null || !neighbour.isOpen) { continue; }
                if (closedSet.Contains(neighbour)) { continue; }

                int movementCostToNeighbour = currentNode.gCost + HeuristicCost(currentNode, neighbour);
                
                if(movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.hCost = HeuristicCost(neighbour, end);
                    neighbour.fCost = neighbour.gCost + neighbour.hCost;
                    neighbour.parent = currentNode;
                    //Debug.Log("Setting " + neighbour.position + " parent to " + currentNode.position);

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        Debug.Log("No path was found!");
        return null;
    }

    List<Node> RetracePath(Node start, Node end)
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
        path.Add(currentNode);
        path.Reverse();
        Debug.Log("Retraced Path");
        return path;
        //GetComponent<CityGrid>().path = path;
    }

    Node AlignToManhattanGrid(Node start, Node end)
    {
        Node closest = end;
        Node previous = end;
        int currentCost = HeuristicCost(start, end);
        foreach(Node neighbour in end.neighbours)
        {
            if(neighbour == null || !neighbour.isOpen) { continue; }

            if (closest == end)
                closest = neighbour;

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
        int deltaZ = (int)Mathf.Abs(start.position.z - end.position.z);
        return deltaX + deltaZ;
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
