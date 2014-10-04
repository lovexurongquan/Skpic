/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：attribute for key.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;

namespace Skpic.Async.Attributes
{
    // do not want to depend on data annotations that is not in client profile
    /// <summary>
    /// key attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : System.Attribute
    {
        /// <summary>
        /// set key is identity
        /// </summary>
        public KeyAttribute(string isIdentity)
        {
            IsIdentity = isIdentity;
        }

        /// <summary>
        /// identity flag.
        /// </summary>
        public string IsIdentity { get; private set; }
    }
}