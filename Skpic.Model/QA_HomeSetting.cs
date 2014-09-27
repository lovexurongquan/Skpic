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
    [Table("QA_HomeSetting")]
	[Serializable]
    public class QA_HomeSetting
    {
		public QA_HomeSetting()
		{
			SetDefaultValue();
		}

		/// <summary>
        /// set default by database.
        /// </summary>
		public void SetDefaultValue()
		{
			HomeSetting_ID = "";
			HomeSetting_Type = "";
			HomeSetting_Content = "";
			HomeSetting_Url = "";
			HomeSetting_Remark = "";
			HomeSetting_UrlParam = "";
			HomeSetting_TipContent = "";
		}

		/// <summary>
		/// 配置主键
		/// </summary>
		[Key("False")]
        [Length(36)]
		public string HomeSetting_ID { get; set; }

		/// <summary>
		/// 配置类别
		/// </summary>
        [Length(328)]
		public string HomeSetting_Type { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
        [Length(4096)]
		public string HomeSetting_Content { get; set; }

		/// <summary>
		/// 图片路径
		/// </summary>
        [Length(16)]
		public byte[] HomeSetting_Img { get; set; }

		/// <summary>
		/// 选中时访问路径
		/// </summary>
        [Length(1024)]
		public string HomeSetting_Url { get; set; }

		/// <summary>
		/// 是否显示
		/// </summary>
        [Length(4)]
		public int? HomeSetting_IsShow { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
        [Length(2048)]
		public string HomeSetting_Remark { get; set; }

		/// <summary>
		/// 跳转页面参数
		/// </summary>
        [Length(36)]
		public string HomeSetting_UrlParam { get; set; }

		/// <summary>
		/// 跳转页面提示
		/// </summary>
        [Length(128)]
		public string HomeSetting_TipContent { get; set; }

    }
}
 