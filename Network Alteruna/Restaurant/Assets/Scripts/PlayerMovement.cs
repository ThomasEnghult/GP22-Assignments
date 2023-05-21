using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Inputs
    private PlayerControls inputActions;
    private CharacterController controller;
    private InputAction move_IA;
    private InputAction jump_IA;
    private InputAction run_IA;

    //Character Movement
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerWalkSpeed = 4.0f;
    [SerializeField] private float playerRunSpeed = 6.0f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    private Vector3 move;

    //Alteruna
    private Alteruna.Avatar _avatar;
    private bool usingAlteruna;

    //Camera
    public GameObject cameraPosition;

    //Animation
    private Animator animator;

    private void Awake()
    {
        usingAlteruna = TryGetComponent(out _avatar);

        inputActions = GetComponent<InputController>().inputActions;
    }

    void Start()
    {
        //Character Controller
        controller = GetComponent<CharacterController>();


        //Alteruna
        if (usingAlteruna && !_avatar.IsMe)
        {
            enabled = false;
            return;
        }

        //Camera
        Camera.main.GetComponent<SimpleCameraController>().trackObject = gameObject;
        Camera.main.GetComponent<SimpleCameraController>().distanceFromObject = cameraPosition.transform;

        //Animation
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateAnimation();
    }


    private void UpdateMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 moveInputs = move_IA.ReadValue<Vector2>();
        Vector3 newMove = new Vector3(moveInputs.x, 0, moveInputs.y);

        float speed = run_IA.IsPressed() ? playerRunSpeed : playerWalkSpeed;

        move = Vector3.Lerp(move, newMove, Time.deltaTime * turnSpeed);

        controller.Move(move * Time.deltaTime * speed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        animator.SetTrigger("jumped");
        animator.SetBool("isJumping", true);
    }

    private void UpdateAnimation()
    {
        bool isMoving = (move_IA.ReadValue<Vector2>() != Vector2.zero);
        animator.SetBool("isMoving", isMoving);

        bool isRunning = run_IA.IsPressed() && isMoving;
        animator.SetBool("isRunning", isRunning);

        bool isJumping = !controller.isGrounded;
        animator.SetBool("isJumping", isJumping);
    }


    private void OnEnable()
    {
        //Move
        move_IA = inputActions.Player.Move;
        move_IA.Enable();

        //Jump
        jump_IA = inputActions.Player.Jump;
        jump_IA.Enable();
        jump_IA.performed += Jump;

        //Run
        run_IA = inputActions.Player.Run;
        run_IA.Enable();

    }

    private void OnDisable()
    {
        move_IA.Disable();
        jump_IA.Disable();
        run_IA.Disable();
    }
}