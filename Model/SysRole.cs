using System;
using System.ComponentModel.DataAnnotations;
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
		[Required(ErrorMessage = "模块名称必填.")]
		public string Name { set; get; }
		/// <summary>
		/// 使用角色的人数
		/// </summary>
		public int RoleNum { set; get; }
		/// <summary>
		/// 角色描述
		/// </summary>
		[MaxLength(200, ErrorMessage = "最大{0}个字.")]
		public string Description { set; get; }
		/// <summary>
		/// 状态
		/// </summary>
		public bool State { set; get; }
	}
}
