using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    float width, height;
    float margin = 0.25f;
    Vector2 wrap;

    void Start()
    {
        //Calculate screen size
        width = Camera.main.orthographicSize * Camera.main.aspect + margin;
        height = Camera.main.orthographicSize + margin;
    }

    // Update is called once per frame
    void Update()
    {
        wrap = transform.position;

        //Screen wrap magic :)
        wrap.x = (wrap.x + width * 3) % (width * 2) - width;
        wrap.y = (wrap.y + height * 3) % (height * 2) - height;

        transform.position = wrap;
    }
}
