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
	/// 功能表
	/// </summary>
    [Table("QA_Function")]
	[Serializable]
    public class QA_Function
    {
		public QA_Function()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			Function_ID = "";
			Function_UpdateUser = "";
			Function_Order = 1;
			Function_IsShow = true;
			Function_SystemType = "";
			Function_Name = "";
			Function_Url = "";
			PageDetails_ParentId = "";
			PageDetails_ChildId = "";
			Function_Remark = "";
			Function_CreateTime = DateTime.Now;
			Function_CreateUser = "";
			Function_UpdateTime = DateTime.Now;
		}

		/// <summary>
		/// 功能主键
		/// </summary>
		[Key("False")]
        [Length(36)]
		public string Function_ID { get; set; }

		/// <summary>
		/// 更新人
		/// </summary>
        [Length(72)]
		public string Function_UpdateUser { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
        [Length(4)]
		public int? Function_Order { get; set; }

		/// <summary>
		/// 是否显示
		/// </summary>
        [Length(1)]
		public bool? Function_IsShow { get; set; }

		/// <summary>
		/// 系统类别
		/// </summary>
        [Length(128)]
		public string Function_SystemType { get; set; }

		/// <summary>
		/// 功能名称
		/// </summary>
        [Length(128)]
		public string Function_Name { get; set; }

		/// <summary>
		/// 功能路径
		/// </summary>
        [Length(512)]
		public string Function_Url { get; set; }

		/// <summary>
		/// 父页面id
		/// </summary>
		[ForeignKey("PageDetails_ID")]
		[ForeignTable("QA_PageDetails")]
        [Length(36)]
		public string PageDetails_ParentId { get; set; }

		/// <summary>
		/// 子页面id
		/// </summary>
		[ForeignKey("PageDetails_ID")]
		[ForeignTable("QA_PageDetails")]
        [Length(36)]
		public string PageDetails_ChildId { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
        [Length(1024)]
		public string Function_Remark { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
        [Length(8)]
		public DateTime? Function_CreateTime { get; set; }

		/// <summary>
		/// 添加人
		/// </summary>
        [Length(72)]
		public string Function_CreateUser { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
        [Length(8)]
		public DateTime? Function_UpdateTime { get; set; }

    }
}
 