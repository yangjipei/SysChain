using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;

namespace SysChain.DAL
{
	public class SysRoleAndMoudle
	{
		public SysRoleAndMoudle()
		{
		}
		/// <summary>
		/// 插入角色&模块关系
		/// </summary>
		/// <returns>影响行数>0为成功,小于0失败</returns></returns>
		/// <param name="ModelList">角色&关系实体</param>
		/// <param name="RoleID">角色ID</param>
		public int Insert(List<Model.SysRoleAndMoudle> ModelList,int RoleID)
		{
			List<string> li = new List<string>();
			li.Add(" delete from  SysRoleAndMoudle where RoleID="+RoleID+" ;");
			foreach (Model.SysRoleAndMoudle m in ModelList)
			{
				li.Add(String.Format(" insert into SysRoleAndMoudle(RoleID,MoudleID) values ({0},{1}) ;",m.RoleID,m.MoudleID));
			}
			return DbHelperSQL.ExecuteSqlTran(li);
		}
	}
}
