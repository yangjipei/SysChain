using System;
using System.ComponentModel.DataAnnotations;

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
		/// 归属编号
		/// </summary>
		public int ParentID { set; get; }
		/// <summary>
		/// 登录账号
		/// </summary>
		[Required(ErrorMessage = "登录账号不能为空")]
		public string LoginName { set; get; }
		/// <summary>
		/// 角色名称
		/// </summary>
		[Required(ErrorMessage = "角色不能为空")]
		public int RoleID { set; get; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public string RoleName { set; get; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		[Required(ErrorMessage = "用户姓名不能为空")]
		public string Name { set; get; }
		/// <summary>
		/// 性别 1男 0女
		/// </summary>
		[Required(ErrorMessage = "请选择性别")]
		public bool Gender { set; get; }
		/// <summary>
		/// 电话号码
		/// </summary>
		[Required(ErrorMessage = "电话号码不能为空")]
		public string Telephone { set; get; }
		/// <summary>
		/// 部门
		/// </summary>
		[Required(ErrorMessage = "用户所在部门不能为空")]
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
