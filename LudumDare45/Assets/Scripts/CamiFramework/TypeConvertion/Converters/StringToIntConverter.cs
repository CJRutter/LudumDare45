using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.TypeConvertion
{
    public class StringToIntConverter : ITypeConverter
    {
        public bool CanConvert(Type sourceType, Type destinType)
        {
            return sourceType == typeof(string) && destinType == typeof(int);
        }

        public object Convert(object source, Type destinType)
        {
            if (!CanConvert(source.GetType(), destinType))
                return null;

            int value;
            if (!int.TryParse((string)source, out value))
                return null;

            return value;
        }
    }
}