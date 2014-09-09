using System;

namespace Skpic.SqlMapperExtensions
{
    /// <summary>
    /// foreign table
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignTableAttribute : Attribute
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