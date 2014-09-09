using System;

namespace Skpic.SqlMapperExtensions
{

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WriteAttribute : Attribute
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