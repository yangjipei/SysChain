using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Proxies;
using SysChain.Model;
using System.Data;
namespace SysChain.BLL
{
	public class SysAttribute
	{
		private readonly DAL.SysAttribute dal = new DAL.SysAttribute();
		public SysAttribute()
		{
		}
		/// <summary>
		/// 插入属性
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysAttribute Model)
		{
			return dal.Insert(Model);
		}
		/// <summary>
		/// 删除指定属性及值
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="AttributeID">Moudle identifier.</param>
		public bool DeleAttribute(int AttributeID)
		{
			return dal.DeleAttribute(AttributeID);
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
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysAttribute Model)
		{
			return dal.ModifyModel(Model);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysAttribute> GetListByPage(string strWhere, string OnTable, string orderBy, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere, OnTable, orderBy, startIndex, endIndex);
		}
	}
}
