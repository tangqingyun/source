using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Basement.Framework.Data
{
    public interface IDatabase
    {
        int Insert<T>(T model) where T : class, IEntity, new();
        int Insert<T>(IEnumerable<T> list) where T : class, IEntity, new();
        object InsertIdentity<T>(T model) where T : class, IEntity, new();
        int Delete<T>(Expression<Predicate<T>> where) where T : class, IEntity, new();
        int Update<T>(T model, Expression<Predicate<T>> where) where T : class, IEntity, new();
        int Update<T>(Expression<Predicate<T>> columns, Expression<Predicate<T>> where) where T : class, IEntity, new();

        DataTable Single(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text);
        T SingleAsModel<T>(string sql, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text) where T : new();
        IEnumerable<T> ExecuteList<T>(string script, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text);
        object ExecuteScalar(string script, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text);
        R ExecuteScalar<R>(string script, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text);
        bool ExecuteBool(string script, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text);
        int ExecuteNonQuery(string script, IList<DbParameter> listParms, CommandType commandType = CommandType.Text);
        bool ExecuteNoQuery(string script, IList<DbParameter> listParms, CommandType commandType = CommandType.Text);
        DataSet GetDataSet(string script, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text);
        DataTable GetDataTable(string script, IList<DbParameter> listParms = null, CommandType commandType = CommandType.Text);
        bool ExecuteTransation(IDictionary<string, IList<DbParameter>> dic, CommandType commandType = CommandType.Text);
        DataTable GetPageList(string script, string condition, string orderby, int index, int size);
        IEnumerable<T> GetPageList<T>(string script, string condition, string orderby, int index, int size);

        ArrayList ExecuteList<T1, T2>(string script, IList<DbParameter> parameters, CommandType commandType = CommandType.Text);
        ArrayList ExecuteList<T1, T2, T3>(string script, IList<DbParameter> parameters, CommandType commandType = CommandType.Text);
        ArrayList ExecuteList<T1, T2, T3, T4>(string script, IList<DbParameter> parameters, CommandType commandType = CommandType.Text);
    }
}
