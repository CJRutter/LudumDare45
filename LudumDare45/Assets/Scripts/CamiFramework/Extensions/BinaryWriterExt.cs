using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace CamiFramwork.Extensions
{
    public static class BinaryWriterExt
    {
        public static void WriteTransform(this BinaryWriter writer, Transform transformToSave)
        {
            writer.WriteTransform(transformToSave, true, true);
        }
    
        public static void WriteTransform(this BinaryWriter writer, Transform transformToSave, bool includeRotation, bool includeScale)
        {
            writer.Write(transformToSave.localPosition.x);
            writer.Write(transformToSave.localPosition.y);

            if (includeRotation)
                writer.Write(transformToSave.localRotation.eulerAngles.z);

            if (includeScale)
            {
                writer.Write(transformToSave.localScale.x);
                writer.Write(transformToSave.localScale.y);
            }
        }

        public static void WriteVector2(this BinaryWriter writer, Vector2 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
        }

        public static void WriteVector3(this BinaryWriter writer, Vector3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }
        
        public static void WriteColour(this BinaryWriter writer, Color color)
        {
            byte a = (byte)(color.a * 255);
            byte r = (byte)(color.r * 255);
            byte g = (byte)(color.g * 255);
            byte b = (byte)(color.b * 255);
            writer.Write(a);
            writer.Write(r);
            writer.Write(g);
            writer.Write(b);
        }

        public static void WriteHingeJoint2D(this BinaryWriter writer, HingeJoint2D joint)
        {
            byte flags = 0;

            if (joint.enabled)
                flags |= 0x01;
            if (joint.useMotor)
                flags |= 0x02;
            if (joint.useLimits)
                flags |= 0x04;

            writer.Write(flags);
            writer.WriteVector2(joint.anchor);
            writer.WriteVector2(joint.connectedAnchor - (Vector2)joint.transform.position);

            if (joint.useMotor)
            {
                writer.Write(joint.motor.motorSpeed);
                writer.Write(joint.motor.maxMotorTorque);    
            }

            if (joint.useLimits)
            {
                writer.Write(joint.limits.min);
                writer.Write(joint.limits.max);
            }
        }
    }
}