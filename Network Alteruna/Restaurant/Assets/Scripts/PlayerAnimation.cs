using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Alteruna;

public class PlayerAnimation : AttributesSync
{
    //Inputs
    private InputController inputController;
    private InputAction move_IA;
    private InputAction jump_IA;
    private InputAction run_IA;

    //Animations
    private Animator animator;

    //Movement
    private PlayerMovement playerMovement;

    //Alteruna
    [SynchronizableField] private bool isMoving = false;
    [SynchronizableField] private bool isRunning = false;
    [SynchronizableField] private bool isJumping = false;
    [SynchronizableField] private bool isHolding = false;
    private bool startedJump = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputController = GetComponent<InputController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        move_IA = inputController.move_IA;
        jump_IA = inputController.jump_IA;
        run_IA = inputController.run_IA;
    }

    void Update()
    {
        UpdateAnimator();
    }

    public void UpdateAnimationStates()
    {
            isMoving = (move_IA.ReadValue<Vector2>() != Vector2.zero);
            isRunning = run_IA.IsPressed() && isMoving;
            isJumping = !playerMovement.groundedPlayer;
    }

    public void UpdateAnimator()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isHolding", isHolding);

        if (startedJump)
        {
            animator.SetTrigger("jumped");
            startedJump = false;
        }
    }

    public void Jump()
    {
        InvokeRemoteMethod(nameof(TriggerJump), UserId.AllInclusive);
    }

    [SynchronizableMethod]
    void TriggerJump()
    {
        startedJump = true;
    }

    public void GrabRelease(bool isHolding)
    {
        this.isHolding = isHolding;
    }
}
