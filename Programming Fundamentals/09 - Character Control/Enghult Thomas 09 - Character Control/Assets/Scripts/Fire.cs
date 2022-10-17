using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject bullet;
    static public float fireCooldown = 0.05f;
    private float cooldown = 0;

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetMouseButton(0) && cooldown <= 0)
        {
            cooldown = fireCooldown;
            var newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.transform.localScale = newBullet.transform.localScale * fireCooldown * 10;
            newBullet.AddComponent<BulletController>();
        }
    }
}
