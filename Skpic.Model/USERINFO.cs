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
    [Table("USERINFO")]
	[Serializable]
    public class USERINFO
    {
		public USERINFO()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			USERINFO_PHOTO = "";
			USERINFO_REMARK = "";
			CREATER = "";
			UPDATER = "";
			USERINFO_CODE = "";
			USERINFO_NAME = "";
			USERINFO_TITLE = "";
			USERINFO_PHONE = "";
			USERINFO_MOBILE = "";
			USERINFO_EMAIL = "";
		}

		/// <summary>
		/// 
		/// </summary>
        [Length(22)]
		public decimal USERINFO_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERINFO_PHOTO { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(4000)]
		public string USERINFO_REMARK { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string CREATER { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(11)]
		public DateTime CREATEDTIME { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string UPDATER { get; set; }

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
		public string USERINFO_CODE { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(22)]
		public decimal USERINFO_SEX { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERINFO_NAME { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(7)]
		public DateTime USERINFO_BRITHDAY { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERINFO_TITLE { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERINFO_PHONE { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERINFO_MOBILE { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [Length(128)]
		public string USERINFO_EMAIL { get; set; }

    }
}
 