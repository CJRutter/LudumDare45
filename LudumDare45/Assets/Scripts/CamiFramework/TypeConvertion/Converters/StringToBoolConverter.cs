using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.TypeConvertion
{
    public class StringToBoolConverter : ITypeConverter
    {
        public bool CanConvert(Type sourceType, Type destinType)
        {
            return sourceType == typeof(string) && destinType == typeof(bool);
        }

        public object Convert(object source, Type destinType)
        {
            if (!CanConvert(source.GetType(), destinType))
                return null;

            bool value;
            if (!bool.TryParse((string)source, out value))
                return null;

            return value;
        }
    }
}