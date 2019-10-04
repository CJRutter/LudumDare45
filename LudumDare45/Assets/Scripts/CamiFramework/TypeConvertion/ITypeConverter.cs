using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CamiFramwork.TypeConvertion
{
    public interface ITypeConverter
    {
        bool CanConvert(Type sourceType, Type destinType);
        object Convert(object source, Type destinType);
    }
}