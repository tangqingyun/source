using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basement.Framework.Data.Core;

namespace Basement.Framework.Data
{
    public static class SqlServerLanguage<T> where T : class, IEntity, new()
    {
        public static void Initialize() { }

        private const string INSERT_BLOCK = "INSERT INTO {0}({1}) VALUES({2})";
        //private const string INSERT_BLOCK_IDENTITY = "INSERT INTO {0}({1}) VALUES({2}) SELECT @@IDENTITY";
        private const string INSERT_BLOCK_IDENTITY = "INSERT INTO {0}({1}) VALUES({2}) SELECT SCOPE_IDENTITY()";
        private const string INSERT_BATCH_BLOCK = "INSERT INTO {0}({1})";
        private const string UPDATE_BLOCK = "UPDATE {0} SET {1} {2}";
        private const string DELETE_BLOCK = "DELETE {0} ";
        private const string DELETE_BLOCK_FROM = "DELETE FROM {0} WHERE {1}";
        private const string ROWCOUNT_BLOCK = "SELECT COUNT(0) FROM {0}(NOLOCK) {1}";
        //private const string EXIST_BLOCK = "SELECT {0} EXISTS_ROW FROM {1}(NOLOCK) A WHERE EXISTS(SELECT {1} FROM {0}(NOLOCK) B {2})";
        private const string SELECT_BLOCK = "SELECT {0} FROM {1}(NOLOCK) {2} {3}";
        private const string SELECT_PADING = @"WITH _TBR_ AS(SELECT {0}, ROW_NUMBER() OVER({1}) AS _RID_ FROM {2}(NOLOCK) {3})
SELECT {4} FROM {2}(NOLOCK) WHERE {0} IN (SELECT {0} FROM _TBR_ WHERE _RID_ > {5} AND _RID_ <= {6})";
        private const string TOP_BLOCK = "SELECT TOP {0} {1} FROM {2}(NOLOCK) {3} {4}";
        private const string SINGLE_BLOCK = "SELECT {0} FROM {1}(NOLOCK) {2}";
       
         
        public static string InsertScript { get; private set; }
        public static string InsertScriptIdentity { get; private set; }
        public static string InsertBatchScript { get; private set; }
        public static string UpdateScript { get; private set; }
        public static string UpdateColmunsScript { get; private set; }
        public static string DeleteScript { get; private set; }
        public static string DeleteScriptFrom { get; private set; }
        public static string RowCountScript { get; private set; }
        public static string ExistScript { get; private set; }
        public static string SelectScript { get; private set; }
        public static string SelectPaddingScript { get; private set; }
        public static string TopScript { get; private set; }
        public static string SingleScript { get; private set; }

        static SqlServerLanguage()
        { 
            InsertScript = string.Format(INSERT_BLOCK, Entity<T>.TableName,
                string.Join(",", Entity<T>.ColumnsWithOutReadony),
                "@" + string.Join(",@", Entity<T>.ColumnsWithOutReadony));

            InsertScriptIdentity = string.Format(INSERT_BLOCK_IDENTITY, Entity<T>.TableName,
                string.Join(",", Entity<T>.ColumnsWithOutReadony),
                "@" + string.Join(",@", Entity<T>.ColumnsWithOutReadony));

            InsertBatchScript = string.Format(INSERT_BATCH_BLOCK, Entity<T>.TableName,
                string.Join(",", Entity<T>.ColumnsWithOutReadony));

            UpdateScript = string.Format(UPDATE_BLOCK, Entity<T>.TableName, 
                string.Join(",", Entity<T>.ColumnsWithOutReadony.Select(s=> string.Format("{0}=@{0}", s))), string.Empty);

            UpdateColmunsScript = string.Format(UPDATE_BLOCK, Entity<T>.TableName, "{0}", "{1}");

            //DeleteScript = string.Format(DELETE_BLOCK, Entity<T>.TableName, "{0}");
            //DeleteScriptFrom = string.Format(DELETE_BLOCK_FROM, Entity<T>.TableName);

            RowCountScript = string.Format(ROWCOUNT_BLOCK, Entity<T>.TableName, string.Empty);
                      
            TopScript = string.Format(TOP_BLOCK, "{0}",string.Join(",",Entity<T>.Columns), Entity<T>.TableName, "{1}", "{2}");

            SingleScript = string.Format(SINGLE_BLOCK, string.Join(",", Entity<T>.Columns), Entity<T>.TableName, "{0}"); 

            SelectScript = string.Format(SELECT_BLOCK, string.Join(",", Entity<T>.Columns), Entity<T>.TableName, "{0}", "{1}");

            SelectPaddingScript = string.Format(SELECT_PADING, 
                Entity<T>.PrimaryKey, 
                "{1}",//order by
                Entity<T>.TableName,
                "{0}",//where
              string.Join(",", Entity<T>.Columns),
              "{2}",//pageIndex
              "{3}"//pagesize
              );


        }

        
    }
}
