using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class myBall : ProcessingLite.GP21
{
    public float radius;
    public float maxSpeed;
    public float speed;
    public float drag;
    public float bounce;
    public float gravityModifier;
    public bool gravity;

    public Vector2 position;
    ColorOfBall color;
    Vector2 force;
    Vector2 gravityForce;

    public myBall(float radius)
    {
        position.x = Random.Range(0, Width);
        position.y = Random.Range(0, Height);
        this.radius = radius;
        maxSpeed = -1;
        drag = 0;
        bounce = 1;
        speed = 5;
        gravity = false;
        gravityModifier = 10;
        color = new ColorOfBall(0);
    }

    public myBall(float x, float y, float radius)
    {
        position.x = x;
        position.y = y;
        this.radius = radius;
        maxSpeed = -1;
        drag = 0;
        bounce = 1;
        speed = 5;
        gravity = false;
        gravityModifier = 10;
        color = new ColorOfBall(0);
    }

    public bool Gravity()
    {
        return gravity;
    }
    public void Gravity(bool gravity)
    {
        this.gravity = gravity;
    }

    public void updatePos()
    {
        position += force * Time.deltaTime;
    }

    public void updatePos(Vector2 pos)
    {
        position += pos * speed * Time.deltaTime;
    }

    public void updateForce(Vector2 force)
    {
        if (gravity)
            gravityForce = gravityModifier * Vector3.down;
        else
            gravityForce = Vector3.zero;
        //Add gravity
        this.force += gravityForce * Time.deltaTime;
        //Add force
        this.force += force * speed * Time.deltaTime;
        //Add drag
        this.force -= this.force * (drag * Time.deltaTime);
        //Limit max speed
        if (maxSpeed != -1)
            this.force = Vector2.ClampMagnitude(this.force, maxSpeed);
    }

    public void setRandomForce(float max)
    {
        force.x = Random.Range(-max, max);
        force.y = Random.Range(-max, max); ;
    }

    public void setForce(float x, float y)
    {
        force.x = x;
        force.y = y;
    }

    public void setColor(int rgb)
    {
        color.rgb = rgb;
    }

    public void setColor(int r, int g, int b)
    {
        color.rgb = -1;
        color.r = r;
        color.g = g;
        color.b = b;
    }

    private void changeColor(ColorOfBall col)
    {
        if (col.rgb != -1)
            Fill(col.rgb);
        else
            Fill(col.r, col.g, col.b);
    }
    public void Draw()
    {
        //Set color
        changeColor(color);
        //Draw circle
        Circle(position.x, position.y, radius * 2);

        //Draw additional circles for the wrap around
        //Right side
        Circle(position.x + Width, position.y, radius * 2);
        //Left side
        Circle(position.x - Width, position.y, radius * 2);
    }

    public void CheckWallCollision()
    {
        WrapHorizontalEdge();
        InvertVerticalEdge();
        StopVerticalEdge();
    }

    private void WrapHorizontalEdge()
    {
        //Handles wrap around on right side
        if (position.x > Width)
            position.x = 0;
        //Handles wrap around on left side
        if (position.x < 0)
            position.x = Width;
    }
    private void StopVerticalEdge()
    {
        //Handles stop on top side
        if (position.y > Height - radius)
            position.y = Height - radius;
        //Handles stop on bottom side
        if (position.y < 0 + radius)
            position.y = 0 + radius;
    }
    private void InvertVerticalEdge()
    {
        //Invert the force if it hits top side
        if (position.y >= Height - radius)
            force.y *= -1;
        //Invert the force if it hits bottom side
        if (position.y <= 0 + radius)
            force.y *= -bounce;
    }

    public bool BallCollision(myBall other)
    {
        float maxDistance = this.radius + other.radius;
        float x1 = this.position.x;
        float x2 = other.position.x;
        float y1 = this.position.y;
        float y2 = other.position.y;

        if (Mathf.Abs(x1 - x2) > maxDistance || Mathf.Abs(y1 - y2) > maxDistance)
        {
            return false;
        }
        else if (Vector2.Distance(this.position, other.position) > maxDistance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void BallCollider(myBall other)
    {
        float m1 = this.radius;
        float x1 = this.position.x;
        float y1 = this.position.y;
        Vector2 v1 = this.force;

        float m2 = other.radius;
        float x2 = other.position.x;
        float y2 = other.position.y;
        Vector2 v2 = other.force;

        Vector2 newVelocity1 = v1 - (2 * m2) / (m1 + m2) * ((v1 - v2) * (x1 - x2)) / Mathf.Abs(x1 - x2) * (x1 - x2);
        Vector2 newVelocity2 = v2 - (2 * m1) / (m2 + m1) * ((v2 - v1) * (x2 - x1)) / Mathf.Abs(x2 - x1) * (x2 - x1);
        this.force = newVelocity1;
        other.force = newVelocity2;

        Debug.Log("Collider triggered!");
    }

}


public struct ColorOfBall
{
    public int rgb;
    public int r, g, b;

    public ColorOfBall(int rgb)
    {
        this.rgb = rgb;
        this.r = -1;
        this.g = -1;
        this.b = -1;

    }
    public ColorOfBall(int r, int g, int b)
    {
        rgb = -1;
        this.r = r;
        this.g = g;
        this.b = b;
    }
}