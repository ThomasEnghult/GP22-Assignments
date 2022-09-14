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
    public int color = 1;
    int red = 0, green = 0, blue = 0;
    int ocilating = 0;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 16;
    }

    // Update is called once per frame
    void Update()
    {
        StrokeWeight(diameter);
        ocilating = (int)Mathf.Ceil(Mathf.Sin(Mathf.Floor(Time.timeSinceLevelLoad) / 255));
        Debug.Log(ocilating);


        Background(0); //Clears the background and sets the color to 255.
        ParabolicCurves(axis1,axis2, numberOfLines);

        

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


        //Moves the lines across the screen
        x += Time.deltaTime;
        y += Time.deltaTime;

    }
    void ParabolicCurves(float axis1, float axis2, int numberOfLines)
    {
        

        for (int i = 0; i < numberOfLines; i++)
        {

            

            if (i % 3 == 0)
            {
                
                Stroke(red, green, blue);
                red = (red + 1*ocilating) % 255;
                green = (255 - red + 85) % 255;
                blue = (255 - green + 85) % 255;
            }
            //Draws the lines
            Line(axis1 / numberOfLines * i, 0, 0, axis2 - (axis2 / numberOfLines * i));
        }
    }

    void changeColor(int red, int green, int blue)
    {

        Stroke(red, blue, green);
    }

}
