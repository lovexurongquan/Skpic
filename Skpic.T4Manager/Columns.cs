/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for£ºall the columns in database.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

namespace Skpic.T4Manager
{
    /// <summary>
    /// table columns in database.
    /// </summary>
    public class Columns
    {
        /// <summary>
        /// column order in table.
        /// </summary>
        public string Colorder { get; set; }

        /// <summary>
        /// table name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// column name in table.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// column description.
        /// </summary>
        public string DeText { get; set; }

        /// <summary>
        /// database type with this column.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// this column length.
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// column preci.
        /// </summary>
        public string Preci { get; set; }

        /// <summary>
        /// cloumn scale.
        /// </summary>
        public string Scale { get; set; }

        /// <summary>
        /// this column is identity. True or False.
        /// </summary>
        public string IsIdentity { get; set; }

        /// <summary>
        /// this column is pramary key. True or False.
        /// </summary>
        public string IsPk { get; set; }

        /// <summary>
        /// is computed into sql text.
        /// </summary>
        public string IsComputed { get; set; }

        /// <summary>
        /// this column is nullable. True or False.
        /// </summary>
        public string IsNull { get; set; }

        /// <summary>
        /// the index name with this column.
        /// </summary>
        public string IndexName { get; set; }
        
        /// <summary>
        /// the index sort.
        /// </summary>
        public string IndexSort { get; set; }

        /// <summary>
        /// this column create time.
        /// </summary>
        public string Create_Date { get; set; }

        /// <summary>
        /// this column modify time.
        /// </summary>
        public string Modify_Date { get; set; }

        /// <summary>
        /// this column defalut value.
        /// </summary>
        public string DefaultVal { get; set; }

        /// <summary>
        /// foreign table name.
        /// </summary>
        public string Rftable { get; set; }

        /// <summary>
        /// foreign key name.
        /// </summary>
        public string Rfcolumn { get; set; }
    }
}