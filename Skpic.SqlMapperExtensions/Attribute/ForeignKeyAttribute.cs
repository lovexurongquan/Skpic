using System;

namespace Skpic.SqlMapperExtensions
{


    /// <summary>
    /// foreign key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : Attribute
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