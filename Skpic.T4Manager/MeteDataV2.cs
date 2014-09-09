using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Skpic.T4Manager
{
    public class MdFactory
    {
        private static string _masterConnectionString = "";
        private static XmlHelper _sqler = new XmlHelper("MsSql2008");
        private static ISql _sql = (ISql)new MsSql();
        private static readonly Dictionary<string, Database> DbList = new Dictionary<string, Database>();
        private static string _currentDbName = "";
        private static List<string> _dbList;

        static MdFactory()
        {
            if (ConfigurationManager.ConnectionStrings["MetaDataDB"] == null)
                return;
            MdFactory._masterConnectionString = ConfigurationManager.ConnectionStrings["MetaDataDB"].ConnectionString;
        }

        public static void SetSqlType(SqlType sqlType)
        {
            if (ConfigurationManager.ConnectionStrings["MetaData" + (object)sqlType + "DB"] != null)
                MdFactory._masterConnectionString = ConfigurationManager.ConnectionStrings["MetaData" + (object)sqlType + "DB"].ConnectionString;
            MdFactory._sqler = new XmlHelper(((object)sqlType).ToString());
            if (sqlType != SqlType.MsSql2008)
                return;
            MdFactory._sql = (ISql)new MsSql();
        }

        public static void SetConnectionStr(string connectionStr)
        {
            MdFactory._masterConnectionString = connectionStr;
        }

        public static Database SetCurrentDbName(string dbName, bool refresh = false)
        {
            if (refresh)
                MdFactory.DbList.Remove(dbName);
            MdFactory._currentDbName = dbName;
            return MdFactory.GetDataBase();
        }

        public static List<string> GetDbList(bool refresh = false)
        {
            if (MdFactory._dbList == null || MdFactory._dbList.Count == 0 || refresh)
            {
                MdFactory._dbList = new List<string>();
                DataSet dataSet = MdFactory._sql.ExeDs(MdFactory._masterConnectionString, MdFactory._sqler.DataBaseListSql);
                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                        MdFactory._dbList.Add(dataRow[0].ToString());
                }
            }
            return MdFactory._dbList;
        }

        public static Database GetDataBase()
        {
            Database database = (Database)null;
            if (MdFactory.DbList.ContainsKey(MdFactory._currentDbName))
            {
                database = MdFactory.DbList[MdFactory._currentDbName];
            }
            else
            {
                MdFactory.GetDbList(false);
                if (MdFactory._dbList.Contains(MdFactory._currentDbName))
                {
                    database = MdFactory.BindDb();
                    MdFactory.DbList.Add(MdFactory._currentDbName, database);
                }
            }
            return database;
        }

        private static Database BindDb()
        {
            Database database = new Database()
            {
                ConnectionString = MdFactory._masterConnectionString.Replace("master", MdFactory._currentDbName),
                DbName = MdFactory._currentDbName
            };
            List<TableObject> list1 = MdFactory._sql.ExeModeList<TableObject>(database.ConnectionString, MdFactory._sqler.TableObjectSql);
            List<FieldObject> list2 = MdFactory._sql.ExeModeList<FieldObject>(database.ConnectionString, MdFactory._sqler.FieldObjectSql);
            List<ProObject> list3 = MdFactory._sql.ExeModeList<ProObject>(database.ConnectionString, MdFactory._sqler.ProObjectSql);
            List<ParObject> list4 = MdFactory._sql.ExeModeList<ParObject>(database.ConnectionString, MdFactory._sqler.ParObjectSql);
            foreach (TableObject tableObject in list1)
            {
                TableObject table = tableObject;
                if (table.type.Trim() == "U")
                    database.AddTable(table);
                else
                    database.AddView(table);
                foreach (FieldObject field in Enumerable.Where<FieldObject>((IEnumerable<FieldObject>)list2, (Func<FieldObject, bool>)(z => z.TableName == table.name)))
                {
                    field.Table = table;
                    table.Columns.Add(field);
                    if (field.isPK)
                        table.ForeignKeys.Add(field);
                    database.AddField(field);
                }
            }
            foreach (ProObject proObject in list3)
            {
                ProObject pro = proObject;
                database.AddPro(pro);
                foreach (ParObject parObject in Enumerable.Where<ParObject>((IEnumerable<ParObject>)list4, (Func<ParObject, bool>)(z => z.ProName == pro.ProName)))
                {
                    parObject.Pro = pro;
                    pro.Parameters.Add(parObject);
                }
            }
            return database;
        }
    }
    internal interface ISql
    {
        List<T> ExeModeList<T>(string conStr, string cmdStr) where T : ITableBase, new();

        DataSet ExeDs(string conStr, string cmdStr);
    }
    public class MsSql : ISql
    {
        public List<T> ExeModeList<T>(string conStr, string cmdStr) where T : ITableBase, new()
        {
            DateTime now = DateTime.Now;
            List<T> list = new List<T>();
            SqlCommand sqlCommand = new SqlCommand(cmdStr);
            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlCommand.Connection = sqlConnection;
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
            using (IDataReader dataReader = (IDataReader)sqlCommand.ExecuteReader())
            {
                Dictionary<string, int> colNameIndex = Database.GetColNameIndex((IDataRecord)dataReader);
                while (dataReader.Read())
                    list.Add((T)new T().BuildEntity((IDataRecord)dataReader, colNameIndex));
                dataReader.Close();
            }
            sqlConnection.Close();
            return list;
        }

        public DataSet ExeDs(string conStr, string cmdStr)
        {
            DataSet dataSet = new DataSet();
            SqlCommand selectCommand = new SqlCommand(cmdStr);
            SqlConnection sqlConnection = new SqlConnection(conStr);
            selectCommand.Connection = sqlConnection;
            ((DataAdapter)new SqlDataAdapter(selectCommand)).Fill(dataSet);
            return dataSet;
        }
    }
    public static class MetaEx
    {
        public static Dictionary<string, FieldObject> MapField = new Dictionary<string, FieldObject>()
    {
      {
        "PageSize",
        new FieldObject()
        {
          ColumnName = "PageSize",
          TableName = "DataPage",
          TypeName = "int",
          rftable = "dp"
        }
      },
      {
        "PageIndex",
        new FieldObject()
        {
          ColumnName = "PageIndex",
          TableName = "DataPage",
          TypeName = "int",
          rftable = "dp"
        }
      },
      {
        "RowCount",
        new FieldObject()
        {
          ColumnName = "RowCount",
          TableName = "DataPage",
          TypeName = "bigint",
          rftable = "dp"
        }
      }
    };

        static MetaEx()
        {
        }

        public static string DbTypeStrToCs(string dbtype, int? preci, int? scale)
        {
            switch (dbtype.ToLower().Trim())
            {
                case "datetime":
                case "smalldatetime":
                case "date":
                    return "DateTime";
                case "smallint":
                    return "short";
                case "int":
                    return "int";
                case "bigint":
                    return "long";
                case "tinyint":
                    return "byte";
                case "float":
                    return "double";
                case "money":
                case "smallmoney":
                case "decimal":
                    return "decimal";
                case "numeric":
                case "number":
                    int? nullable1 = preci;
                    if ((nullable1.GetValueOrDefault() > 9 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
                    {
                        int? nullable2 = scale;
                        if ((nullable2.GetValueOrDefault() != 0 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                            return "int";
                    }
                    int? nullable3 = preci;
                    if ((nullable3.GetValueOrDefault() > 7 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
                        return "float";
                    int? nullable4 = preci;
                    if ((nullable4.GetValueOrDefault() <= 7 ? 0 : (nullable4.HasValue ? 1 : 0)) != 0)
                    {
                        int? nullable2 = preci;
                        if ((nullable2.GetValueOrDefault() >= 16 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                            return "double";
                    }
                    return "decimal";
                case "bit":
                    return "bool";
                default:
                    return "string";
            }
        }

        public static DbType DbTypeStrToDbType(string dbtype, int? preci, int? scale)
        {
            switch (dbtype.ToLower().Trim())
            {
                case "datetime":
                    return DbType.DateTime;
                case "smallint":
                    return DbType.Int16;
                case "int":
                    return DbType.Int32;
                case "bigint":
                    return DbType.Int64;
                case "tinyint":
                    return DbType.Byte;
                case "float":
                    return DbType.Single;
                case "decimal":
                    return DbType.Decimal;
                case "number":
                    int? nullable1 = preci;
                    if ((nullable1.GetValueOrDefault() > 9 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
                    {
                        int? nullable2 = scale;
                        if ((nullable2.GetValueOrDefault() != 0 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                            return DbType.Int32;
                    }
                    int? nullable3 = preci;
                    if ((nullable3.GetValueOrDefault() > 7 ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
                        return DbType.Single;
                    int? nullable4 = preci;
                    if ((nullable4.GetValueOrDefault() <= 7 ? 0 : (nullable4.HasValue ? 1 : 0)) != 0)
                    {
                        int? nullable2 = preci;
                        if ((nullable2.GetValueOrDefault() >= 16 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                            return DbType.Double;
                    }
                    return DbType.Decimal;
                case "bit":
                    return DbType.Boolean;
                default:
                    return DbType.String;
            }
        }

        public static string DbTypeStrToDbTypeStr(string dbtype, int? preci, int? scale)
        {
            switch (dbtype.ToLower().Trim())
            {
                case "date":
                case "datetime":
                    return "DateTime";
                case "smallint":
                    return "Int16";
                case "int":
                    return "Int32";
                case "bigint":
                    return "Int64";
                case "tinyint":
                    return "Byte";
                case "float":
                    return "Double";
                case "decimal":
                case "number":
                    return "Decimal";
                case "bit":
                    return "Boolean";
                default:
                    return "String";
            }
        }
    }
    public interface ITableBase
    {
        object BuildEntity(IDataRecord dataRecord, Dictionary<string, int> colName);
    }
    [Serializable]
    public class TableObject : ITableBase
    {
        public List<FieldObject> Columns = new List<FieldObject>();
        public List<FieldObject> ForeignKeys = new List<FieldObject>();
        public const string nameField = "name";
        public const string cuserField = "cuser";
        public const string typeField = "type";
        public const string datesField = "dates";
        public const string commentsField = "comments";

        public string name { get; set; }

        public string cuser { get; set; }

        public string type { get; set; }

        public DateTime dates { get; set; }

        public string comments { get; set; }

        public string CommentsSimplified
        {
            get
            {
                int length = this.comments.IndexOf('_');
                if (length <= 0)
                    return this.comments;
                else
                    return this.comments.Substring(0, length);
            }
        }

        public string Prefix
        {
            get
            {
                int length = this.name.IndexOf('_');
                if (length <= 0)
                    return this.name;
                else
                    return this.name.Substring(0, length);
            }
        }

        public string PagedName
        {
            get
            {
                return this.Prefix + "_Get" + this.name.Replace(this.Prefix + "_", "") + "Paged";
            }
        }

        public object BuildEntity(IDataRecord dataRecord, Dictionary<string, int> colName)
        {
            if (colName.ContainsKey("name"))
                this.name = dataRecord.GetString(colName["name"]);
            if (colName.ContainsKey("cuser"))
                this.cuser = dataRecord.GetString(colName["cuser"]);
            if (colName.ContainsKey("type"))
                this.type = dataRecord.GetString(colName["type"]);
            if (colName.ContainsKey("dates"))
                this.dates = dataRecord.GetDateTime(colName["dates"]);
            if (colName.ContainsKey("comments"))
                this.comments = dataRecord.IsDBNull(colName["comments"]) ? (string)null : dataRecord.GetString(colName["comments"]);
            return (object)this;
        }

        public string proName(string pro)
        {
            return this.Prefix + pro + this.name.Replace(this.Prefix + "_", "");
        }
    }
    [Serializable]
    public class ParObject : ITableBase
    {
        public const string ProNameField = "ProName";
        public const string ParNameField = "ParName";
        public const string TypeNameField = "TypeName";
        public const string lengthField = "length";
        public const string scaleField = "scale";
        public const string iscomputedField = "iscomputed";
        public const string isoutparamField = "isoutparam";
        public const string isnullableField = "isnullable";
        public const string precField = "prec";

        public string ProName { get; set; }

        public string ParName { get; set; }

        public string TypeName { get; set; }

        public short length { get; set; }

        public int scale { get; set; }

        public int iscomputed { get; set; }

        public int isoutparam { get; set; }

        public int isnullable { get; set; }

        public short prec { get; set; }

        public ProObject Pro { get; set; }

        public string DbTypeNameStr
        {
            get
            {
                return MetaEx.DbTypeStrToDbTypeStr(this.TypeName, new int?(0), new int?(0));
            }
        }

        public SqlDbType DbTypeName
        {
            get
            {
                return (SqlDbType)Enum.Parse(typeof(SqlDbType), this.TypeName, true);
            }
        }

        public string TypeNameCs
        {
            get
            {
                return MetaEx.DbTypeStrToCs(this.TypeName, new int?((int)this.prec), new int?(this.scale));
            }
        }

        public string ParName2
        {
            get
            {
                return this.ParName.TrimStart(new char[1]
        {
          '@'
        });
            }
        }

        public object BuildEntity(IDataRecord dataRecord, Dictionary<string, int> colName)
        {
            if (colName.ContainsKey("ProName"))
                this.ProName = dataRecord.IsDBNull(colName["ProName"]) ? (string)null : dataRecord.GetString(colName["ProName"]);
            if (colName.ContainsKey("ParName"))
                this.ParName = dataRecord.IsDBNull(colName["ParName"]) ? (string)null : dataRecord.GetString(colName["ParName"]);
            if (colName.ContainsKey("TypeName"))
                this.TypeName = dataRecord.IsDBNull(colName["TypeName"]) ? (string)null : dataRecord.GetString(colName["TypeName"]);
            if (colName.ContainsKey("length"))
                this.length = dataRecord.GetInt16(colName["length"]);
            if (colName.ContainsKey("scale"))
                this.scale = dataRecord.IsDBNull(colName["scale"]) ? 0 : dataRecord.GetInt32(colName["scale"]);
            if (colName.ContainsKey("iscomputed"))
                this.iscomputed = dataRecord.IsDBNull(colName["iscomputed"]) ? 0 : dataRecord.GetInt32(colName["iscomputed"]);
            if (colName.ContainsKey("isoutparam"))
                this.isoutparam = dataRecord.IsDBNull(colName["isoutparam"]) ? 0 : dataRecord.GetInt32(colName["isoutparam"]);
            if (colName.ContainsKey("isnullable"))
                this.isnullable = dataRecord.IsDBNull(colName["isnullable"]) ? 0 : dataRecord.GetInt32(colName["isnullable"]);
            if (colName.ContainsKey("prec"))
                this.prec = dataRecord.IsDBNull(colName["prec"]) ? (short)0 : dataRecord.GetInt16(colName["prec"]);
            return (object)this;
        }

        public string AddParameterStr(string tableName)
        {
            if (this.isoutparam == 1)
            {
                if (MetaEx.MapField.ContainsKey(this.ParName2))
                    return "\tDB.AddParameter(cmd, " + MetaEx.MapField[this.ParName2].TableName + "." + MetaEx.MapField[this.ParName2].ColumnName + "Field, DbType." + MetaEx.MapField[this.ParName2].DbTypeNameStr + ", ParameterDirection.InputOutput, String.Empty, DataRowVersion.Default, " + MetaEx.MapField[this.ParName2].rftable + "." + MetaEx.MapField[this.ParName2].ColumnName + ");\r\n";
                else
                    return "\tDB.AddParameter(cmd, " + tableName + "." + this.ParName2 + "Field, DbType." + this.DbTypeNameStr + ", ParameterDirection.InputOutput, String.Empty, DataRowVersion.Default, item." + this.ParName2 + ");\r\n";
            }
            else if (!MetaEx.MapField.ContainsKey(this.ParName2))
                return "\tDB.AddInParameter(cmd, " + tableName + "." + this.ParName2 + "Field, DbType." + this.DbTypeNameStr + ", item." + this.ParName2 + ");\r\n";
            else
                return "\tDB.AddInParameter(cmd, " + MetaEx.MapField[this.ParName2].TableName + "." + MetaEx.MapField[this.ParName2].ColumnName + "Field, DbType." + MetaEx.MapField[this.ParName2].DbTypeNameStr + ", " + MetaEx.MapField[this.ParName2].rftable + "." + MetaEx.MapField[this.ParName2].ColumnName + ");\r\n";
        }

        public string GetOutParaValueStr(string tableName)
        {
            if (this.isoutparam != 1)
                return "";
            if (MetaEx.MapField.ContainsKey(this.ParName2))
                return "\t" + MetaEx.MapField[this.ParName2].rftable + "." + MetaEx.MapField[this.ParName2].ColumnName + " = (" + MetaEx.MapField[this.ParName2].TypeNameCs + ")DB.GetParameterValue(cmd, " + MetaEx.MapField[this.ParName2].TableName + "." + MetaEx.MapField[this.ParName2].ColumnName + "Field);\r\n";
            else
                return "\titem." + this.ParName2 + " = (" + this.TypeNameCs + ")DB.GetParameterValue(cmd, " + tableName + "." + this.ParName2 + "Field);\r\n";
        }
    }
    [Serializable]
    public class ProObject : ITableBase
    {
        private static readonly Regex FirstStr = new Regex("[\\u4e00-\\u9fa5]\\w+");
        private static readonly Regex TableName = new Regex("\\sdelete\\s+[\\w.\\[\\]]+|\\sinto\\s+[\\w.\\[\\]]+|\\supdate\\s+[\\w.\\[\\]]+|\\sfrom\\s+[\\w.\\[\\]]+", RegexOptions.IgnoreCase);
        private static List<string> _Columns = new List<string>()
    {
      "PageSize",
      "PageIndex",
      "RowCount",
      "PageCount",
      "OrderField"
    };
        public List<ParObject> Parameters = new List<ParObject>();
        public const string ProNameField = "ProName";
        public const string textField = "text";
        public const string colidField = "colid";
        public const string crdateField = "crdate";

        public string ProName { get; set; }

        public string text { get; set; }

        public short colid { get; set; }

        public DateTime crdate { get; set; }

        public string FirstChinese
        {
            get
            {
                return ProObject.FirstStr.Match(this.text).Value;
            }
        }

        public List<string> TableNamePossible
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Capture capture in ProObject.TableName.Matches(this.text))
                {
                    string str1 = capture.Value.Trim().Replace("\t", " ").Replace("\n", " ");
                    int num1 = str1.LastIndexOf('.');
                    if (num1 > 0)
                        str1 = str1.Substring(num1 + 1, str1.Length - num1 - 1);
                    int num2 = str1.LastIndexOf(' ');
                    if (num2 > 0)
                        str1 = str1.Substring(num2 + 1, str1.Length - num2 - 1);
                    string str2 = str1.TrimStart(new char[1]
          {
            '['
          }).TrimEnd(new char[1]
          {
            ']'
          });
                    if (!list.Contains(str2))
                        list.Add(str2);
                }
                return list;
            }
        }

        static ProObject()
        {
        }

        public object BuildEntity(IDataRecord dataRecord, Dictionary<string, int> colName)
        {
            if (colName.ContainsKey("ProName"))
                this.ProName = dataRecord.GetString(colName["ProName"]);
            if (colName.ContainsKey("text"))
                this.text = dataRecord.IsDBNull(colName["text"]) ? (string)null : dataRecord.GetString(colName["text"]);
            if (colName.ContainsKey("colid"))
                this.colid = dataRecord.GetInt16(colName["colid"]);
            if (colName.ContainsKey("crdate"))
                this.crdate = dataRecord.GetDateTime(colName["crdate"]);
            return (object)this;
        }

        public string GetExtMethod(string tableName, List<FieldObject> Columns)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            if (Enumerable.Count<ParObject>((IEnumerable<ParObject>)this.Parameters) > 3)
            {
                foreach (ParObject parObject in this.Parameters)
                {
                    ParObject par = parObject;
                    if (!ProObject._Columns.Contains(par.ParName2) && !Enumerable.Any<FieldObject>((IEnumerable<FieldObject>)Columns, (Func<FieldObject, bool>)(z => z.ColumnName == par.ParName2)) && (!(par.TypeNameCs == "DateTime") || !par.ParName.EndsWith("End")))
                        str2 = str2 + (par.isoutparam == 1 ? "ref " : "") + par.TypeNameCs + " " + par.ParName2 + ",";
                }
            }
            else
            {
                foreach (ParObject parObject in this.Parameters)
                {
                    if (!ProObject._Columns.Contains(parObject.ParName2))
                        str2 = str2 + (parObject.isoutparam == 1 ? "ref " : "") + parObject.TypeNameCs + " " + parObject.ParName2 + ",";
                }
            }
            string str4;
            if (this.ProName.EndsWith("_GetPaged") || Enumerable.Any<ParObject>((IEnumerable<ParObject>)this.Parameters, (Func<ParObject, bool>)(z => z.ParName2 == "PageSize")))
                str4 = str1 + "public " + str3 + "DataSet " + this.ProName + "(" + str2 + "DataPage dp)";
            else if (this.text.IndexOf("from " + tableName, StringComparison.CurrentCultureIgnoreCase) >= 0)
                str4 = str1 + "public " + str3 + "DataSet " + this.ProName + "(" + str2.TrimEnd(new char[1]
        {
          ','
        }) + ")";
            else
                str4 = str1 + "public " + str3 + "long " + this.ProName + "(" + str2 + "SqlTransaction tran = null)";
            return str4;
        }

        public bool ProIsInTable(List<FieldObject> Columns)
        {
            int num = 0;
            foreach (ParObject parObject in this.Parameters)
            {
                ParObject par = parObject;
                if (Enumerable.Any<FieldObject>((IEnumerable<FieldObject>)Columns, (Func<FieldObject, bool>)(z => z.ColumnName == par.ParName2)))
                    ++num;
                if (num > 3)
                    break;
            }
            return num > 2;
        }

        public string GetParSetValueStr(string tableName, List<FieldObject> Columns)
        {
            string str = "";
            foreach (ParObject parObject in this.Parameters)
            {
                ParObject par = parObject;
                if (Enumerable.Any<FieldObject>((IEnumerable<FieldObject>)Columns, (Func<FieldObject, bool>)(z => z.ColumnName == par.ParName2)))
                    str = str + "            cmd.Parameters[\"" + par.ParName + "\"].Value = " + par.ParName2 + ";\r\n";
                else if (!ProObject._Columns.Contains(par.ParName2))
                    str = str + "            cmd.Parameters[\"" + par.ParName + "\"].Value = " + par.ParName2 + ";\r\n";
            }
            return str;
        }
    }
    [Serializable]
    public class FieldObject : ITableBase
    {
        public const string colorderField = "colorder";
        public const string TableNameField = "TableName";
        public const string ColumnNameField = "ColumnName";
        public const string TypeNameField = "TypeName";
        public const string LengthField = "Length";
        public const string PreciField = "Preci";
        public const string IsIdentityField = "IsIdentity";
        public const string isPKField = "isPK";
        public const string IndexNameField = "IndexName";
        public const string IndexSortField = "IndexSort";
        public const string defaultValField = "defaultVal";
        public const string rftableField = "rftable";
        public const string rfcolumnField = "rfcolumn";
        public const string ScaleField = "Scale";
        public const string isComputedField = "isComputed";
        public const string isNullField = "isNull";
        public const string Create_DateField = "Create_Date";
        public const string Modify_DateField = "Modify_Date";
        public const string deTextField = "deText";

        public int colorder { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string TypeName { get; set; }

        public short Length { get; set; }

        public byte Preci { get; set; }

        public bool IsIdentity { get; set; }

        public bool isPK { get; set; }

        public string IndexName { get; set; }

        public string IndexSort { get; set; }

        public string defaultVal { get; set; }

        public string rftable { get; set; }

        public string rfcolumn { get; set; }

        public byte Scale { get; set; }

        public bool isComputed { get; set; }

        public bool isNull { get; set; }

        public DateTime Create_Date { get; set; }

        public DateTime Modify_Date { get; set; }

        public string deText { get; set; }

        public TableObject Table { get; set; }

        public string DbTypeNameStr
        {
            get
            {
                return MetaEx.DbTypeStrToDbTypeStr(this.TypeName, new int?((int)this.Preci), new int?((int)this.Scale));
            }
        }

        public SqlDbType DbTypeName
        {
            get
            {
                return (SqlDbType)Enum.Parse(typeof(SqlDbType), this.TypeName, true);
            }
        }

        public string TypeNameCs
        {
            get
            {
                return MetaEx.DbTypeStrToCs(this.TypeName, new int?((int)this.Preci), new int?((int)this.Scale));
            }
        }

        public string DeTextSimplified
        {
            get
            {
                int length = this.deText.IndexOf('_');
                if (length <= 0)
                    return this.deText;
                else
                    return this.deText.Substring(0, length);
            }
        }

        public string ParStr
        {
            get
            {
                if (this.DbTypeName == SqlDbType.VarChar)
                    return "\r\n\t,@" + this.ColumnName + " " + this.TypeName + "(" + ((int)this.Length == -1 ? "MAX" : this.Length.ToString()) + ")=NULL--" + this.deText;
                else if (this.DbTypeName == SqlDbType.DateTime)
                    return "\r\n\t,@" + this.ColumnName + " " + this.TypeName + "=NULL--" + this.deText + "\r\n\t,@" + this.ColumnName + "End " + this.TypeName + "=NULL--" + this.deText;
                else
                    return "\r\n\t,@" + this.ColumnName + " " + this.TypeName + "=NULL--" + this.deText;
            }
        }

        public string WhereStr
        {
            get
            {
                if (this.DbTypeName == SqlDbType.VarChar)
                    return "\r\n\t\tAND (@" + this.ColumnName + " IS NULL OR " + this.ColumnName + " = @" + this.ColumnName + ")--" + this.deText;
                else if (this.DbTypeName == SqlDbType.DateTime)
                    return "\r\n\t\tAND (" + ("1900>=year(@" + this.ColumnName + ")").PadRight(30, ' ') + " OR @" + this.ColumnName + " IS NULL OR " + this.ColumnName + " >= @" + this.ColumnName + ")--" + this.deText + "\r\n\t\tAND (" + ("1900>=year(@" + this.ColumnName + "End)").PadRight(30, ' ') + " OR @" + this.ColumnName + "End IS NULL OR " + this.ColumnName + " <= @" + this.ColumnName + "End)";
                else
                    return "\r\n\t\tAND ( @" + this.ColumnName + " IS NULL OR " + this.ColumnName + " = @" + this.ColumnName + ")--" + this.deText;
            }
        }

        public string GetCoIndexStr
        {
            get
            {
                return "\t\tint co" + this.ColumnName + " = dr.GetOrdinal(" + this.TableName + "." + this.ColumnName + "Field);\r\n";
            }
        }

        public string GetRowValStr
        {
            get
            {
                if (this.isNull)
                {
                    string str = "0";
                    if (this.DbTypeName == SqlDbType.VarChar)
                        str = "string.Empty";
                    else if (this.DbTypeName == SqlDbType.DateTime)
                        str = "(DateTime)System.Data.SqlTypes.SqlDateTime.Null";
                    return "\t\t\trow." + (object)this.ColumnName + " = dr.IsDBNull(co" + this.ColumnName + ") ? " + str + " : dr.Get" + (string)(object)this.DbTypeName + "(co" + this.ColumnName + ");\r\n";
                }
                else
                    return "\t\t\trow." + (object)this.ColumnName + " = dr.Get" + (string)(object)this.DbTypeName + "(co" + this.ColumnName + ");\r\n";
            }
        }

        public object BuildEntity(IDataRecord dataRecord, Dictionary<string, int> colName)
        {
            if (colName.ContainsKey("colorder"))
                this.colorder = dataRecord.GetInt32(colName["colorder"]);
            if (colName.ContainsKey("TableName"))
                this.TableName = dataRecord.GetString(colName["TableName"]);
            if (colName.ContainsKey("ColumnName"))
                this.ColumnName = dataRecord.IsDBNull(colName["ColumnName"]) ? null : dataRecord.GetString(colName["ColumnName"]);
            if (colName.ContainsKey("TypeName"))
                this.TypeName = dataRecord.GetString(colName["TypeName"]);
            if (colName.ContainsKey("Length"))
                this.Length = dataRecord.GetInt16(colName["Length"]);
            if (colName.ContainsKey("Preci"))
                this.Preci = dataRecord.GetByte(colName["Preci"]);
            if (colName.ContainsKey("IsIdentity"))
                this.IsIdentity = dataRecord.GetBoolean(colName["IsIdentity"]);
            if (colName.ContainsKey("isPK"))
                this.isPK = dataRecord.GetBoolean(colName["isPK"]);
            if (colName.ContainsKey("IndexName"))
                this.IndexName = dataRecord.GetString(colName["IndexName"]);
            if (colName.ContainsKey("IndexSort"))
                this.IndexSort = dataRecord.GetString(colName["IndexSort"]);
            if (colName.ContainsKey("defaultVal"))
                this.defaultVal = dataRecord.GetString(colName["defaultVal"]);
            if (colName.ContainsKey("rftable"))
                this.rftable = dataRecord.IsDBNull(colName["rftable"]) ? null : dataRecord.GetString(colName["rftable"]);
            if (colName.ContainsKey("rfcolumn"))
                this.rfcolumn = dataRecord.IsDBNull(colName["rfcolumn"]) ? null : dataRecord.GetString(colName["rfcolumn"]);
            if (colName.ContainsKey("Scale"))
                this.Scale = dataRecord.GetByte(colName["Scale"]);
            if (colName.ContainsKey("isComputed"))
                this.isComputed = dataRecord.GetBoolean(colName["isComputed"]);
            if (colName.ContainsKey("isNull"))
                this.isNull = !dataRecord.IsDBNull(colName["isNull"]) && dataRecord.GetBoolean(colName["isNull"]);
            if (colName.ContainsKey("Create_Date"))
                this.Create_Date = dataRecord.GetDateTime(colName["Create_Date"]);
            if (colName.ContainsKey("Modify_Date"))
                this.Modify_Date = dataRecord.GetDateTime(colName["Modify_Date"]);
            if (colName.ContainsKey("deText"))
                this.deText = dataRecord.IsDBNull(colName["deText"]) ? (string)null : dataRecord.GetString(colName["deText"]);
            return (object)this;
        }
    }
    [Serializable]
    public class Database
    {
        private readonly List<TableObject> _tables = new List<TableObject>();
        private readonly List<TableObject> _views = new List<TableObject>();
        private readonly Dictionary<string, TableObject> _tableAndView = new Dictionary<string, TableObject>();
        private readonly List<ProObject> _procedures = new List<ProObject>();
        private readonly Dictionary<string, ProObject> _proDic = new Dictionary<string, ProObject>();
        private readonly Dictionary<string, string> _allFieldNote = new Dictionary<string, string>();
        private Dictionary<string, FieldObject> _fieldObjectsHaveTableName = new Dictionary<string, FieldObject>();

        public string ConnectionString { get; set; }

        public string DbName { get; set; }

        public string Description { get; set; }

        public List<TableObject> Tables
        {
            get
            {
                return this._tables;
            }
        }

        public List<TableObject> Views
        {
            get
            {
                return this._views;
            }
        }

        public List<ProObject> Procedures
        {
            get
            {
                return this._procedures;
            }
        }

        public void AddTable(TableObject table)
        {
            this._tables.Add(table);
            if (this._tableAndView.ContainsKey(table.name))
                return;
            this._tableAndView.Add(table.name, table);
        }

        public void AddView(TableObject view)
        {
            this._views.Add(view);
            if (this._tableAndView.ContainsKey(view.name))
                return;
            this._tableAndView.Add(view.name, view);
        }

        public TableObject GetTableOrView(string name)
        {
            if (!this._tableAndView.ContainsKey(name))
                return (TableObject)null;
            else
                return this._tableAndView[name];
        }

        public List<TableObject> GetTableView()
        {
            return Enumerable.ToList<TableObject>((IEnumerable<TableObject>)this._tableAndView.Values);
        }

        public void AddPro(ProObject pro)
        {
            this._procedures.Add(pro);
            if (this._proDic.ContainsKey(pro.ProName))
                return;
            this._proDic.Add(pro.ProName, pro);
        }

        public ProObject GetPro(string name)
        {
            if (!this._proDic.ContainsKey(name))
                return (ProObject)null;
            else
                return this._proDic[name];
        }

        public void AddField(FieldObject field)
        {
            if (!string.IsNullOrEmpty(field.deText) && !this._allFieldNote.ContainsKey(field.ColumnName))
                this._allFieldNote.Add(field.ColumnName, field.deText);
            if (this._fieldObjectsHaveTableName.ContainsKey(field.TableName + field.ColumnName))
                return;
            this._fieldObjectsHaveTableName.Add(field.TableName + field.ColumnName, field);
        }

        public string GetFieldNote(string ColumnName)
        {
            if (!this._allFieldNote.ContainsKey(ColumnName))
                return "";
            else
                return this._allFieldNote[ColumnName];
        }

        public Dictionary<string, List<FieldObject>> GetFieldTreeByFieldWithTableName(List<string> fieldWithTableNames)
        {
            Dictionary<string, FieldObject> dictionary1 = this._fieldObjectsHaveTableName;
            Dictionary<string, List<FieldObject>> dictionary2 = new Dictionary<string, List<FieldObject>>();
            if (fieldWithTableNames != null)
            {
                foreach (string key in fieldWithTableNames)
                {
                    if (dictionary1.ContainsKey(key))
                    {
                        FieldObject fieldObject = dictionary1[key];
                        if (!dictionary2.ContainsKey(fieldObject.TableName))
                            dictionary2.Add(fieldObject.TableName, new List<FieldObject>());
                        dictionary2[fieldObject.TableName].Add(fieldObject);
                    }
                }
            }
            return dictionary2;
        }

        public static Dictionary<string, int> GetColNameIndex(IDataRecord dataRecord)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < dataRecord.FieldCount; ++i)
                dictionary.Add(dataRecord.GetName(i), i);
            return dictionary;
        }
    }
    public enum SqlType
    {
        MsSql2008,
        MySql,
        Oracle,
    }
}