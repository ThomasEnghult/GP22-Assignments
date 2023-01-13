using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Node[,] GridNodes;

    private void Awake()
    {
        GridNodes = GetComponent<Grid>().GridNodes;
    }

    public void FindPath(Node start, Node end)
    {
        GridNodes = GetComponent<Grid>().GridNodes;
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(start);

        while(openSet.Count != 0)
        {
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

            if(currentNode == end)
            {
                RetracePath(start, end);
                return;
            }

            //for (int i = 0; i < currentNode.neighbours.Length; i++)
            //{
            //    Node neighbour = currentNode.neighbours[i];

            Node[] neighbours = ShuffleNeighbours(currentNode.neighbours);

            foreach(Node neighbour in neighbours)
            {
                if (neighbour == null) { continue; }
                if (closedSet.Contains(neighbour)) { continue; }

                int movementCostToNeighbour = currentNode.gCost + HeuristicCost(currentNode, neighbour);
                
                if(movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.fCost = HeuristicCost(neighbour, end);
                    neighbour.parent = currentNode;

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

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent; 
        }
        path.Reverse();

        GetComponent<Grid>().path = path;
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
