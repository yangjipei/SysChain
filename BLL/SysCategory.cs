using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Proxies;
using SysChain.Model;
using System.Data;
namespace SysChain.BLL
{
	public class SysCategory
	{
		private readonly DAL.SysCategory dal = new DAL.SysCategory();
		public SysCategory()
		{
		}
		/// <summary>
		/// 新增模块
		/// </summary>
		/// <returns>返回新增模块ID</returns>
		/// <param name="Model">品类实体</param>
		public int Insert(Model.SysCategory Model)
		{
			return dal.Insert(Model);
		}
		/// <summary>
		/// 根据条件获得品类数量
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="strWhere">String where.</param>
		public int GetCount(string strWhere)
		{
			return dal.GetCount(strWhere);
		}
		/// <summary>
		/// 删除指定模块
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="CategoryID">Moudle identifier.</param>
		public bool DeleCategory(int CategoryID)
		{
			return dal.DeleCategory(CategoryID);
		}
		/// <summary>
		/// 品类排序
		/// </summary>
		/// <returns><c>true</c>, if moudle was ranked, <c>false</c> otherwise.</returns>
		/// <param name="sourceid">Sourceid.</param>
		/// <param name="targetid">Targetid.</param>
		public bool RankCategory(int sourceid, int targetid)
		{
			return dal.RankCategory(sourceid, targetid);
		}
		/// <summary>
		/// Gets the list.
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="strWhere">String where.</param>
		public List<VM_SysCategory> GetList(string strWhere)
		{
			System.Data.DataTable dt = dal.GetList(strWhere);
			List<VM_SysCategory> li = CreateTree(dt, 0);
			return li;
		}
		private List<VM_SysCategory> CreateTree(DataTable dt, int ParentID)
		{
			List<VM_SysCategory> li = new List<VM_SysCategory>();
			DataRow[] dr = dt.Select("ParentID=" + ParentID);
			foreach (DataRow d in dr)
			{
				VM_SysCategory m = new VM_SysCategory();
				m.CategoryID = int.Parse(d["CategoryID"].ToString()) ;
				m.CategoryName = d["Name"].ToString();
				m.ParentID = int.Parse(d["ParentID"].ToString());
				m.Layer = int.Parse(d["Layer"].ToString());
				if (m.Layer < 3)
				{
					m.Children = CreateTree(dt, m.CategoryID);
				}
				li.Add(m);
			}
			return li;
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysCategory> GetListByPage(string strWhere, string OnTable, string orderBy, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere, OnTable, orderBy, startIndex, endIndex);
		}
		/// <summary>
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="CategoryID">品类编号</param>
		public Model.SysCategory GetEntity(int CategoryID)
		{
			return dal.GetEntity(CategoryID);
		}
		/// <summary>
		/// 生成新的排序Code
		/// </summary>
		/// <returns>The new order code.</returns>
		/// <param name="ParentID">Parent identifier.</param>
		public string GetNewOrderCode(int ParentID, int level)
		{
			return dal.GetNewOrderCode(ParentID, level);
		}
		/// <summary>
		/// 获得层级
		/// </summary>
		/// <returns>The layer.</returns>
		/// <param name="CategoryID">Category identifier.</param>
		public int GetLayer(int CategoryID)
		{
			return dal.GetLayer(CategoryID);
		}
		/// <summary>
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysCategory Model)
		{
			return dal.ModifyModel(Model);
		}
		/// <summary>
		/// 获得列表
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="strWhere">String where.</param>
		/// <param name="orderBy">Order by.</param>
		public List<Model.SysCategory> GetList(string strWhere, string orderBy)
		{
			return dal.GetList(strWhere, orderBy);
		}
		/// <summary>
		/// 更新模块状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="CategoryID">用户编号</param>
		public int UpdateState(int CategoryID)
		{
			return dal.UpdateState(CategoryID);
		}
	}
}
