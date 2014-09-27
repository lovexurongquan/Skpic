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
	/// 角色与页面对照表
	/// </summary>
    [Table("QA_SysRoleToPageDetails")]
	[Serializable]
    public class QA_SysRoleToPageDetails
    {
		public QA_SysRoleToPageDetails()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			PageDetails_ID = "";
			SysRole_ID = "";
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
		/// 角色ID
		/// </summary>
		[ForeignKey("SysRole_ID")]
		[ForeignTable("SysRole")]
        [Length(36)]
		public string SysRole_ID { get; set; }

		/// <summary>
		/// 主键
		/// </summary>
		[ForeignKey("PageFunction_ID")]
		[ForeignTable("QA_PageFunction")]
        [Length(36)]
		public string PageFunction_ID { get; set; }

    }
}
 