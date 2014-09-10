/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：attribute for table.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;

namespace Skpic.Async.Attributes
{

    /// <summary>
    /// table attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : System.Attribute
    {
        /// <summary>
        /// table attribute
        /// </summary>
        /// <param name="tableName"></param>
        public TableAttribute(string tableName)
        {
            Name = tableName;
        }

        /// <summary>
        /// name
        /// </summary>
        public string Name { get;private set; }
    }
}