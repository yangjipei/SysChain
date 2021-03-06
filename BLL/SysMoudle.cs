﻿using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Proxies;

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
		/// 根据条件获得模块数量
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="strWhere">String where.</param>
		public int GetCount(string strWhere)
		{
			return dal.GetCount(strWhere);
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
		/// 获得列表
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="strWhere">String where.</param>
		/// <param name="orderBy">Order by.</param>
		public List<Model.SysMoudle> GetList(string strWhere, string orderBy)
		{
			return dal.GetList(strWhere, orderBy);
		}
		/// <summary>
		/// 根据角色获得模块
		/// </summary>
		/// <returns>The list by role.</returns>
		/// <param name="RoleID">Role identifier.</param>
		public List<Model.SysMoudle> GetListByRole(int RoleID)
		{
			return dal.GetListByRole(RoleID);	
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		/// <returns>返回实体集合</returns>
		/// <param name="strWhere">查询条件</param>
		/// <param name="orderby">排序字段</param>
		/// <param name="startIndex">开始数目</param>
		/// <param name="endIndex">结束数目</param>
		public List<Model.SysMoudle> GetListByPage(string strWhere,string OnTable, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere,OnTable, orderby, startIndex, endIndex);
		}
		/// <summary>
		/// 模块排序
		/// </summary>
		/// <returns><c>true</c>, if moudle was ranked, <c>false</c> otherwise.</returns>
		/// <param name="sourceid">Sourceid.</param>
		/// <param name="targetid">Targetid.</param>
		public bool RankMoudle(int sourceid, int targetid)
		{
			return dal.RankMoudle(sourceid, targetid);
		}
		/// <summary>
		/// 删除指定模块
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="MoudleID">Moudle identifier.</param>
		public bool DeleMoudle(int MoudleID)
		{
			return dal.DeleMoudle(MoudleID);
		}
		/// <summary>
		/// 生成模块树
		/// </summary>
		/// <returns>The tree.</returns>
		/// <param name="strWhere">String where.</param>
		/// <param name="orderBy">Order by.</param>
		public List<Model.MoudleForTree> GetTree(string strWhere,string orderBy)
		{
			List<Model.SysMoudle> li = this.GetList(strWhere, orderBy);
			List<Model.MoudleForTree> model = new List<Model.MoudleForTree>();
			foreach (Model.SysMoudle m in li)
			{
				if (m.ParentID == 0)
				{
					Model.MoudleForTree tp = new Model.MoudleForTree();
					tp.Node = m;
					tp.Childers = GetSubTree(li, m.MoudleID);
					model.Add(tp);
				}
			}
			return model;
		}
		private List<Model.SysMoudle> GetSubTree(List<Model.SysMoudle> ml,int ParentID)
		{
			List<Model.SysMoudle> li = new List<Model.SysMoudle>();
			foreach (Model.SysMoudle m in ml)
			{
				if (m.ParentID == ParentID)
				{
					li.Add(m);
				}
			}
			return li;
		}
	}
}
