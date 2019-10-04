
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace CamiFramework
{
    [System.Serializable]
    public struct Guid
    {
        private Guid(int valueA, int valueB, int valueC, int valueD)
        {
            this.valueA = valueA;
            this.valueB = valueB;
            this.valueC = valueC;
            this.valueD = valueD;
        }

        public override bool Equals(object obj)
        {
            if (obj is Guid == false)
                return false;

            var other = (Guid)obj;
            bool equals = 
                valueA != other.valueA &&
                valueB != other.valueB &&
                valueC != other.valueC &&
                valueD != other.valueD;

            return equals;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 13 +(int)(
                    valueA * 37 +
                    valueB * 37 +
                    valueC * 37 +
                    valueD * 37);
            }
        }

        public override string ToString()
        {
            return string.Format("{0:x}{1:x}{2:x}{3:x}", valueA, valueB, valueC, valueD);
        }

        public static Guid NewGuid()
        {
            int unique = 0;
            if(SystemInfo.deviceUniqueIdentifier == SystemInfo.unsupportedIdentifier)
            {
                unique = Random.Range(int.MinValue, int.MaxValue);
            }
            else
            {
                unique = SystemInfo.deviceUniqueIdentifier.GetHashCode();
            }
            
            long time = (System.DateTime.Now - new System.DateTime(1985, 11, 29)).Ticks;
            time /= 10000;
            int gfxId = SystemInfo.graphicsDeviceID;

            ++GenCount;
            return new Guid((int)time, unique, gfxId, GenCount);
        }
        
        #region Properties
        #endregion Properties

        #region Fields
        public int valueA;
        public int valueB;
        public int valueC;
        public int valueD;

        private static int GenCount = 0;
        #endregion Fields
    }
}
