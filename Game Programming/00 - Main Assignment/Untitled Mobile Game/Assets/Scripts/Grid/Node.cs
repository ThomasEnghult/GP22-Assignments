using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    up,
    down,
    left,
    right
}

public class Node
{
    public NodeInteraction interaction;
    public GameObject structure;

    public Vector3 position;
    public Node[] neighbours;
    public Node parent;
    public bool snapToGrid = true;

    public int fCost;
    public int gCost;
    public int hCost;

    public bool isOpen = true;

    public Node(Vector3 position)
    {
        neighbours = new Node[4];
        this.position = position;
    }

    public Node GetNeighbour(Directions direction)
    {
        return neighbours[(int)direction];
    }

    public void SetNeighbour(Directions direction, Node node)
    {
        neighbours[(int)direction] = node;
    }

    public void Interact()
    {
        Debug.Log("Interacting with node: " + position);
        if(interaction != null)
        {
            interaction.TriggerInteraction();
        }
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

    public Directions GetRandomDirection(Directions cameFromThisDirection)
    {
        List<Directions> validDirections = new List<Directions>();
        for (Directions direction = 0; (int)direction < neighbours.Length; direction++)
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
