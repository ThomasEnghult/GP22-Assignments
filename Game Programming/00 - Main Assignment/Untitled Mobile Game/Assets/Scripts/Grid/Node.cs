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
    public GameObject gameObject;

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
}
