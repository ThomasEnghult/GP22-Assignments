using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignment : ProcessingLite.GP21
{
    public float x = 0;
    public float y = 0;
    public float diameter = 0.2f;

    public float axis1 = 10, axis2 = 10;
    public int numberOfLines = 10;

    public int red = 255, green = 0, blue = 255;
    int count = 0;
    int colorcount = 0;
    public int framerate = 85;
    int[] occilate = new int[] { -1, 1 };
    public float colorchangespeed = 1;
    public float spaceBetweenLines = 2f;
    public float scanchangespeed = 1;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = framerate;
        StrokeWeight(diameter);
        


        Background(0); //Clears the background and sets the color to 0.
        ParabolicCurves(axis1,axis2, numberOfLines);

        scanLines(spaceBetweenLines, scanchangespeed);

        //Changes color of the drawn lines between red and green
        if (count % (int)(framerate*colorchangespeed) /255 == 0)
        {
            // Occilates between -1 and 1
            int change = occilate[Mathf.FloorToInt(colorcount / 255) % 2];

            //Changes the value of red
            if (colorcount / (255) % 3 == 0)
            {
                //Debug.Log("inside red");
                red = red + change * (int)(255 * colorchangespeed) / framerate;
            }
            //Changes the value of green
            if (colorcount / (255) % 3 == 1)
            {
                //Debug.Log("inside green");
                green = green + change * (int)(255 * colorchangespeed) / framerate;
            }
            //Changes the value of blue
            if (colorcount / (255) % 3 == 2)
            {
                //Debug.Log("inside blue");
                blue = blue + change * (int)(255 * colorchangespeed) / framerate;
            }

            //Stroke function with over/under-flow protection
            changeColor(red, green, blue);

            //How fast we change the hue
            colorcount += (int)(255 * colorchangespeed)/ framerate;
        }

        //Draws my name on the screen
        drawName(2,Height-3.5f);

        //Moves the lines across the screen
        x += Time.deltaTime;
        y += Time.deltaTime;

        // counts the frames
        count++;
    }

    void drawName(float x, float y)
    {
        // Line(Point A(x, y), Point B(x, y))
        //T 0-2, 1-3
        Line(1 + x, 1 + y, 1 + x, 3 + y);
        Line(0 + x, 3 + y, 2 + x, 3 + y);

        //h 3-4, 1-3
        Line(3 + x, 1 + y, 3 + x, 3 + y);
        Line(3 + x, 2 + y, 4 + x, 2 + y);
        Line(4 + x, 1 + y, 4 + x, 2 + y);

        //o 5-6, 1-3
        Square(5.5f + x, 1.5f + y, 1f);

        //m 7-9, 1-2
        Line(7 + x, 1 + y, 7 + x, 2 + y); //  |
        Line(7 + x, 2 + y, 8 + x, 2 + y);// -
        Line(8 + x, 1 + y, 8 + x, 2 + y);// |
        Line(8 + x, 2 + y, 9 + x, 2 + y);// -
        Line(9 + x, 1 + y, 9 + x, 2 + y);// |

        //a 10-11, 1-2
        Square(10.5f + x, 1.5f + y, 1f);
        Line(11.5f + x, 1 + y, 11 + x, 2 + y);

        //s 12-13, 1-2
        Line(12 + x, 2 + y, 13 + x, 2 + y);
        Line(12 + x, 1.5f + y, 12 + x, 2 + y);
        Line(12 + x, 1.5f + y, 13 + x, 1.5f + y);
        Line(13 + x, 1.5f + y, 13 + x, 1 + y);
        Line(12 + x, 1f + y, 13 + x, 1 + y);
    }
    
    void ParabolicCurves(float axis1, float axis2, int numberOfLines)
    {
        for (int i = 0; i < numberOfLines; i++)
        {
            //Draws the lines
            Line(axis1 / numberOfLines * i, 0, 0, axis2 - (axis2 / numberOfLines * i));
        }
    }

    void scanLines(float spaceBetweenLines, float speed)
    {
        for (int i = 0; i < Height / spaceBetweenLines; i++)
        {
            //Increase y-cord each time loop run
            float y1 = i * spaceBetweenLines;

            //Draw a line from left side of screen to the right
            float y2 = y * speed % spaceBetweenLines;
            Line(0, y1 + y2, Width, y1 + y2);
        }
    }
    void changeColor(int red, int green, int blue)
    {
        if (red > 255)
        {
            red = 255;
        }
        if (red < 0)
        {
            red = 0;
        }
        if (green > 255)
        {
            green = 255;
        }
        if (green < 0)
        {
            green = 0;
        }
        if (blue > 255)
        {
            blue = 255;
        }
        if (blue < 0)
        {
            blue = 0;
        }

        Stroke(red, green, blue);
    }
}
