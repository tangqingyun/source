using Basement.Framework.Configuration;
using Basement.Framework.Data.Configuration;
using Basement.Framework.Data.Core;
using Basement.Framework.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Basement.Framework.Data
{
    /// <summary>
    /// ADO.NET数据库操作帮助类
    /// </summary>
    public class BaseDatabase : IDatabase
    {
        private readonly string connectionName;
        private DbProviderFactory dbProviderFactory;
        private DbCommand Command = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        public BaseDatabase(string connectionName)
            : this(connectionName, SqlClientFactory.Instance)
        {

        }

        private BaseDatabase(string connectionName, DbProviderFactory dbProviderFactory)
        {
            string errorMsg = string.Empty;
            string connection = connectionName;
            if (string.IsNullOrWhiteSpace(connection))
            {
                errorMsg = "请在Configs中设置" + connectionName + "节点";
            }

            try
            {
                this.connectionName = connection;
                this.dbProviderFactory = dbProviderFactory;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable Single(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            using (DbConnection connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType, listParms);
                    DataTable dt = ExecuteResult.GetDataTable(dbCommand, dbProviderFactory);
                    dbCommand.Parameters.Clear();
                    return dt;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql字符串</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual T SingleAsModel<T>(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text) where T : new()
        {
            CheckScript(sql);
            T t;
            using (DbConnection connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType, listParms);
                    t = ExecuteResult.GetSingleDataReader<T>(dbCommand);
                    dbCommand.Parameters.Clear();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
            return t;
        }

        /// <summary>
        /// 获取List集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">sql字符串</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual IEnumerable<T> ExecuteList<T>(string sql,
            IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            IList<T> list;
            using (DbConnection connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType, listParms);
                    list = ExecuteResult.GetDataReader<T>(dbCommand);
                    dbCommand.Parameters.Clear();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
            return list;
        }

        /// <summary>
        /// 获取单行单列
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual object ExecuteScalar(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            object o;
            using (DbConnection connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType, listParms);
                    o = ExecuteResult.GetScalar(dbCommand);
                    dbCommand.Parameters.Clear();
                }
                catch (Exception e)
                {
                    throw e;
                }
                connection.Close();
            }
            return o;
        }

        /// <summary>
        /// 获取单列
        /// </summary>
        /// <typeparam name="R">转化的类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">命令字符类型</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual R ExecuteScalar<R>(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            R obj = (R)ExecuteScalar(sql, listParms, commandType);
            return obj;
        }

        /// <summary>
        /// 获取bool值
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual bool ExecuteBool(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            bool bol;
            using (DbConnection connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType, listParms);
                    bol = ExecuteResult.GetExecuteBool(dbCommand);
                    dbCommand.Parameters.Clear();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
            return bol;
        }

        /// <summary>
        /// 执行增、删、改
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            int n;
            using (DbConnection connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType, listParms);
                    n = ExecuteResult.GetNonQuery(dbCommand);
                    dbCommand.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }

            return n;
        }

        public virtual bool ExecuteNoQuery(string script, IList<DbParameter> listParms, CommandType commandType = CommandType.Text)
        {
            int n = ExecuteNonQuery(script, listParms, commandType);
            return n > 0 ? true : false;
        }


        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string sql, IList<DbParameter> listParms, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            DataSet ds;
            using (DbConnection connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType, listParms);
                    ds = ExecuteResult.GetDataSet(dbCommand, dbProviderFactory);
                    dbCommand.Parameters.Clear();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }

            }
            return ds;
        }
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <param name="listParms">参数</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, IList<DbParameter> listParms, CommandType commandType = CommandType.Text)
        {
            CheckScript(sql);
            DataTable dt = null;
            try
            {
                using (DbConnection connection = this.CreateConnection())
                {
                    try
                    {
                        connection.Open();
                        DbCommand dbCommand = this.GetDbCommand(connection, sql, commandType,
                        listParms);
                        DbDataAdapter da = new SqlDataAdapter();
                        dt = ExecuteResult.GetDataTable(dbCommand, dbProviderFactory);
                        dbCommand.Parameters.Clear();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            return dt;
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="dic">SQL字典</param>
        /// <param name="commandType">解释命令字符串</param>
        /// <returns></returns>
        public virtual bool ExecuteTransation(IDictionary<string, IList<DbParameter>> dic, CommandType commandType = CommandType.Text)
        {
            using (DbConnection conn = this.CreateConnection())
            {
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    if (dic.Count == 0) return false;
                    try
                    {
                        foreach (var item in dic)
                        {
                            using (DbCommand cmd = this.GetDbCommand(trans.Connection, item.Key, commandType, item.Value))
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = trans;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                        }
                        trans.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw e;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }

        }

        public virtual ArrayList ExecuteList<T1, T2>(string script, IList<DbParameter> parameters, CommandType commandType = CommandType.Text)
        {
            CheckScript(script);
            ArrayList list = new ArrayList();
            using (DbConnection connection = this.CreateConnection())
            {
                DbCommand cmd = this.GetDbCommand(connection, script, commandType, parameters);
                DbDataReader reader = cmd.ExecuteReader();
                list.Add(reader.ToListWithUncertainty<T1>());
                if (reader.NextResult()) list.Add(reader.ToListWithUncertainty<T2>());
                reader.Close();
                cmd.Parameters.Clear();
            }
            return list;
        }

        public virtual ArrayList ExecuteList<T1, T2, T3>(string script, IList<DbParameter> parameters, CommandType commandType = CommandType.Text)
        {
            CheckScript(script);
            ArrayList list = new ArrayList();
            using (DbConnection connection = this.CreateConnection())
            {
                DbCommand cmd = this.GetDbCommand(connection, script, commandType, parameters);
                DbDataReader reader = cmd.ExecuteReader();
                list.Add(reader.ToListWithUncertainty<T1>());
                if (reader.NextResult()) list.Add(reader.ToListWithUncertainty<T2>());
                if (reader.NextResult()) list.Add(reader.ToListWithUncertainty<T3>());
                reader.Close();
                cmd.Parameters.Clear();
            }
            return list;
        }

        public virtual ArrayList ExecuteList<T1, T2, T3, T4>(string script, IList<DbParameter> parameters, CommandType commandType = CommandType.Text)
        {
            CheckScript(script);
            ArrayList list = new ArrayList();
            using (DbConnection connection = this.CreateConnection())
            {
                DbCommand cmd = this.GetDbCommand(connection, script, commandType, parameters);
                DbDataReader reader = cmd.ExecuteReader();
                list.Add(reader.ToListWithUncertainty<T1>());
                if (reader.NextResult()) list.Add(reader.ToListWithUncertainty<T2>());
                if (reader.NextResult()) list.Add(reader.ToListWithUncertainty<T3>());
                if (reader.NextResult()) list.Add(reader.ToListWithUncertainty<T4>());
                reader.Close();
                cmd.Parameters.Clear();
            }
            return list;
        }


        #region - 公共方法


        private DbCommand GetDbCommand(DbConnection dbConnection, string commandText,
         CommandType commandType, IList<DbParameter> ListDataParameter)
        {
            DbCommand dbCommand = this.dbProviderFactory.CreateCommand();
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = commandText;
            if (ListDataParameter != null && ListDataParameter.Count > 0)
            {
                SetParameter(ListDataParameter, dbCommand);
            }
            dbCommand.Connection = dbConnection;
            //dbCommand.CommandTimeout = 0;
            Command = dbCommand;
            return dbCommand;
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        private DbConnection CreateConnection()
        {
            try
            {
                DbConnection connection = this.dbProviderFactory.CreateConnection();
                connection.ConnectionString = this.connectionName;
                return connection;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //设置参数
        private void SetParameter(IList<DbParameter> ListDataParameter, DbCommand dbCommand)
        {
            if (ListDataParameter != null && ListDataParameter.Count > 0)
            {
                foreach (DbParameter dbParameter in ListDataParameter)
                {
                    if ((dbParameter.Direction == ParameterDirection.InputOutput || dbParameter.Direction == ParameterDirection.Input) &&
                       (dbParameter.Value == null))
                    {
                        dbParameter.Value = DBNull.Value;
                    }
                    dbCommand.Parameters.Add(dbParameter);
                }
            }
        }

        public void AddParameter(IList<DbParameter> ListDataParameter, string ParameterName, DbType dbType, object value)
        {
            AddParameter(ListDataParameter, ParameterName, dbType, value, 0, ParameterDirection.Input, true, string.Empty, DataRowVersion.Default);
        }

        public void AddParameter(IList<DbParameter> ListDataParameter, string parameterName, DbType dbType, object value, int size, ParameterDirection direction, bool nullable, string sourceColumn, DataRowVersion sourceVersion)
        {
            DbParameter parame = CreateParameter(parameterName, dbType, size, direction, nullable, sourceColumn, sourceVersion, value);
            ListDataParameter.Add(parame);
        }

        private DbParameter CreateParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, bool nullable, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter param = this.dbProviderFactory.CreateParameter();
            ConfigureParameter(param, parameterName, dbType, size, direction, nullable, sourceColumn, sourceVersion, value);
            return param;
        }

        private void ConfigureParameter(DbParameter param, string ParameterName, DbType dbType, int size, ParameterDirection direction, bool nullable, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.ParameterName = ParameterName;
            param.DbType = dbType;
            param.Size = size;
            param.Value = (value == null) ? DBNull.Value : value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        protected void ConvertParameters<T>(T model, ref IList<DbParameter> parameters) where T : class, IEntity, new()
        {
            int len = Entity<T>.ColumnsPropertyWithOutReadony.Length;
            if (len == 0)
            {
                return;
            }
            parameters = new List<DbParameter>();
            for (int i = 0; i < len; i++)
            {
                DbParameter parm = new SqlParameter();
                parm.Value = Entity<T>.ColumnsPropertyWithOutReadony[i].GetValue(model, null) != null ? Entity<T>.ColumnsPropertyWithOutReadony[i].GetValue(model, null) : DBNull.Value;
                parm.ParameterName = Entity<T>.Properties[Entity<T>.ColumnsPropertyWithOutReadony[i].Name].ParameterName;
                parm.DbType = Entity<T>.Properties[Entity<T>.ColumnsPropertyWithOutReadony[i].Name].ColumnType;
                parameters.Add(parm);
            }
        }

        protected void ConvertBatchParameters<T>(T model, ref DbParameter[] parameters, ref StringBuilder scriptBlock, int index)
        {
            if (index >= 1) scriptBlock.AppendLine(" UNION ALL ");
            parameters = new DbParameter[Entity<T>.ColumnsPropertyWithOutReadony.Length];
            DbParameter parm = null;
            for (int i = 0; i < Entity<T>.ColumnsPropertyWithOutReadony.Length; i++)
            {
                parm = new SqlParameter();
                parm.Value = Entity<T>.ColumnsPropertyWithOutReadony[i].GetValue(model, null) != null ? Entity<T>.ColumnsPropertyWithOutReadony[i].GetValue(model, null) : DBNull.Value;
                parm.ParameterName = Entity<T>.Properties[Entity<T>.ColumnsPropertyWithOutReadony[i].Name].ParameterName + index.ToString();
                parm.DbType = Entity<T>.Properties[Entity<T>.ColumnsPropertyWithOutReadony[i].Name].ColumnType;
                parameters[i] = parm;
            }
            scriptBlock.AppendLine(string.Format("SELECT {0}", string.Join(",", parameters.Select(s => s.ParameterName))));
        }

        protected static string checkScriptWord = "drop|truncate";
        protected static Regex regex = new Regex(checkScriptWord, RegexOptions.IgnoreCase);
        protected virtual void CheckScript(string script)
        {
            if (regex.IsMatch(script))
            {
                string text = "T-SQL WARNING " + checkScriptWord;
                Exception ex = new Exception(text);
                throw ex;
            }
        }

        /// <summary>
        /// 获取分页SQL脚本
        /// </summary>
        /// <param name="script">语句</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="size">页容量</param>
        /// <param name="index">当前页</param>
        /// <returns></returns>
        private const string PAGE_SCRIPT_BLOCK = @" 
        WITH t AS (SELECT ROW_NUMBER() OVER(ORDER BY {0}) AS row_number, * FROM ({1}) AS a  {2}) 
        SELECT * from t WHERE row_number BETWEEN {3} AND {4}";
        public static string PageScript(string script, string condition, string orderby, int index, int size)
        {
            PageEntity pInfo = new PageEntity(size, index);
            string sqlStatement = string.Format(PAGE_SCRIPT_BLOCK,
                                       orderby,
                                       script,
                                       "",
                                       pInfo.SetStart.ToString(),
                                       pInfo.SetEnd.ToString()
                                       );
            return sqlStatement;
        }

        #endregion

        public virtual int Insert<T>(T model) where T : class, IEntity, new()
        {
            throw new Exception("BaseDatabase.Add<T>(T model)方法未实现");
        }

        public virtual int Insert<T>(IEnumerable<T> list) where T : class, IEntity, new()
        {
            throw new NotImplementedException();
        }

        public virtual object InsertIdentity<T>(T model) where T : class, IEntity, new()
        {
            throw new NotImplementedException();
        }

        public virtual int Delete<T>(System.Linq.Expressions.Expression<Predicate<T>> where) where T : class, IEntity, new()
        {
            throw new NotImplementedException();
        }

        public virtual int Update<T>(T model, System.Linq.Expressions.Expression<Predicate<T>> where) where T : class, IEntity, new()
        {
            throw new NotImplementedException();
        }

        public virtual int Update<T>(System.Linq.Expressions.Expression<Predicate<T>> columns, System.Linq.Expressions.Expression<Predicate<T>> where) where T : class, IEntity, new()
        {
            throw new NotImplementedException();
        }

        public virtual DataTable GetPageList(string script, string condition, string orderby, int index, int size)
        {
            throw new NotImplementedException("接口未实现");
        }

        public virtual IEnumerable<T> GetPageList<T>(string script, string condition, string orderby, int index, int size)
        {
            throw new NotImplementedException("接口未实现");
        }


    }


}
