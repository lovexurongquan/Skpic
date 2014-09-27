/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for£ºSql server adapter.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Skpic.Async.Adapter
{
    /// <summary>
    /// sqlserver adapter
    /// </summary>
    public class SqlServerAdapter : ISqlAdapter
    {
        public T Insert<T>(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, T entityToInsert) where T : class
        {
            string cmd = String.Format("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);

            connection.Execute(cmd, entityToInsert, transaction, commandTimeout);

            //NOTE: would prefer to use IDENT_CURRENT('tablename') or IDENT_SCOPE but these are not available on SQLCE
            var r = connection.Query("select @@IDENTITY id", transaction: transaction, commandTimeout: commandTimeout);
            var id = r.First().id;
            var propertyInfos = keyProperties as IList<PropertyInfo> ?? keyProperties.ToList();
            if (propertyInfos.Any())
                propertyInfos.First().SetValue(entityToInsert, id, null);
            return entityToInsert;
        }
    }
}