/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：attribute for foreign table.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;

namespace Skpic.Async.Attributes
{
    /// <summary>
    /// foreign table
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignTableAttribute : System.Attribute
    {
        /// <summary>
        /// foreign table
        /// </summary>
        /// <param name="name">foreign table name</param>
        public ForeignTableAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// foreign table name
        /// </summary>
        public string Name { get; private set; }
    }
}