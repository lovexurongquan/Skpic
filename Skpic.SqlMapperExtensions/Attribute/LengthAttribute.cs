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