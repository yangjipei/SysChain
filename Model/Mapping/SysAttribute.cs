using System;
namespace SysChain.Model
{
	public partial class SysAttribute
	{
		/// <summary>
		/// 属性编号
		/// </summary>
		public int AttributeID { set; get; }
		/// <summary>
		/// 品类编号
		/// </summary>
		public int CategoryID { set; get; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// 属性类型
		/// </summary>
		public int Type { set; get; }
		/// <summary>
		/// 是否会影响价格
		/// </summary>
		public bool IsImportant { set; get; }
	}
}
