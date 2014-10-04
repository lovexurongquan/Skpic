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
using System.Linq;
using System.Linq.Expressions;
using Skpic.Async;
using Skpic.Factory;
using Skpic.IDataAccessLayer;

namespace Skpic.DataAccessLayer
{
    public class BasicData<T> : UnitWork, IBasicData where T : class
    {
        public BasicData(string connStringName = "ConnectionString")
            : base(connStringName)
        {

        }

        /// <summary>
        /// query by primary key.
        /// </summary>
        /// <param name="id">primary key value.</param>
        /// <returns></returns>
        public T FirstOrDefault(string id)
        {
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.QueryByKey<T>(id);
            }
        }

        /// <summary>
        /// query by primary key.
        /// </summary>
        /// <param name="id">primary key value.</param>
        /// <returns></returns>
        public T FirstOrDefault(int id)
        {
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.QueryByKey<T>(id);
            }
        }

        /// <summary>
        /// query model by expression.
        /// </summary>
        /// <param name="where">where expression.</param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> where)
        {
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.Query(where).FirstOrDefault();
            }
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> where)
        {
            using (var con = DbContextFactory.GetConnection(ConnectionStringName))
            {
                return con.Query(where);
            }
        }
    }
}