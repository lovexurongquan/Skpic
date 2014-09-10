/*
 * added by laoxu 2014-9-6 17:00:00
 * ---------------------------------------------------------------
 * for: A Unit of Work keeps track of everything you do during a 
 * business transaction that can affect the database. 
 * When you're done, it figures out everything that needs to be done 
 * to alter the database as a result of your work.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 **/

using Skpic.Common;

namespace Skpic.IDataAccessLayer
{
    /// <summary>
    /// A Unit of Work keeps track of everything you do during a business
    /// transaction that can affect the database. When you're done, 
    /// it figures out everything that needs to be done to alter the database
    /// as a result of your work.
    /// </summary>
    public interface IUnitWork
    {
        /// <summary>
        /// commit the unit work
        /// </summary>
        bool Commit();

        /// <summary>
        /// rollback the unit work
        /// </summary>
        void Rollback();

        /// <summary>
        /// Regist the entity's to the Queue
        /// when the commit method called,all the Queue's 
        /// entity will be mapped to sql
        /// </summary>
        /// <typeparam name="T">the type of entity</typeparam>
        /// <param name="entity">the entity to be regist</param>
        /// <param name="state">to add or delete or modified the entity</param>
        /// <returns>the key of commit result</returns>
        void RegistEntity<T>(T entity, EntityState state) where T : class;

    }
}
