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
    [Table("USERLOGININFO")]
	[Serializable]
    public class USERLOGININFO
    {
		public USERLOGININFO()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			USERLOGININFO_ID = "";
			USERINFO_ID = "";
			USERLOGININFO_LOGINNAME = "";
			USERLOGININFO_PWD = "";
			USERLOGININFO_REMARK = "";
			CREATER = "";
			UPDATER = "";
		}

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERLOGININFO_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(11)]
		public DateTime UPDATEDTIME { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(22)]
		public decimal DELETEDSTATUS { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERINFO_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERLOGININFO_LOGINNAME { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERLOGININFO_PWD { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(22)]
		public decimal USERLOGININFO_ENABLE { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(2048)]
		public string USERLOGININFO_REMARK { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(11)]
		public DateTime CREATEDTIME { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string CREATER { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string UPDATER { get; set; }

    }
}
 