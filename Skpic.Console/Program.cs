using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Skpic.Async;
using Skpic.Common;
using Skpic.DataAccessLayer;
using Skpic.Factory;
using Skpic.IDataAccessLayer;
using Skpic.Model;
using Skpic.SqlMapperExtensions;

namespace Skpic.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            #region test dapper query

            //using (var conn = DataBaseFactory.GetConnection("OracleConnString"))
            //{
            //    var XmlHelper = new XmlHelper(DataBaseFactory.GetProviderName("OracleConnString"));
            //    var sql = XmlHelper.TableObjectSql;
            //    var x = conn.Query(sql);
            //} 
            #endregion

            #region test GetTableCollection

            //var t4 = new T4Manager.T4Manager(@"F:\Code\Skpic.Console\", "ConnectionString");

            //var tableCollection = t4.GetTableCollection();

            //var columnsCollection = t4.GetColumnsCollection();

            #endregion

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

            #endregion

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

            #endregion

            #region TestDelete
            //using (var conn = DbContextFactory.GetConnection())
            //{
            //    var sql = "delete from doctorlogininfo where DoctorLoginInfo_LoginName = '@a0'";
            //    var x = conn.Execute(sql, new { a0 = "Administrator" });
            //}
            #endregion

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

            #endregion

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
            //    var i = conn.Query<DoctorLoginInfo>(d => list.Contains(d.DoctorLoginInfo_ID));
            //}
                //var endsWith = conn.Query<DoctorLoginInfo>(d => d.DoctorLoginInfo_ID.EndsWith("e85ff9d2-84c2-4d51-8287-8e95d443762e"));
                //var contains = conn.Query<DoctorLoginInfo>(d => d.DoctorLoginInfo_ID.Contains("e85ff9d2-84c2-4d51-8287-8e95d443762e"));

                //var containsList1 = conn.Query<DoctorLoginInfo>(d => list.Contains(d.DoctorLoginInfo_ID));

                //i'll fuck this. no no no.
                //var containsList2 = conn.Query<DoctorLoginInfo>(d => list.Any(l => l.Equals(d.DoctorLoginInfo_ID)));

                var y = conn.Query<DoctorLoginInfo>(d => d.DoctorInfo_ID.Equals("c46bc122-8115-4ddd-a041-518658843e57"));
            }

            #endregion

            var propertyLength = 1000;
            dynamic dynamicObject = new SimpleDynamic(propertyLength);
            var a = dynamicObject.P1;
            System.Console.WriteLine("初值="+dynamicObject.GetValue("P1"));
            dynamicObject.P1 = "456";
            System.Console.WriteLine("直接赋值="+dynamicObject.GetValue("P1"));
            var b = dynamicObject.P1;
            System.Console.WriteLine("再次取值=" + dynamicObject.GetValue("P1"));
            dynamicObject.SetValue("P1", "789");
            System.Console.WriteLine("方法赋值=" + dynamicObject.GetValue("P1"));
            System.Console.WriteLine("方法取值="+dynamicObject.GetValue("P1"));
            System.Console.ReadKey();
        }

    }
}
