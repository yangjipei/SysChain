using System;
namespace SysChain.Model
{
	/// <summary>
	/// 系统角色表
	/// </summary>
	public partial class SysRole
	{
		/// <summary>
		/// 角色编号
		/// </summary>
		public int RoleID { set; get; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public string RoleName { set; get; }
		/// <summary>
		/// 角色描述
		/// </summary>
		public string RoleDesc { set; get; }
	}
}
