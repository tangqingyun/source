using Basement.Framework.Data.Attribute;
using Basement.Framework.Data.Core;
using Basement.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Basement.Framework.Data
{
    public class SqlServer : BaseDatabase
    {
        public SqlServer(string connectionName)
            : base(connectionName)
        {
        }

        public override int Insert<T>(T model)
        {
            IList<DbParameter> parms = null;
            this.ConvertParameters<T>(model, ref parms);
            int n = this.ExecuteNonQuery(SqlServerLanguage<T>.InsertScript, parms);
            return n;
        }

        public override int Insert<T>(IEnumerable<T> list)
        {
            DbParameter[] parmsAll = new SqlParameter[0];
            DbParameter[] parms = null;
            StringBuilder scriptBlock = new StringBuilder();
            scriptBlock.AppendLine(SqlServerLanguage<T>.InsertBatchScript);
            int index = 0;

            foreach (T item in list)
            {
                this.ConvertBatchParameters<T>(item, ref parms, ref scriptBlock, index++);
                parmsAll = parmsAll.Concat(parms).ToArray();
            }

            return this.ExecuteNonQuery(scriptBlock.ToString(), parmsAll, System.Data.CommandType.Text);
          
        }

        public override object InsertIdentity<T>(T model)
        {
            IList<DbParameter> parms = null;
            this.ConvertParameters<T>(model, ref parms);
            return this.ExecuteScalar(SqlServerLanguage<T>.InsertScriptIdentity, parms);
        }

        public override DataTable GetPageList(string script, string condition, string orderby, int size, int index)
        {
            string sql = PageScript(script, condition, orderby, index, size);
            DataTable dt = this.GetDataTable(sql, null);
            return dt;
        }

        public override IEnumerable<T> GetPageList<T>(string script, string condition, string orderby, int index, int size)
        {
            string sql = PageScript(script, condition, orderby, index, size);
            return ExecuteList<T>(sql);
        }
        
    }


    
}


