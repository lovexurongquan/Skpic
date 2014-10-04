using System;

namespace Skpic.Rainbow
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IgnorePropertyAttribute : Attribute
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ignore"></param>
        public IgnorePropertyAttribute(bool ignore)
        {
            Value = ignore;
        }

        /// <summary>
        ///
        /// </summary>
        public bool Value { get; set; }
    }
}