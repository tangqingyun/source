using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.CustomSerializer
{
    public interface ISerialize<T>
    {
        string Serialize(T obj);
        T Deserialize(string stream);
    }
}
