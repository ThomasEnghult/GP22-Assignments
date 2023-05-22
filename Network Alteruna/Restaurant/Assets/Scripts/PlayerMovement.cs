using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using Alteruna;

public class PlayerMovement : AttributesSync
{
    //Inputs
    private PlayerControls inputActions;
    private CharacterController controller;
    private InputAction move_IA;
    private InputAction jump_IA;
    private InputAction run_IA;

    //Character Movement
    [SerializeField] private float playerWalkSpeed = 4.0f;
    [SerializeField] private float playerRunSpeed = 6.0f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private Vector3 move;

    [SynchronizableField] public bool groundedPlayer;

    void Start()
    {
        //Character Controller
        controller = GetComponent<CharacterController>();
    }

    public void UpdateMovement(Vector2 moveInputs, bool isRunning)
    {

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 newMove = new Vector3(moveInputs.x, 0, moveInputs.y);
        float speed = isRunning ? playerRunSpeed : playerWalkSpeed;
        move = Vector3.Lerp(move, newMove, Time.deltaTime * turnSpeed);
        controller.Move(move * Time.deltaTime * speed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
}