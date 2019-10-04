using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace CamiFramwork.Extensions
{
    public static class BinaryReaderExt
    {
        public static void ReadTransform(this BinaryReader reader, Transform transformToLoad)
        {
            reader.ReadTransform(transformToLoad, true, true);
        }

        public static void ReadTransform(this BinaryReader reader, Transform transformToLoad, bool includeRotation, bool includeScale)
        {
            Vector2 position;
            position.x = reader.ReadSingle();
            position.y = reader.ReadSingle();
            transformToLoad.localPosition = position;

            if (includeRotation)
                transformToLoad.localRotation = Quaternion.Euler(0f, 0f, reader.ReadSingle());

            if (includeScale)
            {
                Vector3 scale = new Vector3(1f, 1f, 1f);
                scale.x = reader.ReadSingle();
                scale.y = reader.ReadSingle();
                transformToLoad.localScale = scale;
            }
        }

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            Vector2 value;
            value.x = reader.ReadSingle();
            value.y = reader.ReadSingle();
            return value;
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            Vector3 value;
            value.x = reader.ReadSingle();
            value.y = reader.ReadSingle();
            value.z = reader.ReadSingle();
            return value;
        }

        public static Color ReadColour(this BinaryReader reader)
        {
            byte a = reader.ReadByte();
            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();

            Color colour;
            colour.a = a / 255f;
            colour.r = r / 255f;
            colour.g = g / 255f;
            colour.b = b / 255f;
            return colour;
        }

        public static void ReadHingeJoint2D(this BinaryReader reader, HingeJoint2D joint)
        {
            byte flags = reader.ReadByte();

            joint.enabled = (flags & 0x01) > 0;
            joint.anchor = reader.ReadVector2();
            joint.connectedAnchor = reader.ReadVector2() + (Vector2)joint.transform.position;

            if ((flags & 0x02) > 0)
            {
                joint.useMotor = true;
                JointMotor2D motor = joint.motor;
                motor.motorSpeed = reader.ReadSingle();
                motor.maxMotorTorque = reader.ReadSingle();
                joint.motor = motor;
            }
            else
            {
                joint.useMotor = false;
            }

            if ((flags & 0x04) > 0)
            {
                joint.useLimits = true;
                JointAngleLimits2D limits = joint.limits;
                limits.min = reader.ReadSingle();
                limits.max = reader.ReadSingle();
                joint.limits = limits;
            }
            else
            {
                joint.useLimits = false;
            }
        }
    }
}