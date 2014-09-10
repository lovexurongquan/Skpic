/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：all table in database.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

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