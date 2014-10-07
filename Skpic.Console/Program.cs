using System;
using System.Linq;
using System.Linq.Expressions;
using Skpic.Async;
using Skpic.DataAccessLayer;
using Skpic.Factory;
using Skpic.IDataAccessLayer;
using Skpic.Model;
using System.Collections.Generic;

namespace Skpic.Console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            #region test dapper query

            //using (var conn = DataBaseFactory.GetConnection("OracleConnString"))
            //{
            //    var XmlHelper = new XmlHelper(DataBaseFactory.GetProviderName("OracleConnString"));
            //    var sql = XmlHelper.TableObjectSql;
            //    var x = conn.Query(sql);
            //}

            #endregion test dapper query

            #region test GetTableCollection

            //var t4 = new T4Manager.T4Manager(@"F:\Code\Skpic.Console\", "ConnectionString");

            //var tableCollection = t4.GetTableCollection();

            //var columnsCollection = t4.GetColumnsCollection();

            #endregion test GetTableCollection

            #region TestConnection

            //using (var conn=DbContextFactory.GetConnection())
            //{
            //    var login = new DoctorLoginInfo
            //    {
            //        DoctorInfo_ID = Guid.NewGuid().ToString(),
            //        DoctorLoginInfo_ID = "e85ff9d2-84c2-4d51-8287-8e95d443762e",

            //        DoctorLoginInfo_Pwd = "11111123",
            //        DoctorLoginInfo_LoginName = "Administrator",
            //        DoctorLoginInfo_CreateTime = DateTime.Now,
            //        DoctorLoginInfo_Pwd_Temp_Time = DateTime.Now
            //    };
            //    conn.Update(login);
            //    System.Console.WriteLine(login.DoctorLoginInfo_ID);
            //}

            #endregion TestConnection

            #region TestDeleteLambda

            //var list = new List<string>()
            //{
            //    "e85ff9d2-84c2-4d51-8287-8e95d443762e",
            //    "7e46bf7b-90dd-4480-9cea-29cc72239f08",
            //    "901dc11d-eff5-4be8-ae57-f5d513e56723",
            //    "32dbffdc-925f-41db-99df-fa03cb05de4f"
            //};

            //using (var conn = DbContextFactory.GetConnection())
            //{
            //    var i = conn.Delete<DoctorLoginInfo>(d => !list.Contains(d.DoctorLoginInfo_ID));
            //}

            #endregion TestDeleteLambda

            #region TestDelete

            //using (var conn = DbContextFactory.GetConnection())
            //{
            //    var sql = "delete from doctorlogininfo where DoctorLoginInfo_LoginName = '@a0'";
            //    var x = conn.Execute(sql, new { a0 = "Administrator" });
            //}

            #endregion TestDelete

            #region TestUnitWork

            //IUnitWork unitWork = new UnitWork();

            //var login1 = new DoctorLoginInfo()
            //{
            //    DoctorLoginInfo_ID = Guid.NewGuid().ToString(),
            //    DoctorInfo_ID = Guid.NewGuid().ToString(),
            //    DoctorLoginInfo_Enable = true,
            //    DoctorLoginInfo_Pwd = "sssss",
            //    DoctorLoginInfo_LoginName = "sxxxx",
            //    DoctorLoginInfo_Pwd_Temp = "jsldj",
            //    DoctorLoginInfo_Remark = "jskdfj",
            //    DoctorLoginInfo_CreateTime = DateTime.Now,
            //    DoctorLoginInfo_Pwd_Temp_Time = DateTime.Now
            //};

            //unitWork.RegistEntity(login1, EntityState.Create);

            //var login2 = new DoctorLoginInfo()
            //{
            //    DoctorLoginInfo_ID = "e85ff9d2-84c2-4d51-8287-8e95d443762e",
            //    DoctorInfo_ID = Guid.NewGuid().ToString(),
            //    DoctorLoginInfo_Enable = true,
            //    DoctorLoginInfo_Pwd = "e85ff9d2-84c2-4d51-8287-8e95d443762e",
            //    DoctorLoginInfo_LoginName = "e85ff9d2-84c2-4d51-8287-8e95d443762e",
            //    DoctorLoginInfo_Pwd_Temp = "jsldj",
            //    DoctorLoginInfo_Remark = "jskdfj",
            //    DoctorLoginInfo_CreateTime = DateTime.Now,
            //    DoctorLoginInfo_Pwd_Temp_Time = DateTime.Now
            //};

            //unitWork.RegistEntity(login2, EntityState.Modified);

            ////var login3 = new DoctorLoginInfo()
            ////{
            ////    DoctorLoginInfo_ID = "79e23e66-c56f-4e41-a2d1-dd101b3867dd"
            ////};

            ////unitWork.RegistEntity(login3, EntityState.Delete);
            //unitWork.Commit();

            #endregion TestUnitWork

            #region TestQueryLambda

            //var list = new List<string>()
            //{
            //    "29a127cf-8a46-4894-a6e2-2856af22e2ed",
            //    "7e46bf7b-90dd-4480-9cea-29cc72239f08",
            //    "901dc11d-eff5-4be8-ae57-f5d513e56723",
            //    "32dbffdc-925f-41db-99df-fa03cb05de4f"
            //};

            //using (var conn = DbContextFactory.GetConnection())
            //{
            //    var endsWith = conn.Query<DoctorLoginInfo>(d => d.DoctorLoginInfo_ID.EndsWith("901dc11d-eff5-4be8-ae57-f5d513e56723") || list.Contains(d.DoctorLoginInfo_ID)).GroupBy(g => g.DoctorLoginInfo_Pwd);
            //    var i = conn.Query<DoctorLoginInfo>(d => list.Contains(d.DoctorLoginInfo_ID)).OrderBy(d => d.DoctorLoginInfo_Pwd);
            //}

            //var contains = conn.Query<DoctorLoginInfo>(d => d.DoctorLoginInfo_ID.Contains("e85ff9d2-84c2-4d51-8287-8e95d443762e"));

            //var containsList1 = conn.Query<DoctorLoginInfo>(d => list.Contains(d.DoctorLoginInfo_ID));

            ////i'll fuck this. no no no.
            //var containsList2 = conn.Query<DoctorLoginInfo>(d => list.Any(l => l.Equals(d.DoctorLoginInfo_ID)));

            //var y = conn.Query<DoctorLoginInfo>(d => d.DoctorInfo_ID.Equals("c46bc122-8115-4ddd-a041-518658843e57"));

            #endregion TestQueryLambda

            #region TestDynamic

            //var propertyLength = 1000;
            //dynamic dynamicObject = new SimpleDynamic(propertyLength);
            //var a = dynamicObject.P1;
            ////dynamicObject.SetValue("P1",)
            //System.Console.WriteLine("初值=" + dynamicObject.GetValue("P1"));
            //dynamicObject.P1 = "456";
            //System.Console.WriteLine("直接赋值=" + dynamicObject.GetValue("P1"));
            //var b = dynamicObject.P1;
            //System.Console.WriteLine("再次取值=" + dynamicObject.GetValue("P1"));
            //dynamicObject.SetValue("P1", "789");
            //System.Console.WriteLine("方法赋值=" + dynamicObject.GetValue("P1"));
            //System.Console.WriteLine("方法取值=" + dynamicObject.GetValue("P1"));

            #endregion TestDynamic

            #region TestDbType

            //var dbType = DbType.Oracle;
            //DbType.TryParse("Oracle", out dbType);

            //var dbTypeStr = DbContextFactory.GetProviderName("MySqlConnString");

            //DbType.TryParse(dbTypeStr, out dbType); 

            #endregion

            #region TestBasicData

            IBasicData<DoctorLoginInfo> basicData = new BasicData<DoctorLoginInfo>();

            var ss = basicData
                .Where(d => true)
                .Where(d => d.DoctorInfo_ID == "xxxx")
                .Where(d => d.DoctorLoginInfo_LoginName == "aaaa")
                .OrderBy(d => d.DoctorLoginInfo_ID)
                .OrderBy(d => d.DoctorLoginInfo_Remark)
                .OrderByDescending(d => d.DoctorLoginInfo_CreateTime)
                //.GroupBy(d => new { d.DoctorLoginInfo_Pwd, d.DoctorLoginInfo_LoginName, d.DoctorLoginInfo_Pwd_Temp_Time })
                //.GroupBy(d => d.DoctorLoginInfo_Remark)
                .Skip(1)
                .Take(10)
                .Distinct().Select(d=>d.DoctorLoginInfo_Pwd);
            System.Console.WriteLine(ss);

            #endregion

            System.Console.ReadKey();
        }

        public static void TestOrder<T, TKey>(Expression<Func<T, TKey>> order) where T : class
        {
            var body = order.Body as MemberExpression;
            var b = body.Member.Name;
            var d = b;
            //var x = orderArray;
        }
    }

    public enum DbType
    {
        MsSql,
        MySql,
        Oracle
    }
}