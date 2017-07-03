using System;
namespace SysChain.Model
{
	public partial class SysProductBase
	{
		/// <summary>
		/// 产品编号SPU
		/// </summary>
		public int ProductID { set; get; }
		/// <summary>
		/// 一级品类
		/// </summary>
		public int FirCategoryID { set; get; }
		public string FirCategoryName { set; get; }
		/// <summary>
		/// 二级品类
		/// </summary>
		public int SecCategoryID { set; get; }
		public string SecCategoryName { set; get; }
		/// <summary>
		/// 三级品类
		/// </summary>
		public int ThiCategoryID { set; get; }
		public string ThiCategoryName { set; get; }
		/// <summary>
		/// 归属店铺
		/// </summary>
		public int StoreID { set; get; }
		/// <summary>
		/// 产品标题
		/// </summary>
		public string Title { set; get; }
		/// <summary>
		/// 产品关键词
		/// </summary>
		public string KeyWords { set; get; }
		/// <summary>
		/// 产品主图
		/// </summary>
		public string MainPic { set; get; }
		/// <summary>
		/// 产品分组
		/// </summary>
		public int GroupID { set; get; }
		/// <summary>
		/// 销售单位编号
		/// </summary>
		public int UnitID{ set; get; }
		/// <summary>
		/// 录入人
		/// </summary>
		public int UserID { set; get; }
		/// <summary>
		/// 模块状态 0:禁用,1:启用
		/// </summary>
		public bool State { set; get; }
		/// <summary>
		/// 录入时间
		/// </summary>
		public DateTime EntryDate { set; get; }
		/// <summary>
		/// 录入人
		/// </summary>
		/// <value>The name of the user.</value>
		public string UserName { set; get; }
		/// <summary>
		/// 分组名
		/// </summary>
		/// <value>The name of the group.</value>
		public string GroupName { set; get; }
		/// <summary>
		/// 单位名称
		/// </summary>
		/// <value>The name of the unit.</value>
		public string UnitName { set; get; }

	}
}
