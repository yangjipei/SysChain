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
		/// 模块名称
		/// </summary>
		public string MoudleName { set; get; }
		/// <summary>
		/// 模块链接
		/// </summary>
		public string MoudleUrl { set; get; }
		/// <summary>
		/// 模块样式
		/// </summary>
		public string MoudleStyle { set; get; }
		/// <summary>
		/// 模块状态 0:禁用,1:启用
		/// </summary>
		public bool MoudleStatus { set; get; }

	}
}
