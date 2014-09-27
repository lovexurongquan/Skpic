/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：don't edit this model.
 * if you edit, it will lose.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using Skpic.Async.Attributes;

namespace Skpic.Model
{
	/// <summary>
	/// 
	/// </summary>
    [Table("DoctorLoginInfo")]
	[Serializable]
    public class DoctorLoginInfo
    {
		public DoctorLoginInfo()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			DoctorLoginInfo_ID = "";
			DoctorInfo_ID = "";
			DoctorLoginInfo_LoginName = "";
			DoctorLoginInfo_Pwd = "";
			DoctorLoginInfo_Enable = true;
			DoctorLoginInfo_Remark = "";
			DoctorLoginInfo_CreateTime = DateTime.Now;
			DoctorLoginInfo_Pwd_Temp = "";
		}

		/// <summary>
		/// 
		/// </summary>
		[Key("False")]
        [Length(36)]
		public string DoctorLoginInfo_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(36)]
		public string DoctorInfo_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(50)]
		public string DoctorLoginInfo_LoginName { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(50)]
		public string DoctorLoginInfo_Pwd { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(1)]
		public bool? DoctorLoginInfo_Enable { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(600)]
		public string DoctorLoginInfo_Remark { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(8)]
		public DateTime? DoctorLoginInfo_CreateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(50)]
		public string DoctorLoginInfo_Pwd_Temp { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(8)]
		public DateTime? DoctorLoginInfo_Pwd_Temp_Time { get; set; }

    }
}
 