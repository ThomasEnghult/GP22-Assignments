using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float movementspeed = 5;
    public float sprintSpeed = 1.5f;
    public float skateBaseSpeed = 1.5f;
    public float skateSprintSpeed = 2;

    public Animator anim;

    float xMove, yMove;

    //public Transform elbow_left, elbow_right;
    public GameObject skateboard;
    AudioSource walkSound;
    bool isPlayingWalkSound = false;

    //Vector3 currentEulerAngles;
    float skateSprint;

    // Start is called before the first frame update
    void Start()
    {
        walkSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move with arrow/WASD keys
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");

        //Move with mouse
        if (Input.GetMouseButton(0))
        {
            Vector3 aim = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            xMove = aim.x;
            yMove = aim.y;
        }

        //Sets the flags to tell what the player is doing
        bool isMoving = yMove != 0 || xMove != 0;
        bool isSkateboarding = Input.GetButton("Jump");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isSkateboarding", isSkateboarding);
        anim.SetBool("isSprinting", isSprinting);

        //Set movespeed
        float movement = movementspeed * Time.deltaTime;

        //Move faster if sprinting
        if (isSprinting && !isSkateboarding)
        {
            movement *= sprintSpeed;
            walkSound.pitch = 1.5f;
        }
        else
            walkSound.pitch = 1;

            //Skate with shift
            if (isSprinting && isSkateboarding)
        {
            //Set skate velocity to max
            if (skateSprint < 1)
                skateSprint = skateSprintSpeed;

            //Decrease skate velocity over time
            skateSprint = skateSprint - Time.deltaTime;
            movement *= skateSprint;
        }

        //Render skateboard
        skateboard.SetActive(isSkateboarding && isMoving);

        // If moving
        if (isMoving)
        {
            //Calculate where to move player
            Vector3 movePlayer = Vector3.Normalize(new Vector3(xMove, yMove)) * movement;
            
            if (isSkateboarding)
            {
                movePlayer *= skateBaseSpeed;
            }

            //Move player
            transform.position += movePlayer;
            transform.up = new Vector3(xMove, yMove);
        }


        if (isMoving && !isSkateboarding)
        {
            if (!isPlayingWalkSound)
            {
                isPlayingWalkSound = true;
                walkSound.Play(0);
            }


            //Get the arms position
            //currentEulerAngles += new Vector3(x, y, z) * movement * armRotationSpeed;
        }
        else
        {
            if (isPlayingWalkSound)
            {
                isPlayingWalkSound = false;
                walkSound.Pause();
            }
            //Get arm position into neutral
            //currentEulerAngles = new(90, currentEulerAngles.y, currentEulerAngles.z);
        }

        //Set the arms position
        //elbow_left.localEulerAngles = currentEulerAngles;
        //elbow_right.localEulerAngles = currentEulerAngles;
    }
}


