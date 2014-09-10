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
using Skpic.SqlMapperExtensions;

namespace Skpic.Model
{
	/// <summary>
	/// 页面表
	/// </summary>
    [Table("QA_PageDetails")]
	[Serializable]
    public class QA_PageDetails
    {
		public QA_PageDetails()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			PageDetails_ID = "";
			PageDetails_Icon = "";
			PageDetails_Name = "";
			PageDetails_Url = "";
			PageDetails_Remark = "";
			PageDetails_CreateTime = DateTime.Now;
			PageDetails_Type = 1;
			PageDetails_SystemType = "1";
			PageDetails_Controller = "";
			PageDetails_Action = "";
		}

		/// <summary>
		/// 页面主键
		/// </summary>
		[Key("False")]
        [Length(36)]
		public string PageDetails_ID { get; set; }

		/// <summary>
		/// 页面图标
		/// </summary>
        [Length(1024)]
		public string PageDetails_Icon { get; set; }

		/// <summary>
		/// 页面排序
		/// </summary>
        [Length(4)]
		public int? PageDetails_Order { get; set; }

		/// <summary>
		/// 页面名称
		/// </summary>
        [Length(128)]
		public string PageDetails_Name { get; set; }

		/// <summary>
		/// 页面地址
		/// </summary>
        [Length(512)]
		public string PageDetails_Url { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
        [Length(1024)]
		public string PageDetails_Remark { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
        [Length(8)]
		public DateTime PageDetails_CreateTime { get; set; }

		/// <summary>
		/// 页面类别
		/// </summary>
        [Length(4)]
		public int? PageDetails_Type { get; set; }

		/// <summary>
		/// 系统类别
		/// </summary>
        [Length(128)]
		public string PageDetails_SystemType { get; set; }

		/// <summary>
		/// 控制器名称
		/// </summary>
        [Length(128)]
		public string PageDetails_Controller { get; set; }

		/// <summary>
		/// 方法名称
		/// </summary>
        [Length(128)]
		public string PageDetails_Action { get; set; }

    }
}
 