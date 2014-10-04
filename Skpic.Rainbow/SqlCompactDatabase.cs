using System;
using System.Data.Common;
using System.Linq;

namespace Skpic.Rainbow
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TDatabase"></typeparam>
    public abstract class SqlCompactDatabase<TDatabase> : Database<TDatabase> where TDatabase : Database<TDatabase>, new()
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class SqlCompactTable<T> : Table<T>
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="database"></param>
            /// <param name="likelyTableName"></param>
            public SqlCompactTable(Database<TDatabase> database, string likelyTableName)
                : base(database, likelyTableName)
            {
            }

            /// <summary>
            /// Insert a row into the db
            /// </summary>
            /// <param name="data">Either DynamicParameters or an anonymous type or concrete type</param>
            /// <returns></returns>
            public override int? Insert(dynamic data)
            {
                var o = (object)data;
                var paramNames = GetParamNames(o);
                paramNames.Remove("Id");

                var cols = string.Join(",", paramNames);
                var colsParams = string.Join(",", paramNames.Select(p => "@" + p));

                var sql = "insert " + TableName + " (" + cols + ") values (" + colsParams + ")";
                if (Database.Execute(sql, o) != 1)
                {
                    return null;
                }

                return (int)Database.Query<decimal>("SELECT @@IDENTITY AS LastInsertedId").Single();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static TDatabase Init(DbConnection connection)
        {
            var db = new TDatabase();
            db.InitDatabase(connection, 0);
            return db;
        }

        internal override Action<TDatabase> CreateTableConstructorForTable()
        {
            return CreateTableConstructor(typeof(SqlCompactTable<>));
        }
    }
}