using System;
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
		public int ParentID { set; get; }
		/// <summary>
		/// 模块名称
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// 模块链接
		/// </summary>
		public string LinkUrl { set; get; }
		/// <summary>
		/// 模块样式
		/// </summary>
		public string Style { set; get; }
		/// <summary>
		/// 模块状态 0:禁用,1:启用
		/// </summary>
		public bool State { set; get; }

	}
}
