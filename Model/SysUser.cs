using System;
namespace SysChain.Model
{
	/// <summary>
	/// 系统用户表
	/// </summary>
	public partial class SysUser
	{
		/// <summary>
		/// 用户编号
		/// </summary>
		public int UserID { set; get; }
		/// <summary>
		/// 父级编号
		/// </summary>
		public int ParentID { set; get; }
		/// <summary>
		/// 登录账号
		/// </summary>
		public string LoginName { set; get; }
		/// <summary>
		/// 登录密码
		/// </summary>
		public string LoginPassword { set; get; }
		/// <summary>
		/// 角色编号
		/// </summary>
		public int RoleID { set; get; }
		/// <summary>
		/// 账号状态
		/// </summary>
		public bool State { set; get; }

	}
}
