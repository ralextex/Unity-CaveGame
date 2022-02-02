using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 coord;
    public float density;

    public Point()
    {
        coord = new  Vector3(0, 0, 0);
        density = 0;
    }
    public Point(Vector3 point)
    {
        coord = point;
        density = 0;
    }

    public Point(Vector3 point, float v)
    {
        coord = point;
        density = v;
    }

    public void SetDensity(float value)
    {
        density = value;
    }


}
