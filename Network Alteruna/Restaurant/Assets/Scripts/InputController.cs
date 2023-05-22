using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public PlayerControls inputActions;
    public InputAction move_IA;
    public InputAction jump_IA;
    public InputAction run_IA;
    private InputAction grabRelease_IA;

    //Alteruna
    private Alteruna.Avatar _avatar;
    private bool usingAlteruna;
    public bool isMe;

    //Camera
    [SerializeField] private Transform cameraPosition;

    //Movement
    PlayerMovement playerMovement;
    //Animation
    PlayerAnimation playerAnimation;
    //Pickup
    PlayerPickup playerPickup;

    private void Awake()
    {
        inputActions = new PlayerControls();
        usingAlteruna = TryGetComponent(out _avatar);
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerPickup = GetComponent<PlayerPickup>();
    }

    private void Start()
    {
        isMe = !usingAlteruna || _avatar.IsMe;

        if (!isMe)
        {
            enabled = false;
            return;
        }

        //Camera
        Camera.main.GetComponent<SimpleCameraController>().trackObject = gameObject;
        Camera.main.GetComponent<SimpleCameraController>().distanceFromObject = cameraPosition;
    }

    private void Update()
    {
        playerMovement.UpdateMovement(move_IA.ReadValue<Vector2>(), run_IA.IsPressed());
        playerAnimation.UpdateAnimationStates();
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

        //GrabRelease
        grabRelease_IA = inputActions.Player.GrabRelease;
        grabRelease_IA.Enable();
        grabRelease_IA.performed += GrabRelease;

    }

    private void OnDisable()
    {
        move_IA.Disable();
        jump_IA.Disable();
        run_IA.Disable();
        grabRelease_IA.Disable();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        playerMovement.Jump();
        playerAnimation.Jump();
    }

    private void GrabRelease(InputAction.CallbackContext context)
    {
        if (!playerPickup.isHolding)
        {
            playerPickup.GrabItem();
        }
        else
        {
            playerPickup.ReleaseItem();
        }
        //Set animation to the now updated state of holding
        playerAnimation.GrabRelease(playerPickup.isHolding);
    }
}
