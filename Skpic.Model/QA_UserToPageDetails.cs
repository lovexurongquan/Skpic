using System;
using Skpic.SqlMapperExtensions;

namespace Skpic.Model
{
	/// <summary>
	/// 菜单与页面对照表
	/// </summary>
    [Table("QA_UserToPageDetails")]
	[Serializable]
    public class QA_UserToPageDetails
    {
		public QA_UserToPageDetails()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			PageDetails_ID = "";
			DoctorLoginInfo_ID = "";
			PageFunction_ID = "";
		}

		/// <summary>
		/// 
		/// </summary>
		[ForeignKey("PageDetails_ID")]
		[ForeignTable("QA_PageDetails")]
        [Length(36)]
		public string PageDetails_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[ForeignKey("DoctorLoginInfo_ID")]
		[ForeignTable("DoctorLoginInfo")]
        [Length(36)]
		public string DoctorLoginInfo_ID { get; set; }

		/// <summary>
		/// 主键
		/// </summary>
		[ForeignKey("PageFunction_ID")]
		[ForeignTable("QA_PageFunction")]
        [Length(36)]
		public string PageFunction_ID { get; set; }

    }
}
 