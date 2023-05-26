using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerPickup : AttributesSync
{
    GameObject holdingItem;
    [SynchronizableField] public bool isHolding = false;

    [SerializeField] private Transform itemRoot;
    [SerializeField] private float pickupRadius = 2;

    public void ReleaseItem()
    {
        Debug.Log("Tried to release item");
    }

    public bool GrabItem()
    {
        GameObject closestItem = CheckForClosestItem(transform.position, pickupRadius);

        if(closestItem == null)
        {
            Debug.Log("No item nearby to grab");
            return false;
        }
        isHolding = true;

        AttachItemToRoot(closestItem.transform);

        InvokeRemoteMethod(nameof(RemoteGrabItem), UserId.All, transform.position, pickupRadius);

        return true;
    }

    private GameObject CheckForClosestItem()
    {
        return CheckForClosestItem(transform.position, pickupRadius);
    }
    private GameObject CheckForClosestItem(Vector3 position, float pickupRadius)
    {
        float closestDistance = -1;
        GameObject closestItem = null;

        var itemsInRange = Physics.OverlapSphere(position, pickupRadius, ~0, QueryTriggerInteraction.Collide);
        foreach(var item in itemsInRange)
        {
            if (item.CompareTag("Pickup"))
            {
                float distance = Vector3.Distance(position, item.transform.position);

                if (closestDistance == -1 || distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item.gameObject;
                }
            }
        }

        return closestItem;

    }

    [SynchronizableMethod]
    public void RemoteGrabItem(Vector3 position, float pickupRadius)
    {
        isHolding = true;
        GameObject closestItem = CheckForClosestItem(position, pickupRadius);
        AttachItemToRoot(closestItem.transform);
    }

    private void AttachItemToRoot(Transform item)
    {
        item.parent = itemRoot;
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
    }
}
