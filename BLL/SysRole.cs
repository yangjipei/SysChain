using System;
using System.Collections.Generic;

namespace SysChain.BLL
{
	public class SysRole
	{
		private readonly DAL.SysRole dal = new DAL.SysRole();
		public SysRole()
		{
		}
		/// <summary>
		/// 新增模块
		/// </summary>
		/// <returns>返回新增模块ID</returns>
		/// <param name="Model">模块实体</param>
		public int Insert(Model.SysRole Model)
		{
			return dal.Insert(Model);
		}
		/// <summary>
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysRole Model)
		{
			return dal.ModifyModel(Model);
		}
		/// <summary>
		/// 更新模块状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="RoleID">用户编号</param>
		public int UpdateState(int RoleID)
		{
			return dal.UpdateState(RoleID);
		}
		/// <summary>
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="RoleID">模块编号</param>
		public Model.SysRole GetEntity(int RoleID)
		{
			return dal.GetEntity(RoleID);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		/// <returns>返回实体集合</returns>
		/// <param name="strWhere">查询条件</param>
		/// <param name="orderby">排序字段</param>
		/// <param name="startIndex">开始数目</param>
		/// <param name="endIndex">结束数目</param>
		public IEnumerable<Model.SysRole> GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
		}
	}
}
