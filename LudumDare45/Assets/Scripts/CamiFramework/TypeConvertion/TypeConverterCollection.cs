using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.TypeConvertion
{
    public class TypeConverterCollection : List<ITypeConverter>, ITypeConverter
    {
        public TypeConverterCollection()
        {
        }
        
        public TypeConverterCollection(params ITypeConverter[] converters)
        {
            AddConverter(converters);
        }

        public void AddConverter(params ITypeConverter[] converters)
        {
            foreach(ITypeConverter converter in converters)
            {
                Add(converter);
            }
        }

        public ITypeConverter GetConverter(Type sourceType, Type destinType)
        {
            foreach(ITypeConverter converter in this)
            {
                if (converter.CanConvert(sourceType, destinType))
                    return converter;
            }

            return null;
        }

        public bool CanConvert(Type sourceType, Type destinType)
        {
            foreach(ITypeConverter converter in this)
            {
                if (converter.CanConvert(sourceType, destinType))
                    return true;
            }

            return GetConverter(sourceType, destinType) != null;
        }

        public object Convert(object source, Type destinType)
        {
            ITypeConverter converter = GetConverter(source.GetType(), destinType);

            if (converter == null)
                return null;

            return converter.Convert(source, destinType);
        }

        #region Properties
        #endregion Properties

        #region Fields
        #endregion Fields
    }
}
