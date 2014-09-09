using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Skpic.T4Manager
{
    public class DbField : T4Base
    {
        private static readonly Regex NotWordRegex = new Regex("\\W", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex nolockRegex = new Regex("FROM\\s+[\\w\\.]+|JOIN\\s+[\\w\\.]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        static DbField()
        {
        }

        public DbField(string templateFilePath, string ConnectionName = "MetaDataDB")
            : base(templateFilePath, ConnectionName)
        {
        }

        /// <summary>
        /// 构建表元信息
        /// </summary>
        public void BuildTableAbout(StringBuilder outStr, TableObject table, FieldObject pkField)
        {
            string str1 = "00000000";
            FieldObject fieldObject1 = Enumerable.FirstOrDefault<FieldObject>((IEnumerable<FieldObject>)table.Columns, (Func<FieldObject, bool>)(z => z.IsIdentity));
            if (fieldObject1 != null)
                str1 = fieldObject1.ColumnName;
            List<string> list = new List<string>();
            string str2 = "";
            string str3 = "";
            string str4 = "";
            foreach (FieldObject fieldObject2 in table.Columns)
            {
                if (!fieldObject2.isPK)
                    str2 = str2 + fieldObject2.ColumnName + "=@" + fieldObject2.ColumnName + ",";
                list.Add(fieldObject2.ColumnName);
                str3 = str3 + "{ \"" + fieldObject2.ColumnName + "\", typeof(" + fieldObject2.TypeNameCs + ") },";
                str4 = str4 + "{ \"" + fieldObject2.ColumnName + "\", \"" + DbField.NotWordRegex.Split(T4Base.MyDb.GetFieldNote(fieldObject2.ColumnName).Trim())[0] + "\"},";
            }
            outStr.AppendLine(this.Indent + "#region  " + table.name + "表元信息");
            outStr.AppendLine(this.Indent + "public const string dbName = \"" + T4Base.MyDb.DbName + "\";//库名");
            outStr.AppendLine(this.Indent + "public const string tableName = \"" + table.name + "\";//表名");
            outStr.AppendLine(this.Indent + "public const string pkName = \"" + pkField.ColumnName + "\";//主键名");
            if (table.type.Trim() == "V")
            {
                string str5 = "";
                DataSet dataSet = this.ExeSqlDataSet("sp_helptext  " + table.name);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 2)
                {
                    for (int index = 2; index < dataSet.Tables[0].Rows.Count; ++index)
                        str5 = str5 + " " + dataSet.Tables[0].Rows[index][0].ToString().Trim().Replace("  ", " ");
                }
                outStr.AppendLine(this.Indent + "public const string insertSql = @\"\";");
                outStr.AppendLine(this.Indent + "public const string selectSql = @\"" + DbField.nolockRegex.Replace(str5.Trim(), "$0(nolock)") + " where 1 = 1 \";");
                outStr.AppendLine(this.Indent + "public const string deleteSql = @\"\";");
            }
            else
            {
                outStr.AppendLine(this.Indent + "public const string insertSql = @\"Insert Into \" + dbName + \".dbo." + table.name + " (" + (" " + string.Join(",", list.ToArray())).Replace(" " + str1 + ",", "").Trim() + ")values(@" + (" " + string.Join(",@", list.ToArray())).Replace(" " + str1 + ",@", "").Trim() + ");\";");
                outStr.AppendLine(this.Indent + "public const string selectSql = @\"select " + string.Join(",", list.ToArray()) + " from \" + dbName + \".dbo." + table.name + " with(nolock) where 1 = 1 \";");
                outStr.AppendLine(this.Indent + "public const string deleteSql = @\"delete from  \" + dbName + \".dbo." + table.name + " where " + pkField.ColumnName + " = @" + pkField.ColumnName + ";\";");
            }
            outStr.AppendLine(this.Indent + "public static readonly Dictionary<string,Type> fieldNames;//字段");
            outStr.AppendLine(this.Indent + "public static readonly Dictionary<string, string> fieldNotes;//字备注");
            outStr.AppendLine(this.Indent + "static " + table.name + "()");
            outStr.AppendLine(this.Indent + "{");
            outStr.AppendLine(this.Indent + "   fieldNames = new Dictionary<string,Type> {" + str3.TrimEnd(new char[1]
      {
        ','
      }) + "};");
            outStr.AppendLine(this.Indent + "   fieldNotes = new Dictionary<string,string> {" + str4.TrimEnd(new char[1]
      {
        ','
      }) + "};");
            outStr.AppendLine(this.Indent + "}");
            outStr.AppendLine(this.Indent + "/// <summary>库名</summary>");
            outStr.AppendLine(this.Indent + "public string GetDbName() { return dbName;}");
            outStr.AppendLine(this.Indent + "/// <summary>表名</summary>");
            outStr.AppendLine(this.Indent + "public string GetTableName() { return tableName;}");
            outStr.AppendLine(this.Indent + "/// <summary>主键名</summary>");
            outStr.AppendLine(this.Indent + "public string GetPkName() { return pkName;}");
            outStr.AppendLine(this.Indent + "/// <summary>插入Sql</summary>");
            outStr.AppendLine(this.Indent + "public string GetInsertSql(){return insertSql;}");
            outStr.AppendLine(this.Indent + "/// <summary>查询Sql</summary>");
            outStr.AppendLine(this.Indent + "public string GetSelectSql(){return selectSql;}");
            outStr.AppendLine(this.Indent + "/// <summary>删除Sql</summary>");
            outStr.AppendLine(this.Indent + "public string GetDeleteSql(){return deleteSql;}");
            outStr.AppendLine(this.Indent + "/// <summary>字段名集合</summary>");
            outStr.AppendLine(this.Indent + "public Dictionary<string,Type> GetFieldNames() { return fieldNames;}");
            outStr.AppendLine(this.Indent + "/// <summary>字段名集合</summary>");
            outStr.AppendLine(this.Indent + "public Dictionary<string,string> GetFieldNotes() { return fieldNotes;}");
            outStr.AppendLine(this.Indent + "#endregion");
        }

        /// <summary>
        /// 构建默认值
        /// </summary>
        public void BuildDefaultValue(StringBuilder outStr, TableObject table, FieldObject pkField)
        {
            outStr.AppendLine(this.Indent + "#region  " + table.name + "默认值");
            outStr.AppendLine(this.Indent + "public " + table.name + "()");
            outStr.AppendLine(this.Indent + "{");
            outStr.AppendLine(this.Indent + "\tSetDefaultValue();");
            outStr.AppendLine(this.Indent + "}");
            outStr.AppendLine(this.Indent + "public void SetDefaultValue()");
            outStr.AppendLine(this.Indent + "{");
            this.PushIndent("\t");
            foreach (FieldObject fieldObject in table.Columns)
            {
                string s = fieldObject.defaultVal.Replace("(", "").Replace(")", "").Trim(new char[1]
        {
          '\''
        });
                switch (fieldObject.TypeNameCs)
                {
                    case "string":
                        outStr.AppendLine(this.Indent + "if (" + (string)(object)fieldObject.ColumnName + " == null)" + (string)(object)fieldObject.ColumnName + " = \"" + s + "\";");
                        continue;
                    case "byte":
                    case "decimal":
                    case "int":
                    case "long":
                    case "short":
                    case "double":
                        Decimal result1;
                        Decimal.TryParse(s, out result1);
                        if (result1 > new Decimal(0))
                        {
                            outStr.AppendLine(this.Indent + "if (" + (string)(object)fieldObject.ColumnName + " == 0)" + (string)(object)fieldObject.ColumnName + " = " + s + ";");
                            continue;
                        }
                        else
                            continue;
                    case "DateTime":
                        if (s.Contains("getdate"))
                        {
                            outStr.AppendLine(this.Indent + "if (" + (string)(object)fieldObject.ColumnName + ".Year<1900)" + (string)(object)fieldObject.ColumnName + " = DateTime.Now;");
                            continue;
                        }
                        else
                        {
                            DateTime result2;
                            DateTime.TryParse(s, out result2);
                            if (result2.Year >= 1900)
                            {
                                outStr.AppendLine(this.Indent +
                                    (object)"if (" + (string)(object)fieldObject.ColumnName + ".Year<1900)" +
                                    (string)(object)fieldObject.ColumnName + " = new DateTime(" +
                                    result2.Year.ToString() + "," +
                                    result2.Month.ToString() + "," +
                                    result2.Day.ToString() + ");");
                                continue;
                            }
                            else
                                continue;
                        }
                    case "bool":
                        continue;
                    default:
                        if (s.Length > 0)
                        {
                            outStr.AppendLine(this.Indent + "if (" + fieldObject.ColumnName + " == null)" + fieldObject.ColumnName + " = " + s + ";");
                            continue;
                        }
                        else
                            continue;
                }
            }
            this.PopIndent();
            outStr.AppendLine(this.Indent + "}");
            outStr.AppendLine(this.Indent + "#endregion");
        }

        /// <summary>
        /// 构建属性
        /// </summary>
        public void BuildProperty(StringBuilder outStr, TableObject table, FieldObject pkField)
        {
            outStr.AppendLine(this.Indent + (object)"#region  " + table.name + "属性" + Enumerable.Count(table.Columns));
            foreach (FieldObject fieldObject in table.Columns)
            {
                fieldObject.deText = fieldObject.deText.Replace("\r", "").Replace("\n", "");
                if (fieldObject.deText.Trim().Length > 0)
                    outStr.AppendLine(this.Indent + (object)"///<summary>" + fieldObject.deText.Trim() + "," + fieldObject.TypeNameCs + "" + fieldObject.Length + "</summary>");
                else
                    outStr.AppendLine(this.Indent + (object)"///<summary>" + T4Base.MyDb.GetFieldNote(fieldObject.ColumnName).Trim() + "," + fieldObject.TypeNameCs + "" + fieldObject.Length + "</summary>");
                outStr.AppendLine(this.Indent + "public " + fieldObject.TypeNameCs + " " + fieldObject.ColumnName + "{ get; set; }");
            }
            outStr.AppendLine(this.Indent + "#endregion");
        }

        /// <summary>
        /// 构建SqlParameters
        /// </summary>
        public void BuildSqlParameters(StringBuilder outStr, TableObject table, FieldObject pkField)
        {
            outStr.AppendLine(this.Indent + "/// <summary>主键" + pkField.TypeNameCs + "</summary>");
            outStr.AppendLine(this.Indent + "public object PkValue { get { return " + pkField.ColumnName + "; } set { if (value is " + pkField.TypeNameCs + ")" + pkField.ColumnName + " = (" + pkField.TypeNameCs + ")value; } } ");
            outStr.AppendLine(this.Indent + "/// <summary>得到主键集合</summary>");
            outStr.AppendLine(this.Indent + "public Dictionary<string, object> GetPkValues()");
            outStr.AppendLine(this.Indent + "{");
            outStr.AppendLine(this.Indent + "    Dictionary<string, object> pkValues = new Dictionary<string, object>();");
            foreach (FieldObject fieldObject in table.Columns)
            {
                if (fieldObject.isPK)
                    outStr.AppendLine(this.Indent + "    pkValues.Add(\"" + fieldObject.ColumnName + "\", " + fieldObject.ColumnName + ");");
            }
            outStr.AppendLine(this.Indent + "    return pkValues;");
            outStr.AppendLine(this.Indent + "}");
            outStr.AppendLine(this.Indent + "///<summary>实体到参数</summary>");
            outStr.AppendLine(this.Indent + "/// <param name=\"haveDentity\">是否包含自增长列</param>");
            outStr.AppendLine(this.Indent + "public SqlParameterWrapperCollection ToSqlParameters(bool haveDentity=true)");
            outStr.AppendLine(this.Indent + "{");
            this.PushIndent("\t");
            outStr.AppendLine(this.Indent + "SqlParameterWrapperCollection Collection = new SqlParameterWrapperCollection();");
            foreach (FieldObject fieldObject in table.Columns)
            {
                outStr.Append(this.Indent);
                if (fieldObject.IsIdentity)
                    outStr.Append("if (haveDentity) ");
                outStr.AppendLine("Collection.Add(new SqlParameterWrapper(\"@" + fieldObject.ColumnName + "\", " + fieldObject.ColumnName + "));");
            }
            outStr.AppendLine(this.Indent + "return Collection;");
            this.PopIndent();
            outStr.AppendLine(this.Indent + "}");
            outStr.AppendLine(this.Indent + "///<summary>分析sql添加参数</summary>");
            outStr.AppendLine(this.Indent + "/// <param name=\"sql\">要添加参数的sql</param>");
            outStr.AppendLine(this.Indent + "public SqlParameterWrapperCollection ToSqlParameters(string sql)");
            outStr.AppendLine(this.Indent + "{");
            this.PushIndent("\t");
            outStr.AppendLine(this.Indent + "SqlParameterWrapperCollection Collection = new SqlParameterWrapperCollection();");
            outStr.AppendLine(this.Indent + "List<string> sqlHaveParameters =SqlAid.GetSqlParametersList(sql);");
            foreach (FieldObject fieldObject in table.Columns)
                outStr.AppendLine(this.Indent + "if (sqlHaveParameters.Contains(\"" + fieldObject.ColumnName + "\")) Collection.Add(new SqlParameterWrapper(\"@" + fieldObject.ColumnName + "\", " + fieldObject.ColumnName + "));");
            outStr.AppendLine(this.Indent + "return Collection;");
            this.PopIndent();
            outStr.AppendLine(this.Indent + "}");
        }

        /// <summary>
        /// 构建ToJosn
        /// </summary>
        public void BuildToJosn(StringBuilder outStr, TableObject table)
        {
            outStr.AppendLine(this.Indent + "///<summary>返回对象Josn字串</summary>");
            outStr.AppendLine(this.Indent + "public string ToJosn()");
            outStr.AppendLine(this.Indent + "{");
            outStr.AppendLine(this.Indent + "\treturn \"{\\\"TableName\\\":\\\"" + table.name + "\\\"\"");
            foreach (FieldObject fieldObject in table.Columns)
                outStr.AppendLine(this.Indent + "\t\t+\",\\\"" + fieldObject.ColumnName + "\\\":\\\"\"+" + fieldObject.ColumnName + "+\"\\\"\"");
            outStr.AppendLine(this.Indent + "\t\t+\"}\";");
            outStr.AppendLine(this.Indent + "}");
        }

        /// <summary>
        /// 构建IDataRecord填充实体
        /// </summary>
        public void BuildDataRecord(StringBuilder outStr, TableObject table)
        {
            outStr.AppendLine(this.Indent + "///<summary>IDataRecord填充实体,返回自己</summary>");
            outStr.AppendLine(this.Indent + "///<param name=\"colName\">列名的列次序，可调用GetColNameIndex获得</param>");
            outStr.AppendLine(this.Indent + "public " + table.name + " BuildEntity(IDataRecord dataRecord,Dictionary<string,int> colName)");
            outStr.AppendLine(this.Indent + "{");
            foreach (FieldObject fieldObject in table.Columns)
            {
                if (fieldObject.isNull)
                    outStr.AppendLine(this.Indent + "\tif (colName.ContainsKey(\"" + fieldObject.ColumnName + "\")&&!dataRecord.IsDBNull(colName[\"" + fieldObject.ColumnName + "\"]))" + fieldObject.ColumnName + " = dataRecord.Get" + fieldObject.DbTypeNameStr + "(colName[\"" + fieldObject.ColumnName + "\"]);");
                else
                    outStr.AppendLine(this.Indent + "\tif (colName.ContainsKey(\"" + fieldObject.ColumnName + "\"))" + fieldObject.ColumnName + " =dataRecord.Get" + fieldObject.DbTypeNameStr + "(colName[\"" + fieldObject.ColumnName + "\"]);");
            }
            outStr.AppendLine(this.Indent + "\treturn this;");
            outStr.AppendLine(this.Indent + "}");
        }

        /// <summary>
        /// 构建DataRow填充实体
        /// </summary>
        public void BuildDataRow(StringBuilder outStr, TableObject table)
        {
            outStr.AppendLine(this.Indent + "///<summary>DataRow填充实体,返回自己</summary>");
            outStr.AppendLine(this.Indent + "///<param name=\"colName\">列名的列次序，可调用GetColNameIndex获得</param>");
            outStr.AppendLine(this.Indent + "public " + table.name + " BuildEntity(DataRow dr, Dictionary<string, int> colName)");
            outStr.AppendLine(this.Indent + "{");
            foreach (FieldObject fieldObject in table.Columns)
            {
                if (fieldObject.isNull)
                    outStr.AppendLine(this.Indent + "\tif (colName.ContainsKey(\"" + fieldObject.ColumnName + "\")&&!dr.IsNull(colName[\"" + fieldObject.ColumnName + "\"]))" + fieldObject.ColumnName + " =  (" + fieldObject.TypeNameCs + ")dr[colName[\"" + fieldObject.ColumnName + "\"]];");
                else
                    outStr.AppendLine(this.Indent + "\tif (colName.ContainsKey(\"" + fieldObject.ColumnName + "\"))" + fieldObject.ColumnName + " =(" + fieldObject.DbTypeNameStr + ")dr[colName[\"" + fieldObject.ColumnName + "\"]];");
            }
            outStr.AppendLine(this.Indent + "\treturn this;");
            outStr.AppendLine(this.Indent + "}");
        }

        public string Render()
        {
            StringBuilder outStr = new StringBuilder();
            outStr.AppendLine(this.Indent + "using System;");
            outStr.AppendLine(this.Indent + "using System.Collections.Generic;");
            outStr.AppendLine(this.Indent + "using System.Data;");
            outStr.AppendLine(this.Indent + "using TCSmartFramework.DataAccess;");
            AppSettingsSection appSettingsSection = (AppSettingsSection)this._configuration.GetSection("appSettings");
            if (appSettingsSection.Settings["using"] != null && appSettingsSection.Settings["using"].Value != null)
            {
                string str1 = appSettingsSection.Settings["using"].Value;
                char[] chArray = new char[1]
        {
          ','
        };
                foreach (string str2 in str1.Split(chArray))
                {
                    if (!string.IsNullOrWhiteSpace(str2))
                        outStr.AppendLine(this.Indent + "using " + str2 + ";");
                }
            }
            outStr.AppendLine(this.Indent + "namespace " + this.NamespaceStr);
            outStr.AppendLine(this.Indent + "{");
            foreach (TableObject table in T4Base.MyDb.GetTableView())
            {
                if ((this.OnlyTable.Count == 0 || this.OnlyTable.Contains(table.name)) && !this.NoTable.Contains(table.name))
                {
                    table.comments = table.comments.Replace("\r", "").Replace("\n", "");
                    FieldObject pkField = new FieldObject();
                    foreach (FieldObject fieldObject in table.Columns)
                    {
                        if (fieldObject.isPK)
                            pkField = fieldObject;
                    }
                    if (pkField.ColumnName == null)
                        pkField = Enumerable.First<FieldObject>((IEnumerable<FieldObject>)table.Columns);
                    this.PushIndent("\t");
                    outStr.AppendLine(this.Indent + "///<summary>" + table.comments + "</summary>");
                    outStr.AppendLine(this.Indent + "public partial class " + table.name + " : ITableBase<" + table.name + ">");
                    outStr.AppendLine(this.Indent + "{");
                    this.PushIndent("\t");
                    this.BuildTableAbout(outStr, table, pkField);
                    this.BuildDefaultValue(outStr, table, pkField);
                    this.BuildProperty(outStr, table, pkField);
                    this.BuildSqlParameters(outStr, table, pkField);
                    this.BuildDataRecord(outStr, table);
                    this.BuildDataRow(outStr, table);
                    this.BuildToJosn(outStr, table);
                    this.PopIndent();
                    outStr.AppendLine(this.Indent + "}");
                    this.PopIndent();
                }
            }
            outStr.AppendLine(this.Indent + "}");
            return ((object)outStr).ToString();
        }
    }
}