using System;
namespace SysChain.Model
{
	public partial class SysCategory
	{
		/// <summary>
		/// 品类编号
		/// </summary>
		public int CategoryID { set; get; }
		/// <summary>
		/// 品类父级编号
		/// </summary>
		public int ParentID { set; get;}
		/// <summary>
		/// 品类名称
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// 品类状态
		/// </summary>
		public bool State { set; get; }
		/// <summary>
		/// 品类样式
		/// </summary>
		public string Style { set; get; }
		/// <summary>
		/// 排序编号
		/// </summary>
		public string OrderCode { set; get; }
	}
}
