using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;
    public Vector3 moveDirection;
    public float stopY;

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;

        transform.position += speed * Time.deltaTime * moveDirection;

        if(transform.position.y <= stopY)
        {
            Vector3 newPos = transform.position;
            newPos.y = stopY;
            transform.position = newPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Alteruna.Avatar _avatar))
        { 
            if (!_avatar.IsMe)
                return;

            PlayerDied(_avatar);
        }
    }

    void PlayerDied(Alteruna.Avatar _avatar)
    {
        _avatar.GetComponent<PlayerHealth>().MessageDeath(_avatar.Possessor.Index);
    }
}
