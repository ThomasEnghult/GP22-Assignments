using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 4.0f;   
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private Alteruna.Avatar _avatar;
    public GameObject cameraPosition;

    private void Awake()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
    }

    void Start()
    {

        if (!_avatar.IsMe)
        {
            enabled = false;
            return;
        }

        GameManager.Instance.SetMyAvatar(_avatar);

        controller = GetComponent<CharacterController>();

        Camera.main.GetComponent<SimpleCameraController>().trackObject = gameObject;
        Camera.main.GetComponent<SimpleCameraController>().distanceFromObject = cameraPosition.transform;


        //Camera.main.gameObject.transform.parent = cameraPosition.transform;
        //Camera.main.transform.localPosition = Vector3.zero;
        //Camera.main.transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            Debug.Log("Jumped");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void ResetMovement()
    {
        playerVelocity = Vector3.zero;
    }

}