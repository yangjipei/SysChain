using System;
namespace SysChain.Model
{
	public partial class VM_SysUser
	{
		/// <summary>
		/// 序号
		/// </summary>
		public int Row { set; get; }
		/// <summary>
		/// 用户编号
		/// </summary>
		public int UserID { set; get; }
		/// <summary>
		/// 登录账号
		/// </summary>
		public string LoginName { set; get; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public string RoleName { set; get; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// 性别 1男 0女
		/// </summary>
		public bool Gender { set; get; }
		/// <summary>
		/// 电话号码
		/// </summary>
		public string Telephone { set; get; }
		/// <summary>
		/// 部门
		/// </summary>
		public string Department { set; get; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime RegisterDate { set; get; }
		/// <summary>
		/// 账号状态
		/// </summary>
		public bool State { set; get; }
	}
}
