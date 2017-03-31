using System;
namespace SysChain.Model
{
	public partial class SysAttributeValue
	{
		/// <summary>
		/// 属性值ID
		/// </summary>
		public int ListID { set; get; }
		/// <summary>
		/// 属性编号
		/// </summary>
		public int AttributeID { set; get; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public string Name { set; get; }
	}
}
