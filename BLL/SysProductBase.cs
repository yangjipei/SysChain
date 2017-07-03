using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Proxies;
using SysChain.Model;
using System.Data;
namespace SysChain.BLL
{
	public class SysProductBase
	{
		private readonly DAL.SysProductBase dal = new DAL.SysProductBase();
		public SysProductBase()
		{
		}
		/// <summary>
		/// 插入产品基本信息
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysProductBase Model)
		{
			return dal.Insert(Model);
		}
		/// <summary>
		/// 修改产品基础
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">产品基础实体</param>
		public int ModifyModel(Model.SysProductBase Model)
		{
			return dal.ModifyModel(Model);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysProductBase> GetListByPage(string strWhere, string orderBy, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere, orderBy, startIndex, endIndex);
		}
		public List<Model.SysUnit> GetUnit()
		{
			return dal.GetUnit();
		}
	}
}
