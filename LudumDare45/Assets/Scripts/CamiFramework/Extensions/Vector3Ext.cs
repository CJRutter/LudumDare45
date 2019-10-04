using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.Extensions
{
    public static class Vector3Ext
    {
        public static Vector3 New(float value)
        {
            return new Vector3(value, value, value);
        }
    
        public static Vector3 Round(this Vector3 vector)
        {
            vector.x = Mathf.Round(vector.x);
            vector.y = Mathf.Round(vector.y);
            vector.z = Mathf.Round(vector.z);
            return vector;
        }

        public static Vector3 Frac(this Vector3 vector)
        {
            Vector3 result = vector - vector.Round();
            return result;
        }

        public static Vector3 Abs(this Vector3 vector)
        {
            vector.x = Mathf.Abs(vector.x);
            vector.y = Mathf.Abs(vector.y);
            vector.z = Mathf.Abs(vector.z);
            return vector;
        }
    }
}