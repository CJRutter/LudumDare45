using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.Extensions
{
    public static class BoundsExt
    {
        public static Vector3 GetClosestFaceNormal(this Bounds bounds, Vector3 point)
        {
            float closestDis = float.MaxValue;
            Vector3 normal = Vector3.zero;
            Vector3 minDif, maxDif;

            minDif = bounds.min - point;
            minDif = minDif.Abs();

            maxDif = bounds.max - point;
            maxDif = maxDif.Abs();
            
            // if(minDif.x < closestDis) // this will always be true
            {
                closestDis = minDif.x;
                normal = Vector3.left;
            }
            if(maxDif.x < closestDis)
            {
                closestDis = maxDif.x;
                normal = Vector3.right;
            }
            
            if(minDif.y < closestDis)
            {
                closestDis = minDif.y;
                normal = Vector3.down;
            }
            if(maxDif.y < closestDis)
            {
                closestDis = maxDif.y;
                normal = Vector3.up;
            }
            
            if(minDif.z < closestDis)
            {
                closestDis = minDif.z;
                normal = Vector3.back;
            }
            if(maxDif.z < closestDis)
            {
                closestDis = maxDif.z;
                normal = Vector3.forward;
            }

            return normal;
        }
    
        public static Bounds New(Vector3 min, Vector3 max)
        {
            Vector3 size = max - min;
            return new Bounds(min + (size / 2f), size);
        }
    
        public static bool Intersects(this Bounds bounds, Ray ray, float maxDistance, out Vector3 normal, out float distance)
        {
            Vector3 rayOrigin = ray.origin;
            Vector3 rayDelta = maxDistance * ray.direction;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            distance = float.MaxValue;
            normal = ray.direction;

            bool inside = true;

            #region Calc x values
            float xt, xn;
            if(rayOrigin.x < min.x)
            {
                xt = min.x - rayOrigin.x;
                if (xt > rayDelta.x)
                    return false;

                xt /= rayDelta.x;
                inside = false;
                xn = -1.0f;
            }
            else if (rayOrigin.x > max.x)
            {
                xt = max.x - rayOrigin.x;
                if (xt < rayDelta.x)
                    return false;

                xt /= rayDelta.x;
                inside = false;
                xn = 1.0f;
            }
            else
            {
                xt = -1.0f;
                xn = 0;
            }
            #endregion Calc x values
            
            #region Calc y values
            float yt, yn;
            if(rayOrigin.y < min.y)
            {
                yt = min.y - rayOrigin.y;
                if (yt > rayDelta.y)
                    return false;

                yt /= rayDelta.y;
                inside = false;
                yn = -1.0f;
            }
            else if (rayOrigin.y > max.y)
            {
                yt = max.y - rayOrigin.y;
                if (yt < rayDelta.y)
                    return false;

                yt /= rayDelta.y;
                inside = false;
                yn = 1.0f;
            }
            else
            {
                yt = -1.0f;
                yn = 0;
            }
            #endregion Calc y values
            
            #region Calc z values
            float zt, zn;
            if(rayOrigin.z < min.z)
            {
                zt = min.z - rayOrigin.z;
                if (zt > rayDelta.z)
                    return false;

                zt /= rayDelta.z;
                inside = false;
                zn = -1.0f;
            }
            else if (rayOrigin.z > max.z)
            {
                zt = max.z - rayOrigin.z;
                if (zt < rayDelta.z)
                    return false;

                zt /= rayDelta.z;
                inside = false;
                zn = 1.0f;
            }
            else
            {
                zt = -1.0f;
                zn = 0f;
            }
            #endregion Calc z values
            
            if(inside)
            {
                normal = -ray.direction;
                distance = 0f;
                return true;
            }

            int whichPlane = 0;
            float t = xt;
            if(yt > t)
            {
                whichPlane = 1;
                t = yt;
            }
            if(zt > t)
            {
                whichPlane = 2;
                t = zt;
            }

            switch(whichPlane)
            {
                case 0: // yz plane
                {
                    float y = rayOrigin.y + rayDelta.y * t;
                    if (y < min.y || y > max.y)
                        return false;

                    float z = rayOrigin.z + rayDelta.z * t;
                    if (z < min.z || z > max.z)
                        return false;

                    normal.x = xn;
                    normal.y = 0f;
                    normal.z = 0f;

                    break;
                }
                case 1: // xz plane
                {
                    float x = rayOrigin.x + rayDelta.x * t;
                    if (x < min.x || x > max.x)
                        return false;

                    float z = rayOrigin.z + rayDelta.z * t;
                    if (z < min.z || z > max.z)
                        return false;

                    normal.x = 0f;
                    normal.y = yn;
                    normal.z = 0f;
                    break;
                }
                case 2: // xy plane
                {
                    float x = rayOrigin.x + rayDelta.x * t;
                    if (x < min.x || x > max.x)
                        return false;

                    float y = rayOrigin.y + rayDelta.y * t;
                    if (y < min.y || y > max.y)
                        return false;

                    normal.x = 0f;
                    normal.y = 0f;
                    normal.z = zn;

                    break;
                }
            }

            distance = t * maxDistance;
            return true;
        }
    }
}