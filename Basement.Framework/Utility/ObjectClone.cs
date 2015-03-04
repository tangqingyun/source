using Basement.Framework.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Utility
{
    public static class ObjectClone<T> where T : class , new()
    {
        public static T Clone(T obj)
        {
            ISerializer<T> serializer = Serializer<T>.BinaryFormatter;
            byte[] bytes = serializer.ToBinary(obj);
            return serializer.FromBinary(bytes);
        }
    }
}
