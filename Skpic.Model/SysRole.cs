using System;
using Skpic.SqlMapperExtensions;

namespace Skpic.Model
{
	/// <summary>
	/// 系统角色表
	/// </summary>
    [Table("SysRole")]
	[Serializable]
    public class SysRole
    {
		public SysRole()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			SysRole_ID = "";
			SysRole_Name = "";
		}

		/// <summary>
		/// 角色ID
		/// </summary>
		[Key("False")]
        [Length(36)]
		public string SysRole_ID { get; set; }

		/// <summary>
		/// 角色名称
		/// </summary>
        [Length(100)]
		public string SysRole_Name { get; set; }

		/// <summary>
		/// 目前无用
		/// </summary>
        [Length(1)]
		public bool SysRole_Enable { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
        [Length(4)]
		public int SysRole_Order { get; set; }

		/// <summary>
		/// 目前无用
		/// </summary>
        [Length(1)]
		public bool SysRole_IsAdmin { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
        [Length(8)]
		public DateTime SysRole_CreateTime { get; set; }

    }
}
 