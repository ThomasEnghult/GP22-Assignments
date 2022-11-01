using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{
    [Range(0, 25)][SerializeField] int moveSpeed = 8;
    [Range(0, 25)][SerializeField] int jumpHeight = 8;
    AudioSource jumpSound;
    Rigidbody2D rb2d;
    GroundCollider ground;
    CameraController camContr;

    int coinsCollected = 0;
    private bool hasJumped = false;
    
    void Start()
    {
        ground = GetComponentInChildren<GroundCollider>();
        rb2d = GetComponent<Rigidbody2D>();
        jumpSound = GetComponent<AudioSource>();
        camContr = GameObject.Find("Background").GetComponent<CameraController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && ground.isGrounded)
        {
            hasJumped = true;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");

        if (x == 1)
            transform.eulerAngles = new (transform.rotation.x, 0);

        if (x == -1)
            transform.eulerAngles = new(transform.rotation.x, 180);

        Vector3 movement = new Vector3(x * moveSpeed, 0) * Time.deltaTime;
        transform.position += movement;
        camContr.moveCameras(movement.x);

        if (hasJumped)
        {
            hasJumped = false;
            Jump();
        }
    }

    void Jump()
    {
        rb2d.AddForce(Vector2.up * jumpHeight * 2, ForceMode2D.Impulse);
        jumpSound.Play(0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            coinsCollected++;
            Destroy(other.gameObject);

            GameObject stats = GameObject.Find("CoinsCollected");
            stats.GetComponent<TMPro.TextMeshProUGUI>().text = new string("Coins Collected: " + coinsCollected);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            coinsCollected++;
            Destroy(other.gameObject);

            GameObject stats = GameObject.Find("CoinsCollected");
            stats.GetComponent<TMPro.TextMeshProUGUI>().text = new string("Coins Collected: " + coinsCollected);
        }
    }

}
