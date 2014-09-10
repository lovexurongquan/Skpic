/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：attribute for foreign key.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;

namespace Skpic.Async.Attributes
{
    /// <summary>
    /// foreign key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : System.Attribute
    {
        /// <summary>
        /// ForeignKey
        /// </summary>
        /// <param name="name">ForeignKey name</param>
        public ForeignKeyAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
    }

}