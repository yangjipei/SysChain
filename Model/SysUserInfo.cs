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
		/// 真实姓名
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// 电话号码
		/// </summary>
		public string Telphone { set; get; }
		/// <summary>
		/// 联系地址
		/// </summary>
		public string Address { set; get; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime RegisterDate { set; get; }
	}
}
