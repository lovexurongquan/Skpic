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
	/// 页面功能表
	/// </summary>
    [Table("QA_PageFunction")]
	[Serializable]
    public class QA_PageFunction
    {
		public QA_PageFunction()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			PageFunction_ID = "";
			PageFunction_Name = "";
			PageFunction_Method = "";
			PageFunction_Remark = "";
			PageFunction_CreateTime = DateTime.Now;
			PageFunction_SystemType = "";
		}

		/// <summary>
		/// 主键
		/// </summary>
		[Key("False")]
        [Length(36)]
		public string PageFunction_ID { get; set; }

		/// <summary>
		/// 功能名称
		/// </summary>
        [Length(128)]
		public string PageFunction_Name { get; set; }

		/// <summary>
		/// 功能执行的方法
		/// </summary>
        [Length(512)]
		public string PageFunction_Method { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
        [Length(1024)]
		public string PageFunction_Remark { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
        [Length(8)]
		public DateTime PageFunction_CreateTime { get; set; }

		/// <summary>
		/// 功能类别
		/// </summary>
        [Length(4)]
		public int? PageFunction_Type { get; set; }

		/// <summary>
		/// 系统类别
		/// </summary>
        [Length(128)]
		public string PageFunction_SystemType { get; set; }

		/// <summary>
		/// 功能排序
		/// </summary>
        [Length(4)]
		public int? PageFunction_Order { get; set; }

    }
}
 