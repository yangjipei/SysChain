using System;
using System.Collections.Generic;
namespace SysChain.BLL
{
	public class SysAttributeValue
	{
		private readonly DAL.SysAttributeValue dal = new DAL.SysAttributeValue();
		public SysAttributeValue()
		{
		}
		/// <summary>
		/// 插入属性值
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysAttributeValue Model)
		{
			return dal.Insert(Model);
		}
		/// <summary>
		/// 删除指定属性及值
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="ValueID">Moudle identifier.</param>
		public bool DeleAttributeValue(int ValueID)
		{
			return dal.DeleAttributeValue(ValueID);
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
		public int ModifyModel(Model.SysAttributeValue Model)
		{
			return dal.ModifyModel(Model);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysAttributeValue> GetListByPage(string strWhere, string OnTable, string orderBy, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere, OnTable, orderBy, startIndex, endIndex);
		}
	}
}
