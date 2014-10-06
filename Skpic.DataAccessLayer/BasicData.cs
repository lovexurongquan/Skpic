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
using System.Linq.Expressions;
using System.Text;
using Skpic.Async;
using Skpic.Factory;
using Skpic.IDataAccessLayer;

namespace Skpic.DataAccessLayer
{
    public class BasicData<TSource> : UnitWork, IBasicData<TSource> where TSource : class
    {
        public BasicData(string connStringName = "ConnectionString")
            : base(connStringName)
        {
            _sqlBuilder=new StringBuilder();
        }

        private StringBuilder _sqlBuilder;

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
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        public IBasicData<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns></returns>
        public IBasicData<TSource> Skip(int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <param name="count">The number of elements to return.</param>
        /// <returns></returns>
        public IBasicData<TSource> Take(int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Returns distinct elements from a sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns></returns>
        public IBasicData<TSource> Distinct<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns></returns>
        public IEnumerable<TResult> Select<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
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