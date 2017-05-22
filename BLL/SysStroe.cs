using System;
namespace SysChain.BLL
{
	public class SysStroe
	{
		private readonly DAL.SysStroe dal = new DAL.SysStroe();
		public SysStroe()
		{
		}
		/// <summary>
		/// 插入店铺信息
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysStore Model)
		{
			return dal.Insert(Model);
		}
		/// <summary>
		/// 返回该用户是否存在店铺
		/// </summary>
		/// <returns><c>true</c>, if by user identifier was existsed, <c>false</c> otherwise.</returns>
		/// <param name="UserID">User identifier.</param>
		public bool ExistsByUserID(int UserID)
		{
			return dal.ExistsByUserID(UserID);
		}
		/// <summary>
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysStore Model)
		{
			return dal.ModifyModel(Model);
		}
		/// <summary>
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="StoreID">模块编号</param>
		public Model.SysStore GetEntity(int StoreID)
		{
			return dal.GetEntity(StoreID);
		}
	}
}
