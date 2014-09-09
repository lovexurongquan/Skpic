using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Skpic.SqlMapperExtensions
{
    /// <summary>
    /// sql adapter interface
    /// </summary>
    public interface ISqlAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="tableName"></param>
        /// <param name="columnList"></param>
        /// <param name="parameterList"></param>
        /// <param name="keyProperties"></param>
        /// <param name="entityToInsert"></param>
        /// <returns></returns>
        T Insert<T>(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, T entityToInsert) where T : class;
    }
}