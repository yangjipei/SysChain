using System;
namespace SysChain.Model
{
	public class SysGroup
	{
		/// <summary>
		/// 分组编号
		/// </summary>
		public int GroupID { set; get; }
		/// <summary>
		/// 分组归属
		/// </summary>
		public int UserID { set; get; }
		/// <summary>
		/// 分组父级编号
		/// </summary>
		public int ParentID { set; get; }
		/// <summary>
		/// 所在层级
		/// </summary>
		/// <value>The layer.</value>
		public int Layer { set; get; }
		/// <summary>
		/// 分组名称
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// 分组状态
		/// </summary>
		public bool State { set; get; }
		/// <summary>
		/// 分组样式
		/// </summary>
		public string Style { set; get; }
		/// <summary>
		/// 排序编号
		/// </summary>
		public string OrderCode { set; get; }
	}
}
