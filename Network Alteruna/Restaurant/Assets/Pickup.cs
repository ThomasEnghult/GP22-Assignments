using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    private PlayerControls inputActions;
    private InputAction grabRelease_IA;

    GameObject holdingItem;
    bool isHolding = false;

    GameObject closestItem;
    private float closestDistance = -1;
    [SerializeField] private Transform itemRoot;

    private Animator animator;

    private void Awake()
    {
        inputActions = GetComponent<InputController>().inputActions;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GrabRelease(InputAction.CallbackContext context)
    {
        if (!isHolding)
            OnGrab();
        else
            OnRelease();

        animator.SetBool("isHolding", isHolding);
    }

    private void OnRelease()
    {
        Debug.Log("Tried to release item");
    }

    private void OnGrab()
    {
        if(closestItem == null)
        {
            Debug.Log("No item nearby to grab");
            return;
        }

        holdingItem = closestItem;
        closestItem.transform.parent = itemRoot;
        closestItem.transform.localPosition = Vector3.zero;
        closestItem.transform.localRotation = Quaternion.identity;

        isHolding = true;
        closestDistance = -1;
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

    private void OnEnable()
    {
        //GrabRelease
        grabRelease_IA = inputActions.Player.GrabRelease;
        grabRelease_IA.Enable();
        grabRelease_IA.performed += GrabRelease;
    }

    private void OnDisable()
    {
        grabRelease_IA.Disable();
    }


}
