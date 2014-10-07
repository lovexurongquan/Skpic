/*
 * Added by laoxu 2014-10-04 22:00:00
 * ---------------------------------------------------------------
 * for：basic data.
 * this the data access basic class. you can use this to query and all the UnitWork method.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Skpic.Async;
using Skpic.Common;
using Skpic.Factory;
using Skpic.IDataAccessLayer;

namespace Skpic.DataAccessLayer
{
    public class BasicData<TSource> : UnitWork, IBasicData<TSource> where TSource : class
    {
        public BasicData(string connStringName = "ConnectionString")
            : base(connStringName)
        {
            _sqlDictionary = new Dictionary<SqlType, string>();
            _paramDictionary = new Dictionary<string, string>();
            _helper = new LambdaHelper<TSource>();
        }

        /// <summary>
        /// sql builder.
        /// </summary>
        private readonly Dictionary<SqlType, string> _sqlDictionary;
        //private StringBuilder whereBuilder;

        /// <summary>
        /// param collection.
        /// </summary>
        private Dictionary<string, string> _paramDictionary;

        /// <summary>
        /// lambda helper.
        /// </summary>
        private readonly LambdaHelper<TSource> _helper;
        /// <summary>
        /// query by primary key.
        /// </summary>
        /// <param name="id">primary key value.</param>
        /// <returns></returns>
        public TSource FirstOrDefault(string id)
        {
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.Single<TSource>(id);
            }
        }

        /// <summary>
        /// query by primary key.
        /// </summary>
        /// <param name="id">primary key value.</param>
        /// <returns></returns>
        public TSource FirstOrDefault(int id)
        {
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.Single<TSource>(id);
            }
        }

        /// <summary>
        /// query model by expression.
        /// </summary>
        /// <param name="predicate">where expression.</param>
        /// <returns></returns>
        public TSource Single(Expression<Func<TSource, bool>> predicate)
        {
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.Single(predicate);
            }
        }

        /// <summary>
        /// query model by expression.
        /// </summary>
        /// <returns></returns>
        public TSource Single()
        {
            return null;
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        public IBasicData<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            _helper.Init(predicate);
            var whereSql = _helper.GetWhereSql();

            _paramDictionary = _helper.GetParameterDict();
            if (_sqlDictionary.ContainsKey(SqlType.Where))
            {
                _sqlDictionary[SqlType.Where] = whereSql;
            }
            else
            {
                _sqlDictionary.Add(SqlType.Where, whereSql);
            }

            return this;
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns></returns>
        public IBasicData<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            _helper.Init(keySelector, false);
            if (_sqlDictionary.ContainsKey(SqlType.Order))
            {
                _sqlDictionary[SqlType.Order] = _helper.GetOrderSql();
            }
            else
            {
                _sqlDictionary.Add(SqlType.Order, _helper.GetOrderSql());
            }

            return this;
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns></returns>
        public IBasicData<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            _helper.Init(keySelector, true);
            if (_sqlDictionary.ContainsKey(SqlType.Order))
            {
                _sqlDictionary[SqlType.Order] = _helper.GetOrderSql();
            }
            else
            {
                _sqlDictionary.Add(SqlType.Order, _helper.GetOrderSql());
            }

            return this;
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns></returns>
        public IBasicData<TSource> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            _helper.Init(keySelector, SqlType.Group);
            if (_sqlDictionary.ContainsKey(SqlType.Group))
            {
                _sqlDictionary[SqlType.Group] = _helper.GetGroupSql();
            }
            else
            {
                _sqlDictionary.Add(SqlType.Group, _helper.GetGroupSql());
            }

            return this;
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns></returns>
        public IBasicData<TSource> Skip(int count)
        {
            _sqlDictionary.Add(SqlType.Skip, count.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <param name="count">The number of elements to return.</param>
        /// <returns></returns>
        public IBasicData<TSource> Take(int count)
        {
            _sqlDictionary.Add(SqlType.Take, count.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        ///  Returns distinct elements from a sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <returns></returns>
        public IBasicData<TSource> Distinct()
        {
            _sqlDictionary.Add(SqlType.Distinct, SqlType.Distinct.ToString());
            return this;
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <returns></returns>
        public IEnumerable<TSource> Select()
        {
            if (_sqlDictionary.ContainsKey(SqlType.Group))
            {
                throw new Exception("Please select the corresponding keys in the specified group after.");
            }
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.Query<TSource>(_sqlDictionary, _paramDictionary);
            }
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
        /// <param name="keySelector">A transform function to apply to each element.</param>
        /// <returns></returns>
        public IEnumerable<TResult> Select<TResult>(Expression<Func<TSource, TResult>> keySelector) where TResult : class 
        {
            _helper.Init(keySelector, SqlType.Select);
            if (_sqlDictionary.ContainsKey(SqlType.Select))
            {
                _sqlDictionary[SqlType.Select] = _helper.GetSelectSql();
            }
            else
            {
                _sqlDictionary.Add(SqlType.Select, _helper.GetSelectSql());
            }

            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.Query<TResult>(_sqlDictionary, _paramDictionary);
            }
        }

        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// query by lambda.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        public IEnumerable<TSource> Query(Expression<Func<TSource, bool>> predicate)
        {
            throw new NotImplementedException();
        }

    }
}