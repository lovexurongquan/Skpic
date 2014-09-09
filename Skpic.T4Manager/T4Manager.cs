using System;
using System.Collections.Generic;
using System.Xml;
using Skpic.Async;
using Skpic.Factory;

namespace Skpic.T4Manager
{
    public class T4Manager
    {
        /// <summary>
        /// default Constructor
        /// </summary>
        /// <param name="templateFilePath">template file path.</param>
        /// <param name="connectionName"></param>
        public T4Manager(string templateFilePath, string connectionName = "ConnectionString")
        {
            XmlHelper = new XmlHelper(TemplateFactory.GetProviderName(templateFilePath, connectionName));
            ConnectionName = connectionName;
            TemplateFilePath = templateFilePath;
        }

        /// <summary>
        /// template file path. can use this.Host.TemplateFile in template.
        /// </summary>
        private string TemplateFilePath { get; set; }

        /// <summary>
        /// connection to database with this connection name in config.
        /// </summary>
        private string ConnectionName { get; set; }

        /// <summary>
        /// use this to load xml.
        /// </summary>
        private XmlHelper XmlHelper { get; set; }

        /// <summary>
        /// get all table in database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetTableCollection()
        {
            using (var conn = TemplateFactory.GetConnection(TemplateFilePath, ConnectionName))
            {
                var sql = XmlHelper.TableObjectSql;

                return conn.Query<Table>(sql);
            }
        }

        /// <summary>
        /// get all table column in database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Columns> GetColumnsCollection()
        {
            using (var conn = TemplateFactory.GetConnection(TemplateFilePath, ConnectionName))
            {
                var sql = XmlHelper.FieldObjectSql;

                return conn.Query<Columns>(sql);
            }
        }

        /// <summary>
        /// get c# type by database type string.
        /// </summary>
        /// <param name="type">database type string.</param>
        /// <param name="isNull">the type is nullable?</param>
        /// <returns>C# type string.</returns>
        public string GetTypeByDatabase(string type, string isNull)
        {
            var isNullable = Convert.ToBoolean(isNull);
            switch (type.ToLower())
            {
                case "int":
                case "bigint":
                case "tinyint":
                case "integer":
                case "smallint":
                case "mediumint":
                    return GetTypeIsNull("int", isNullable);
                case "bit":
                    return GetTypeIsNull("bool", isNullable);
                case "date":
                case "time":
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    return GetTypeIsNull("DateTime", isNullable);
                case "float":
                case "number":
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    return GetTypeIsNull("decimal", isNullable);
                case "raw":
                case "binary":
                case "image":
                case "varbinary":
                case "bfile":
                case "blob":
                    return "byte[]";
                case "double":
                    return GetTypeIsNull("double", isNullable);
                default:
                    return "string";
            }

        }

        /// <summary>
        /// set model default value.
        /// </summary>
        /// <param name="defaultValue">default in database.</param>
        /// <param name="columnName">the column name in talbe.</param>
        /// <param name="type">the column type in table with column name.</param>
        /// <returns></returns>
        public string SetDefaultValue(string defaultValue, string columnName, string type)
        {
            var s = defaultValue.Replace("(", "").Replace(")", "").Trim(new[] { '\'' });
            switch (type.ToLower())
            {
                case "bit":
                    if (s.Length > 0)
                    {
                        if (s.Equals("1"))
                        {
                            return columnName + " = true;";
                        }
                        return columnName + " = false;";
                    }
                    return "";
                case "date":
                case "time":
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    if (s.Contains("getdate"))
                    {
                        return columnName + " = DateTime.Now;";
                    }
                    DateTime result;
                    DateTime.TryParse(s, out result);
                    if (result.Year >= 1900)
                    {
                        return columnName + " = new DateTime(" + result.Year + "," + result.Month + "," + result.Day + ");";
                    }
                    return "";
                case "int":
                case "bigint":
                case "tinyint":
                case "integer":
                case "smallint":
                case "mediumint":
                case "float":
                case "number":
                case "decimal":
                case "double":
                case "money":
                case "numeric":
                case "smallmoney":
                    Decimal result1;
                    Decimal.TryParse(s, out result1);
                    if (result1 > new Decimal(0))
                    {
                        return columnName + " = " + s + ";";
                    }
                    return "";
                case "raw":
                case "binary":
                case "image":
                case "varbinary":
                case "bfile":
                case "blob":
                    return "";
                default:
                    return columnName + " = \"" + s + "\";";
            }
        }

        /// <summary>
        /// 判断类型是否可为空
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isNull"></param>
        /// <returns></returns>
        private string GetTypeIsNull(string type, bool isNull)
        {
            if (isNull) type = type + "?";
            return type;
        }
    }

    /// <summary>
    /// get xml in this directory
    /// </summary>
    public class XmlHelper
    {
        private readonly XmlNode _xmlNode;

        public string DataBaseListSql
        {
            get
            {
                return GetSqlStr("DataBaseListSql");
            }
        }

        public string TableObjectSql
        {
            get
            {
                return GetSqlStr("TableObjectSql");
            }
        }

        public string FieldObjectSql
        {
            get
            {
                return GetSqlStr("FieldObjectSql");
            }
        }

        public string ProObjectSql
        {
            get
            {
                return GetSqlStr("ProObjectSql");
            }
        }

        public string ParObjectSql
        {
            get
            {
                return GetSqlStr("ParObjectSql");
            }
        }

        public XmlHelper(string fileName = "System.Data.SqlClient")
        {
            var sqlXml = new XmlDocument();
            var manifestResourceStream = GetType().Assembly.GetManifestResourceStream("Skpic.T4Manager." + fileName + ".xml");
            if (manifestResourceStream != null)
                sqlXml.Load(manifestResourceStream);
            _xmlNode = sqlXml.SelectSingleNode("DbSql");
        }

        private string GetSqlStr(string key)
        {
            for (var index = 0; index < _xmlNode.ChildNodes.Count; ++index)
            {
                if (_xmlNode.ChildNodes[index].Attributes != null && _xmlNode.ChildNodes[index].Attributes["name"].Value == key)
                    return _xmlNode.ChildNodes[index].InnerText;
            }
            return "";
        }
    }
}