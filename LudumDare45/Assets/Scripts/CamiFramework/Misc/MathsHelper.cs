using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathsHelper
{
    public const float TAU = Mathf.PI * 2f;
    public const float HalfPi = Mathf.PI / 2f;
    public const float SQRT2 = 1.414213f;

    public static float DistanceSquared(Vector3 vec1, Vector3 vec2)
    {
        Vector3 vec = vec2 - vec1;
        return vec.sqrMagnitude;
    }

    public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
    {
        Vector2 vec = vec2 - vec1;
        return vec.sqrMagnitude;
    }

    public static Vector3 Perp2D(Vector3 vec)
    {
        float temp = vec.y;
        vec.y = -vec.x;
        vec.x = temp;
        return vec;
    }

    public static float WrapAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;
        if (angle < -180f)
            angle += 360f;

        return angle;
    }
    
    public static float FracNeg(float value)
    {
        return value - Mathf.Floor(value);
    }
        
    public static float FracPos(float value)
    {
        return 1f - value + Mathf.Floor(value);
    }
}

