using System;
namespace SysChain.Model
{
	/// <summary>
	/// 系统用户基础信息表
	/// </summary>
	public partial class SysUserInfo
	{
		/// <summary>
		/// 用户编号
		/// </summary>
		public int UserID { set; get; }
		/// <summary>
		/// 性别 1男 0女
		/// </summary>
		public bool Gender { set; get; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public string Name { set; get; }
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
	}
}
