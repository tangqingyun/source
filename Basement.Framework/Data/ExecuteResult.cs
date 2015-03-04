using Basement.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Basement.Framework.Data.Core;

namespace Basement.Framework.Data
{
    /// <summary>
    ///执行结果公用类
    /// </summary>
    internal class ExecuteResult
    {
        public static IList<T> GetDataReader<T>(DbCommand dbCommand)
        {
            IList<T> iList = null;
            if (dbCommand != null)
            {
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    iList = reader.ToList<T>().ToList();
                    //while (reader.Read())
                    //{
                    //    iList.Add(ReflectHandle.DataReaderAsModel<T>(reader));
                    //}
                    reader.Close();
                }
            }
            return iList;
        }

        public static T GetSingleDataReader<T>(DbCommand dbCommand) where T : new()
        {
            T t = default(T);
            if (dbCommand != null)
            {
                IDataReader dr = dbCommand.ExecuteReader();
                if (dr.Read())
                {
                    t = ReflectHandle.DataReaderAsModel<T>(dr);
                }
                dr.Close();
            }
            return t;
        }

        public static object GetScalar(DbCommand dbCommand)
        {
            return (dbCommand != null) ? dbCommand.ExecuteScalar() : null;
        }

        public static int GetNonQuery(DbCommand dbCommand)
        {
            return (dbCommand != null) ? dbCommand.ExecuteNonQuery() : 0;
        }

        public static bool GetExecuteBool(DbCommand dbCommand)
        {
            return (GetNonQuery(dbCommand) != 0) ? true : false;
        }

        public static bool GetValueConvertBool(DbCommand dbCommand)
        {
            int ReturnIntValue = 0;
            Int32.TryParse(GetScalar(dbCommand).ToString(), out ReturnIntValue);
            return (ReturnIntValue != 0) ? true : false;
        }

        public static DataSet GetDataSet(DbCommand dbCommand, DbProviderFactory dbProviderFactory)
        {
            DataSet dataSet = new DataSet();
            if (dbCommand != null)
            {
                using (DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter())
                {
                    ((IDbDataAdapter)adapter).SelectCommand = dbCommand;
                    adapter.Fill(dataSet);
                }
            }
            return dataSet;
        }

        public static DataTable GetDataTable(DbCommand dbCommand, DbProviderFactory dbProviderFactory)
        {
            DataSet dataSet = new DataSet();
            if (dbCommand != null)
            {
                using (DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter())
                {
                    ((IDbDataAdapter)adapter).SelectCommand = dbCommand;
                    adapter.Fill(dataSet);
                }
            }
            return dataSet.Tables[0];
        }

    }
}
