using System;

namespace Skpic.T4Manager
{
    /// <summary>
    /// the table collection in database.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// table name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// table onwer user.
        /// </summary>
        public string Cuser { get; set; }

        /// <summary>
        /// table type.U or V.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// table create time.
        /// </summary>
        public DateTime? Dates { get; set; }

        /// <summary>
        /// table description.
        /// </summary>
        public string Comments { get; set; }
    }
}