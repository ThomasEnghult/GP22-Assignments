using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInteraction : MonoBehaviour
{
    public Node parent;
    public GameObject nodeIcon;


    public NodeInteraction(Node parent, GameObject nodeIcon)
    {
        this.parent = parent;
        this.nodeIcon = Instantiate(nodeIcon);
        nodeIcon.transform.position = new Vector3(parent.position.x, 5.2f, parent.position.y);
    }

    void Start()
    {

    }

    public void TriggerInteraction()
    {
        Debug.Log("Triggered Interaction!");
    }

    public void OnClick()
    {

    }
}
