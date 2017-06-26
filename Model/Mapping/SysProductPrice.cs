using System;
namespace SysChain.Model
{
	public partial class SysProductPrice
	{
		/// <summary>
		/// 产品编号SPU
		/// </summary>
		public int ProductID { set; get; }
		/// <summary>
		/// 属性值=sku
		/// </summary>
		public string ValueID { set; get; }
		/// <summary>
		/// 数量下限
		/// </summary>
		public int lowerLimit { set; get; }
		/// <summary>
		/// 数量上限
		/// </summary>
		public int UpperLimit { set; get; }
		/// <summary>
		/// 产品价格
		/// </summary>		
		public decimal Price { set; get; }

	}
}
