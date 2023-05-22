using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    GameObject holdingItem;
    public bool isHolding = false;

    GameObject closestItem;
    private float closestDistance = -1;
    [SerializeField] private Transform itemRoot;

    private void Awake()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReleaseItem()
    {
        Debug.Log("Tried to release item");
    }

    public bool GrabItem()
    {
        if(closestItem == null)
        {
            Debug.Log("No item nearby to grab");
            return false;
        }

        holdingItem = closestItem;
        closestItem.transform.parent = itemRoot;
        closestItem.transform.localPosition = Vector3.zero;
        //closestItem.transform.localRotation = Quaternion.identity;

        isHolding = true;
        closestDistance = -1;

        return true;
    }

    private void AttachItemToRoot(GameObject item)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (!isHolding && other.CompareTag("Pickup"))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (closestDistance == -1)
                closestDistance = distance;
            else if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = other.gameObject;
            }

        }
    }
}
