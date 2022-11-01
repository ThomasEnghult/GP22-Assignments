using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public bool isGrounded = true;

    void FixedUpdate()
    {
        //Debug.Log(isGrounded);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mario"))
            return;
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mario"))
            return;
        isGrounded = false;
    }

}
