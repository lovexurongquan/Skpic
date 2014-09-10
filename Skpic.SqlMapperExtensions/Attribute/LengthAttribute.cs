/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：attribute for length.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;

namespace Skpic.SqlMapperExtensions
{
    /// <summary>
    /// column length
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : Attribute
    {
        /// <summary>
        /// column length
        /// </summary>
        /// <param name="length">column length</param>
        public LengthAttribute(int length)
        {
            Length = length;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Length { get; private set; }
    }
}