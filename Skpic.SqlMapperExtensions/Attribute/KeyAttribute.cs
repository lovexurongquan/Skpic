using System;

namespace Skpic.SqlMapperExtensions
{

    // do not want to depend on data annotations that is not in client profile
    /// <summary>
    /// key attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
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