using System;
using System.ComponentModel.DataAnnotations;
namespace SysChain.Model
{
	/// <summary>
	/// 系统模块表
	/// </summary>
	public partial class SysMoudle
	{
		/// <summary>
		/// 模块编号
		/// </summary>
		public int MoudleID { set; get; }
		/// <summary>
		/// 父级编号
		/// </summary>
		[Required(ErrorMessage = "上级模块必选.")]
		public int ParentID { set; get; }
		/// <summary>
		/// 模块名称
		/// </summary>
		[Required(ErrorMessage ="模块名称必填.")]
		public string Name { set; get; }
		/// <summary>
		/// 模块链接
		/// </summary>
		[Required(ErrorMessage = "模块链接必填.")]
		public string LinkUrl { set; get; }
		/// <summary>
		/// 模块样式
		/// </summary>
		public string Style { set; get; }
		/// <summary>
		/// 排序编码
		/// </summary>
		public string OrderCode { set; get; }
		/// <summary>
		/// 模块状态 0:禁用,1:启用
		/// </summary>
		public bool State { set; get; }
		/// <summary>
		/// 模块描述
		/// </summary>
		[MaxLength(200,ErrorMessage ="最大{0}个字.")]
		public string MoudleDes { set; get; }

	}
}
