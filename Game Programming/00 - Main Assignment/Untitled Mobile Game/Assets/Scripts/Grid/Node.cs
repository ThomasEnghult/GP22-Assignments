using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum directions
{
    up,
    down,
    left,
    right
}

public class Node
{
    public GameObject road;

    public Vector2 position;
    public Node[] neighbours;
    public Node parent;
    public bool snapToGrid = true;

    public int fCost;

    public int gCost;
    public int hCost;

    public Node(Vector2 position)
    {
        neighbours = new Node[4];
        this.position = position;
    }

    public Node GetNeighbour(directions direction)
    {
        return neighbours[(int)direction];
    }

    public void SetNeighbour(directions direction, Node node)
    {
        neighbours[(int)direction] = node;
    }

    public bool Equals(Node other)
    {
        return (this.position.Equals(other.position));
    }

    public Node GetRandomDirection(Node cameFromThisNode)
    {
        foreach(Node node in ShuffleNeighbours())
        {
            if(node != null && node != cameFromThisNode)
            {
                return node;
            }
        }
        Debug.Log("No random direction was found");
        return cameFromThisNode;
    }

    public directions GetRandomDirection(directions cameFromThisDirection)
    {
        List<directions> validDirections = new List<directions>();
        for (directions direction = 0; (int)direction < neighbours.Length; direction++)
        {
            if(neighbours[(int)direction] != null && direction == cameFromThisDirection)
            {
                validDirections.Add(direction);
            }
        }
        int randomDirection = Random.Range(0, validDirections.Count);
        if (validDirections.Count > 0)
            return validDirections[randomDirection];

        Debug.Log("No random direction was found");
        return cameFromThisDirection;

    }

    public Node[] ShuffleNeighbours()
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
