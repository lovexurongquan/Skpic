using Skpic.Async;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Skpic.Rainbow
{
    /// <summary>
    /// A container for a database, assumes all the tables have an Id column named Id
    /// </summary>
    /// <typeparam name="TDatabase"></typeparam>
    public abstract class Database<TDatabase> : IDisposable where TDatabase : Database<TDatabase>, new()
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        public class Table<T, TId>
        {
            internal Database<TDatabase> Database;
            internal string tableName;
            internal string LikelyTableName;

            /// <summary>
            ///
            /// </summary>
            /// <param name="database"></param>
            /// <param name="likelyTableName"></param>
            public Table(Database<TDatabase> database, string likelyTableName)
            {
                Database = database;
                LikelyTableName = likelyTableName;
            }

            /// <summary>
            ///
            /// </summary>
            public string TableName
            {
                get
                {
                    tableName = tableName ?? Database.DetermineTableName<T>(LikelyTableName);
                    return tableName;
                }
            }

            /// <summary>
            /// Insert a row into the db
            /// </summary>
            /// <param name="data">Either DynamicParameters or an anonymous type or concrete type</param>
            /// <returns></returns>
            public virtual int? Insert(dynamic data)
            {
                var o = (object)data;
                var paramNames = GetParamNames(o);
                paramNames.Remove("Id");

                var cols = string.Join(",", paramNames);
                var colsParams = string.Join(",", paramNames.Select(p => "@" + p));
                var sql = "set nocount on insert " + TableName + " (" + cols + ") values (" + colsParams + ") select cast(scope_identity() as int)";

                return Database.Query<int?>(sql, o).Single();
            }

            /// <summary>
            /// Update a record in the DB
            /// </summary>
            /// <param name="id"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            public int Update(TId id, dynamic data)
            {
                List<string> paramNames = GetParamNames((object)data);

                var builder = new StringBuilder();
                builder.Append("update ").Append(TableName).Append(" set ");
                builder.AppendLine(string.Join(",", paramNames.Where(n => n != "Id").Select(p => p + "= @" + p)));
                builder.Append("where Id = @Id");

                var parameters = new DynamicParameters(data);
                parameters.Add("Id", id);

                return Database.Execute(builder.ToString(), parameters);
            }

            /// <summary>
            /// Delete a record for the DB
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool Delete(TId id)
            {
                return Database.Execute("delete from " + TableName + " where Id = @id", new { id }) > 0;
            }

            /// <summary>
            /// Grab a record with a particular Id from the DB
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public T Get(TId id)
            {
                return Database.Query<T>("select * from " + TableName + " where Id = @id", new { id }).FirstOrDefault();
            }

            /// <summary>
            /// get first model.
            /// </summary>
            /// <returns></returns>
            public virtual T First()
            {
                return Database.Query<T>("select top 1 * from " + TableName).FirstOrDefault();
            }

            /// <summary>
            /// get all model.
            /// </summary>
            /// <returns></returns>
            public IEnumerable<T> All()
            {
                return Database.Query<T>("select * from " + TableName);
            }

            private static ConcurrentDictionary<Type, List<string>> paramNameCache = new ConcurrentDictionary<Type, List<string>>();

            internal static List<string> GetParamNames(object o)
            {
                if (o is DynamicParameters)
                {
                    return (o as DynamicParameters).ParameterNames.ToList();
                }

                List<string> paramNames;
                if (!paramNameCache.TryGetValue(o.GetType(), out paramNames))
                {
                    paramNames = new List<string>();
                    foreach (var prop in o.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public))
                    {
                        var attribs = prop.GetCustomAttributes(typeof(IgnorePropertyAttribute), true);
                        var attr = attribs.FirstOrDefault() as IgnorePropertyAttribute;
                        if (attr == null || (!attr.Value))
                        {
                            paramNames.Add(prop.Name);
                        }
                    }
                    paramNameCache[o.GetType()] = paramNames;
                }
                return paramNames;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Table<T> : Table<T, int>
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="database"></param>
            /// <param name="likelyTableName"></param>
            public Table(Database<TDatabase> database, string likelyTableName)
                : base(database, likelyTableName)
            {
            }
        }

        private DbConnection _connection;
        private int _commandTimeout;
        private DbTransaction _transaction;

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static TDatabase Init(DbConnection connection, int commandTimeout)
        {
            var db = new TDatabase();
            db.InitDatabase(connection, commandTimeout);
            return db;
        }

        internal static Action<TDatabase> TableConstructor;

        internal void InitDatabase(DbConnection connection, int commandTimeout)
        {
            _connection = connection;
            _commandTimeout = commandTimeout;
            if (TableConstructor == null)
            {
                TableConstructor = CreateTableConstructorForTable();
            }

            TableConstructor(this as TDatabase);
        }

        internal virtual Action<TDatabase> CreateTableConstructorForTable()
        {
            return CreateTableConstructor(typeof(Table<>));
        }

        /// <summary>
        /// begin transaction.
        /// </summary>
        /// <param name="isolation"></param>
        public void BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            _transaction = _connection.BeginTransaction(isolation);
        }

        /// <summary>
        /// commit transaction.
        /// </summary>
        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// roll back transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tableType"></param>
        /// <returns></returns>
        protected Action<TDatabase> CreateTableConstructor(Type tableType)
        {
            var dm = new DynamicMethod("ConstructInstances", null, new[] { typeof(TDatabase) }, true);
            var il = dm.GetILGenerator();

            var setters = GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == tableType)
                .Select(p => Tuple.Create(
                        p.GetSetMethod(true),
                        p.PropertyType.GetConstructor(new[] { typeof(TDatabase), typeof(string) }),
                        p.Name,
                        p.DeclaringType
                 ));

            foreach (var setter in setters)
            {
                il.Emit(OpCodes.Ldarg_0);
                // [db]
                il.Emit(OpCodes.Ldstr, setter.Item3);
                // [db, likelyname]
                il.Emit(OpCodes.Newobj, setter.Item2);
                // [table]
                if (setter.Item2.DeclaringType != null)
                {
                    var table = il.DeclareLocal(setter.Item2.DeclaringType);

                    il.Emit(OpCodes.Stloc, table);
                    // []
                    il.Emit(OpCodes.Ldarg_0);
                    // [db]
                    il.Emit(OpCodes.Castclass, setter.Item4);
                    // [db cast to container]
                    il.Emit(OpCodes.Ldloc, table);
                }
                // [db cast to container, table]
                il.Emit(OpCodes.Callvirt, setter.Item1);
                // []
            }

            il.Emit(OpCodes.Ret);
            return (Action<TDatabase>)dm.CreateDelegate(typeof(Action<TDatabase>));
        }

        private static ConcurrentDictionary<Type, string> tableNameMap = new ConcurrentDictionary<Type, string>();

        private string DetermineTableName<T>(string likelyTableName)
        {
            string name;

            if (!tableNameMap.TryGetValue(typeof(T), out name))
            {
                name = likelyTableName;
                if (!TableExists(name))
                {
                    name = "[" + typeof(T).Name + "]";
                }

                tableNameMap[typeof(T)] = name;
            }
            return name;
        }

        private bool TableExists(string name)
        {
            string schemaName = null;

            name = name.Replace("[", "");
            name = name.Replace("]", "");

            if (name.Contains("."))
            {
                var parts = name.Split('.');
                if (parts.Count() == 2)
                {
                    schemaName = parts[0];
                    name = parts[1];
                }
            }

            var builder = new StringBuilder("select 1 from INFORMATION_SCHEMA.TABLES where ");
            if (!String.IsNullOrEmpty(schemaName)) builder.Append("TABLE_SCHEMA = @schemaName AND ");
            builder.Append("TABLE_NAME = @name");

            return _connection.Query(builder.ToString(), new { schemaName, name }, _transaction).Count() == 1;
        }

        /// <summary>
        /// execute sql text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="param">param array default null.</param>
        /// <returns></returns>
        public int Execute(string sql, dynamic param = null)
        {
            return _connection.Execute(sql, param as object, _transaction, _commandTimeout);
        }

        /// <summary>
        /// query method.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="param">param array default null.</param>
        /// <param name="buffered"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, dynamic param = null, bool buffered = true)
        {
            return _connection.Query<T>(sql, param as object, _transaction, buffered, _commandTimeout);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return _connection.Query(sql, map, param as object, transaction, buffered, splitOn);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return _connection.Query(sql, map, param as object, transaction, buffered, splitOn);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return _connection.Query(sql, map, param as object, transaction, buffered, splitOn);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            return _connection.Query(sql, map, param as object, transaction, buffered, splitOn);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string sql, dynamic param = null, bool buffered = true)
        {
            return _connection.Query(sql, param as object, _transaction, buffered);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryMultiple(_connection, sql, param, transaction, commandTimeout, commandType);
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Closed) return;
            if (_transaction != null)
            {
                _transaction.Rollback();
            }

            _connection.Close();
            _connection = null;
        }
    }
}