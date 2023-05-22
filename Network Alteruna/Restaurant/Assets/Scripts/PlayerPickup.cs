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
        GameObject closestItem = CheckForClosestItem();

        if(closestItem == null)
        {
            Debug.Log("No item nearby to grab");
            return false;
        }
        isHolding = true;

        InvokeRemoteMethod(nameof(AttachItemToRoot), UserId.AllInclusive, closestItem.transform);

        return true;
    }

    private GameObject CheckForClosestItem()
    {
        float closestDistance = -1;
        GameObject closestItem = null;

        var itemsInRange = Physics.OverlapSphere(transform.position, pickupRadius, ~0, QueryTriggerInteraction.Collide);
        foreach(var item in itemsInRange)
        {
            if (item.CompareTag("Pickup"))
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);

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
    private void AttachItemToRoot(Transform item)
    {
        item.parent = itemRoot;
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
    }
}
