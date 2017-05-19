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
		public int Insert(Model.SysStroe Model)
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
	}
}
