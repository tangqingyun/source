using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Basement.Framework.Utility
{
    public class AttributeHandle
    {
        public string GetCustomAttributes<T>(Type type,string propertyName)
        {
            var att = type.GetCustomAttributes(false);
            foreach (var fieldDisplayAttribute in att.OfType<T>())
            {
               // return fieldDisplayAttribute.Name;
            }
            return null;
        }
    }
}
