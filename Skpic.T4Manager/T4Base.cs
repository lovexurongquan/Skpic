using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skpic.T4Manager
{
    /// <summary>
    /// T4的帮助 http://msdn.microsoft.com/zh-cn/library/bb126478
    /// </summary>
    public class T4Base
    {
        /// <summary>
        /// 数据库的备注
        /// </summary>
        public static Dictionary<string, FieldObject> Decs = new Dictionary<string, FieldObject>();
        /// <summary>
        /// 表备注
        /// </summary>
        public static Dictionary<string, TableObject> TableDecs = new Dictionary<string, TableObject>();
        /// <summary>
        /// 不要生成的表
        /// </summary>
        public string NamespaceStr = "";
        /// <summary>
        /// 不要生成的表
        /// </summary>
        public List<string> NoTable = new List<string>();
        /// <summary>
        /// 只要生成的表
        /// </summary>
        public List<string> OnlyTable = new List<string>();
        /// <summary>
        /// 格式化\t数
        /// </summary>
        public string Indent = "";
        protected readonly Configuration _configuration;
        /// <summary>
        /// DB
        /// </summary>
        public static Database MyDb;
        public SqlConnection DB;

        static T4Base()
        {
        }

        public T4Base(string templateFilePath, string connectionName = "MetaDataDB")
        {
            string directoryName = Path.GetDirectoryName(templateFilePath);
            string str = Enumerable.FirstOrDefault<string>((IEnumerable<string>)Directory.GetFiles(directoryName, "*.config")) ?? Enumerable.FirstOrDefault<FileInfo>((IEnumerable<FileInfo>)Directory.GetParent(directoryName).GetFiles("*.config")).FullName;
            this._configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
            {
                ExeConfigFilename = str
            }, ConfigurationUserLevel.None);
            AppSettingsSection appSettingsSection = (AppSettingsSection)this._configuration.GetSection("appSettings");
            MdFactory.SetConnectionStr(((ConnectionStringsSection)this._configuration.GetSection("connectionStrings")).ConnectionStrings[connectionName].ConnectionString);
            T4Base.MyDb = MdFactory.SetCurrentDbName(appSettingsSection.Settings[connectionName + "_CurrentDbName"].Value, true);
            foreach (TableObject tableObject in T4Base.MyDb.GetTableView())
            {
                if (!T4Base.TableDecs.ContainsKey(tableObject.name))
                    T4Base.TableDecs.Add(tableObject.name, tableObject);
                foreach (FieldObject fieldObject in tableObject.Columns)
                {
                    if (!T4Base.Decs.ContainsKey(fieldObject.ColumnName))
                        T4Base.Decs.Add(fieldObject.ColumnName, fieldObject);
                }
            }
        }

        public T4Base()
        {
        }

        /// <summary>
        /// 增加格式化\t数
        /// </summary>
        public void PushIndent(string str)
        {
            T4Base t4Base = this;
            string str1 = t4Base.Indent + str;
            t4Base.Indent = str1;
        }

        /// <summary>
        /// 减少化\t数
        /// </summary>
        public void PopIndent()
        {
            this.Indent = this.Indent.Remove(this.Indent.Length - 1);
        }

        /// <summary>
        /// 设置当前数据库
        /// </summary>
        public void SetDb(string dbName, bool refresh = false)
        {
            T4Base.MyDb = MdFactory.SetCurrentDbName(dbName, refresh);
        }

        public DataSet ExeSqlDataSet(string sqlStr)
        {
            if (this.DB == null || this.DB.Database != T4Base.MyDb.DbName)
                this.DB = new SqlConnection(T4Base.MyDb.ConnectionString);
            SqlCommand selectCommand = new SqlCommand(sqlStr, this.DB);
            selectCommand.CommandType = CommandType.Text;
            DataSet dataSet = new DataSet();
            ((DataAdapter)new SqlDataAdapter(selectCommand)).Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// 取数据例子
        /// </summary>
        public void TablesAndViews(StringBuilder str)
        {
            foreach (TableObject tableObject in T4Base.MyDb.GetTableView())
            {
                TableObject table = tableObject;
                if ((this.OnlyTable.Count == 0 || this.OnlyTable.Contains(table.name)) && !this.NoTable.Contains(table.name))
                {
                    foreach (FieldObject fieldObject in table.Columns)
                        ;
                    foreach (ProObject proObject in Enumerable.Where<ProObject>((IEnumerable<ProObject>)T4Base.MyDb.Procedures, (Func<ProObject, bool>)(z => z.text.IndexOf(table.name) >= 0)))
                        ;
                }
            }
        }

        public string GetTbDes(string tbName)
        {
            int num = tbName.LastIndexOf('.');
            if (num > -1)
                tbName = tbName.Substring(num + 1);
            if (!T4Base.TableDecs.ContainsKey(tbName))
                return "";
            TableObject tableObject = T4Base.TableDecs[tbName];
            return tableObject.comments + (object)tableObject.dates;
        }

        public string GetDes(string fieldName)
        {
            if (!T4Base.Decs.ContainsKey(fieldName))
                return "";
            FieldObject fieldObject = T4Base.Decs[fieldName];
            return fieldObject.deText.Trim() + (object)fieldObject.TypeNameCs + (string)(object)fieldObject.Length;
        }
    }
}
