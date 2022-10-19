using UnityEngine;

public class PlayerMovememt : MonoBehaviour
{
    public float speed = 500;
    public float maxSpeed = 10;
    public float rotationSpeed = 350;
    public GameObject thrust1, thrust2;

    Rigidbody2D rb2d;
    float rotation;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
	{
		//Forward motion
		float acc = Input.GetAxis("Vertical");
		acc = Mathf.Clamp(acc, 0, 1);

		if (rb2d.velocity.sqrMagnitude < maxSpeed * maxSpeed)
			rb2d.AddForce(transform.up * acc * speed * Time.deltaTime);

		//Rotation
		float rot = Input.GetAxis("Horizontal");
		rotation -= rot * Time.deltaTime * rotationSpeed;
		rb2d.MoveRotation(rotation);

		//Toggle art for the ship thrusters
		ToggleTrusters(acc, rot);
	}

	private void ToggleTrusters(float acc, float rot)
	{
		if (acc > 0)
		{
			thrust1.SetActive(true);
			thrust2.SetActive(true);
		}
		else if(rot < 0)
		{
			thrust1.SetActive(false);
			thrust2.SetActive(true);
		}
		else if (rot > 0)
		{
			thrust1.SetActive(true);
			thrust2.SetActive(false);
		}
		else
		{
			thrust1.SetActive(false);
			thrust2.SetActive(false);
		}
	}
}
