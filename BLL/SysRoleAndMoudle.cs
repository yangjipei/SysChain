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
		/// <returns>影响行数>0:为成功,<0:失败</returns></returns>
		/// <param name="ModelList">角色&关系实体</param>
		/// <param name="RoleID">角色ID</param>
		public int Insert(List<Model.SysRoleAndMoudle> ModelList, int RoleID)
		{
			return dal.Insert(ModelList,RoleID);
		}
	}
}
