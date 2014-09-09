using System;

namespace Skpic.SqlMapperExtensions
{

    /// <summary>
    /// table attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
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