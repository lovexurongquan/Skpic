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

using System;
using System.Collections.Generic;
using System.Data;
using Skpic.Common;
using Skpic.Factory;
using Skpic.IDataAccessLayer;
using Skpic.SqlMapperExtensions;

namespace Skpic.DataAccessLayer
{
    public class UnitWork : IUnitWork
    {
        #region Unit and others

        /// <summary>
        /// PersitenceLayer portal class
        /// </summary>
        public UnitWork(string connStringName = "ConnectionString")
        {
            ConnectionStringName = connStringName;
            _storeQueue = new Queue<QueueData>();
        }

        /// <summary>
        /// Store data temporarily for Queue
        /// </summary>
        private class QueueData
        {
            /// <summary>
            /// entity state, sp : Added, Edit, Delete, Text, StoredProcedure.
            /// </summary>
            public EntityState State { get; set; }

            /// <summary>
            /// Store the entity
            /// </summary>
            public object Entity { get; set; }

            /// <summary>
            /// entity type.
            /// </summary>
            public Type EntityType { get; set; }
        }

        /// <summary>
        /// use this connection string to connect database.
        /// </summary>
        private string ConnectionStringName { get; set; }

        /// <summary>
        /// a private database entity default null
        /// </summary>
        private IDbConnection _db;

        /// <summary>
        /// Store for all the data for save to database in the future.
        /// </summary>
        private readonly Queue<QueueData> _storeQueue;

        /// <summary>
        /// commit the unit work
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            using (_db = DbContextFactory.GetConnection(ConnectionStringName))
            {
                //begin a trancation
                var tran = _db.BeginTransaction();
                try
                {
                    while (_storeQueue.Count != 0)
                    {
                        var data = _storeQueue.Dequeue();
                        if (data == null)
                        {
                            throw new Exception("program is error.");
                        }
                        switch (data.State)
                        {
                            case EntityState.Create:
                                _db.Insert(data.Entity, data.EntityType, tran);
                                break;
                            case EntityState.Modified:
                                _db.Update(data.Entity, data.EntityType, tran);
                                break;
                            case EntityState.Delete:
                                _db.Delete(data.Entity, data.EntityType, tran);
                                break;
                        }
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    //todo: this could logging.
                    return false;
                }
            }
        }

        /// <summary>
        /// rollback the unit work
        /// </summary>
        public void Rollback()
        {
            _storeQueue.Clear();
        }

        /// <summary>
        /// Regist the entity's to the Queue
        /// when the commit method called,all the Queue's 
        /// entity will be mapped to sql
        /// </summary>
        /// <typeparam name="T">the type of entity</typeparam>
        /// <param name="entity">the entity to be regist</param>
        /// <param name="state">to add or delete or modified the entity</param>
        /// <returns>regist success will be true</returns>
        public void RegistEntity<T>(T entity, EntityState state) where T : class
        {
            var data = new QueueData { Entity = entity, State = state, EntityType = typeof(T) };
            _storeQueue.Enqueue(data);
        }

        #endregion
    }
}