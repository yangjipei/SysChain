using System;
namespace SysChain.Model
{
	public partial class SysProductAttributes
	{
		/// <summary>
		/// 产品编号SPU
		/// </summary>
		public int ProductID { set; get; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public int AttributeID{ set; get; }
		/// <summary>
		/// 属性值
		/// </summary>
		public int ValueID { set; get; }
	}
}
