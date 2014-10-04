/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：attribute for write.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;

namespace Skpic.Async.Attributes
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WriteAttribute : System.Attribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="write"></param>
        public WriteAttribute(bool write)
        {
            Write = write;
        }

        /// <summary>
        ///
        /// </summary>
        public bool Write { get; private set; }
    }
}