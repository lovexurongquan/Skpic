/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for£ºpostgre adapter.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Skpic.Async;

namespace Skpic.SqlMapperExtensions
{
    /// <summary>
    /// Postgres adapter
    /// </summary>
    public class PostgresAdapter : ISqlAdapter
    {
        public T Insert<T>(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, T entityToInsert) where T : class
        {
            var sb = new StringBuilder();
            sb.AppendFormat("insert into {0} ({1}) values ({2})", tableName, columnList, parameterList);

            // If no primary key then safe to assume a join table with not too much data to return
            var propertyInfos = keyProperties as IList<PropertyInfo> ?? keyProperties.ToList();
            if (!propertyInfos.Any())
                sb.Append(" RETURNING *");
            else
            {
                sb.Append(" RETURNING ");
                bool first = true;
                foreach (var property in propertyInfos)
                {
                    if (!first)
                        sb.Append(", ");
                    first = false;
                    sb.Append(property.Name);
                }
            }

            var results = connection.Query(sb.ToString(), entityToInsert, transaction, commandTimeout: commandTimeout).ToList();

            // Return the key by assinging the corresponding property in the object - by product is that it supports compound primary keys
            int id = 0;
            foreach (var p in propertyInfos)
            {
                var value = ((IDictionary<string, object>)results.First())[p.Name.ToLower()];
                p.SetValue(entityToInsert, value, null);
                if (id == 0)
                    id = Convert.ToInt32(value);
            }
            return entityToInsert;
        }
    }
}