using System;
using System.Collections.Generic;

namespace SysChain.BLL
{
	public class SysRoleAndMoudle
	{
		private DAL.SysRoleAndMoudle dal = new DAL.SysRoleAndMoudle();
		public SysRoleAndMoudle()
		{
		}
		/// <summary>
		/// 插入角色&模块关系
		/// </summary>
		public int Insert(List<Model.SysRoleAndMoudle> ModelList, int RoleID)
		{
			return dal.Insert(ModelList,RoleID);
		}
	}
}
