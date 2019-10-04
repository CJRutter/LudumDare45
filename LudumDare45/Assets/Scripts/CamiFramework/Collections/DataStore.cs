using System;
using System.Collections.Generic;

namespace Cami.Collections
{
    public class DataStore
    {
        public DataStore()
        {
            intValues = new Dictionary<int, int>();
            floatValues = new Dictionary<int, float>();
            stringValues = new Dictionary<int, string>();
            objectValues = new Dictionary<int, object>();
        }

        public void Clear()
        {
            intValues.Clear();
            floatValues.Clear();
            stringValues.Clear();
            objectValues.Clear();
        }

        #region Ints
        public int GetInt(int key)
        {
            return intValues[key];
        }

        public void SetInt(int key, int value)
        {
            intValues[key] = value;
        }

        public bool HasInt(int key)
        {
            return intValues.ContainsKey(key);
        }
        #endregion Ints

        #region Floats
        public float GetFloat(int key)
        {
            return floatValues[key];
        }

        public void SetFloat(int key, float value)
        {
            floatValues[key] = value;
        }

        public bool HasFloat(int key)
        {
            return floatValues.ContainsKey(key);
        }
        #endregion Floats

        #region Strings
        public string GetString(int key)
        {
            return stringValues[key];
        }

        public void SetString(int key, string value)
        {
            stringValues[key] = value;
        }

        public bool HasString(int key)
        {
            return stringValues.ContainsKey(key);
        }
        #endregion Strings

        #region Objects
        public object GetObject(int key)
        {
            return objectValues[key];
        }

        public T GetObject<T>(int key)
        {
            return (T)objectValues[key];
        }

        public void SetObject(int key, object value)
        {
            objectValues[key] = value;
        }

        public bool HasObject(int key)
        {
            return objectValues.ContainsKey(key);
        }
        #endregion Objects

        #region Properties
        #endregion Properties

        #region Fields
        private Dictionary<int, int> intValues;
        private Dictionary<int, float> floatValues;
        private Dictionary<int, string> stringValues;
        private Dictionary<int, object> objectValues;
        #endregion Fields
    }
}
