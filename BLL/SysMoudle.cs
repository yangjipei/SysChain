using System;
using System.Collections.Generic;

namespace SysChain.BLL
{
	public class SysMoudle
	{
		private readonly DAL.SysMoudle dal = new DAL.SysMoudle();
		public SysMoudle()
		{
		}
		/// <summary>
		/// 新增模块
		/// </summary>
		/// <returns>返回新增模块ID</returns>
		/// <param name="Model">模块实体</param>
		public int Insert(Model.SysMoudle Model)
		{
			return dal.Insert(Model);
		}
		/// <summary>
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysMoudle Model)
		{
			return dal.ModifyModel(Model);
		}
		/// <summary>
		/// 生成新的排序Code
		/// </summary>
		/// <returns>The new order code.</returns>
		/// <param name="ParentID">Parent identifier.</param>
		public string GetNewOrderCode(int ParentID)
		{
			return dal.GetNewOrderCode(ParentID);
		}
		/// <summary>
		/// 更新模块状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="MoudleID">用户编号</param>
		public int UpdateState(int MoudleID)
		{
			return dal.UpdateState(MoudleID);
		}
		/// <summary>
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="MoudleID">模块编号</param>
		public Model.SysMoudle GetEntity(int MoudleID)
		{
			return dal.GetEntity(MoudleID);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		/// <returns>返回实体集合</returns>
		/// <param name="strWhere">查询条件</param>
		/// <param name="orderby">排序字段</param>
		/// <param name="startIndex">开始数目</param>
		/// <param name="endIndex">结束数目</param>
		public IEnumerable<Model.SysMoudle> GetListByPage(string strWhere,string OnTable, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere,OnTable, orderby, startIndex, endIndex);
		}
	}
}
