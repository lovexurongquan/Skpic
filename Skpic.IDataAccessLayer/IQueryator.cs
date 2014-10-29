/*
 * Added by laoxu 2014-10-04 22:00:00
 * ---------------------------------------------------------------
 * for：basic data.
 * this the data access basic interface. you can use this to query and all the IUnitWork method.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skpic.IDataAccessLayer
{
    public interface IQueryator<TSource> : IUnitWork where TSource : class
    {
        /// <summary>
        /// query by primary key.
        /// </summary>
        /// <param name="id">primary key value.</param>
        /// <returns></returns>
        TSource FirstOrDefault(string id);

        /// <summary>
        /// query by primary key.
        /// </summary>
        /// <param name="id">primary key value.</param>
        /// <returns></returns>
        TSource FirstOrDefault(int id);

        /// <summary>
        /// query model by expression, and returns the only element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <param name="predicate">where expression.</param>
        /// <returns></returns>
        TSource Single(Expression<Func<TSource, bool>> predicate);

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        IQueryator<TSource> Where(Expression<Func<TSource, bool>> predicate);

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns></returns>
        IQueryator<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector);

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns></returns>
        IQueryator<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector);

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns></returns>
        IQueryator<IGrouping<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector);

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns></returns>
        IQueryator<TSource> Skip(int count);

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <param name="count">The number of elements to return.</param>
        /// <returns></returns>
        IQueryator<TSource> Take(int count);

        /// <summary>
        ///  Returns distinct elements from a sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <returns></returns>
        IQueryator<TSource> Distinct();

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <returns></returns>
        IEnumerable<TSource> Select();

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns></returns>
        IEnumerable<TResult> Select<TResult>(Expression<Func<TSource, TResult>> selector) where TResult : class;

        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// query by lambda.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        IEnumerable<TSource> Query(Expression<Func<TSource, bool>> predicate);
    }
}