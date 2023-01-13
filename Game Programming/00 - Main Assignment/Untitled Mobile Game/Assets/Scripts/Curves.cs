using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curves : MonoBehaviour
{
    public int lengthX = 10;
    public int lengthY = 10;
    public float speed = 0.1f;
    public float sphereRadius = 0.1f;

    private void OnDrawGizmos()
    {


        Vector2[,] points = new Vector2[lengthX, lengthY];

        for (int y = 0; y < lengthY; y++)
        {
            float yOffset = 360f * ((float)y / lengthY);
            for (int x = 0; x < lengthX; x++)
            {
                float xOffset = 360f * ((float)x / lengthX);
                float timeDecimals = Time.realtimeSinceStartup % 1;
                Vector3 position = new Vector3(x + Mathf.Sin(Time.realtimeSinceStartup + xOffset + yOffset),y + Mathf.Cos(Time.realtimeSinceStartup + yOffset + xOffset), Mathf.Sin(Time.realtimeSinceStartup + xOffset + yOffset));
                Color color = new Color(1, 1, 1);
                color.r = (1f - Mathf.Abs(Mathf.Cos(Time.realtimeSinceStartup + xOffset + yOffset) * 1f));
                color.g = (1f - Mathf.Abs(Mathf.Cos(Time.realtimeSinceStartup + xOffset + yOffset) * 0.5f));
                color.a = 1;
                Gizmos.color = color;
                Gizmos.DrawSphere(position, sphereRadius);
            }
        }
    }
}
