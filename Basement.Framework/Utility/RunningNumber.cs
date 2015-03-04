
using System;
using System.Collections.Generic;
using System.Text;


namespace Basement.Framework.Utility
{
    public class SingleTon<T> where T:new()
    {
        private static readonly T instance = new T();
        private SingleTon()
        { }

        static public T Instance
        {
            get { return instance; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RunningNumber
    {
        public RunningNumber()
        { 
        }
        /// <summary>
        /// 流水号队列
        /// </summary>
        private System.Collections.Hashtable hash_Searil = new System.Collections.Hashtable();

        private void Init()
        {
            lock (this)
            {
 
            }
        }

        public string GetRunningNumber(string strType)
        {
            if (hash_Searil.Contains(strType))
            {
                //hash_Searil[strType] = hash_Searil[strType] + 1;
            }
            string strNumber = string.Empty;
            return strNumber;
        }

    }
}
