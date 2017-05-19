using System;
namespace SysChain.Model
{
	public partial class SysStroe
	{
		/// <summary>
		/// 店铺编号
		/// </summary>
		public int StroeID { set; get; }
		/// <summary>
		/// 关联账号
		/// </summary>
		public int UserID { set; get; }
		/// <summary>
		/// 店铺名称
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// logo链接
		/// </summary>
		public string LogoLink { set; get; }
		/// <summary>
		/// 店铺描述
		/// </summary>
		public string Discription { set; get; }
	}
}
