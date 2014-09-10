/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for£ºSQLite adapter.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Skpic.Async;

namespace Skpic.SqlMapperExtensions
{
    /// <summary>
    /// SQLite adapter
    /// </summary>
    public class SqLiteAdapter : ISqlAdapter
    {
        public T Insert<T>(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, T entityToInsert) where T : class
        {
            string cmd = String.Format("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);

            connection.Execute(cmd, entityToInsert, transaction, commandTimeout);

            var r = connection.Query("select last_insert_rowid() id", transaction: transaction, commandTimeout: commandTimeout);
            var id = (int)r.First().id;
            var propertyInfos = keyProperties as IList<PropertyInfo> ?? keyProperties.ToList();
            if (propertyInfos.Any())
                propertyInfos.First().SetValue(entityToInsert, id, null);
            return entityToInsert;
        }
    }
}