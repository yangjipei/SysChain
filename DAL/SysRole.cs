using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
using System.Linq;
namespace SysChain.DAL
{
	public class SysRole
	{
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
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysRole where Name =@Name ) begin ");
			strSql.Append(" insert into SysRole(");
			strSql.Append(" Name,Description,State)");
			strSql.Append(" values (");
			strSql.Append(" @Name,@Description,@State)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Bit,1)};
			parameters[0].Value = Model.Name;
			parameters[1].Value = Model.Description;
			parameters[2].Value = Model.State;
			object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysRole Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysRole ");
			strSql.Append("Set Name=@Name,Description=@Description ");
			strSql.Append("Where RoleID=@RoleID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.RoleID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.Description;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 更新模块状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="RoleID">用户编号</param>
		public int UpdateState(int RoleID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Declare  @State int; ");
			strSql.Append("SELECT @State=State From SysRole where  RoleID=@RoleID ;");
			strSql.Append("IF @State = 0 begin  ");
			strSql.Append("Update SysRole Set State =1 Where RoleID=@RoleID  end;");
			strSql.Append("else  begin  ");
			strSql.Append("Update SysRole Set State =0 Where RoleID=@RoleID  end ;");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)};
			parameters[0].Value = RoleID;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="RoleID">模块编号</param>
		public Model.SysRole GetEntity(int RoleID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select  top 1 RoleID,Name,Description,State from SysRole ");
			strSql.Append(" where RoleID=@RoleID");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)
			};
			parameters[0].Value = RoleID;
			DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
			return SetEntity(dt.Rows[0]);

		}
		/// <summary>
		/// 根据条件获得模块数量
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="strWhere">String where.</param>
		public int GetCount(string strWhere)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" Select count(1) From SysRole");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			object rel = DbHelperSQL.GetSingle(strSql.ToString());
			if (rel != null)
			{
				return (int)rel;
			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// 设置实体
		/// </summary>
		/// <returns>返回对象实体</returns>
		/// <param name="dr">行</param>
		private Model.SysRole SetEntity(DataRow dr)
		{
			Model.SysRole  model = new Model.SysRole();
			if (dr != null)
			{
				if (dr["RoleID"].ToString() != "")
				{
					model.RoleID = int.Parse(dr["RoleID"].ToString());
				}
				model.Name = dr["Name"].ToString();
				model.Description = dr["Description"].ToString();
				if (dr.Table.Columns.Contains("RoleNum"))
				{
					if (dr["RoleNum"].ToString() != "")
					{
						model.RoleNum = int.Parse(dr["RoleNum"].ToString());
					}
				}
				if (dr["State"].ToString() != "")
				{
					if ((dr["State"].ToString() == "1") || (dr["State"].ToString().ToLower() == "true"))
					{
						model.State = true;
					}
					else
					{
						model.State = false;
					}
				}

				return model;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysRole> GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT RoleID,Name,Description,State,RoleNum FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby);
			}
			else
			{
				strSql.Append("order by T.RoleID ");
			}
			strSql.Append(")AS Row, T.* ,");
			strSql.Append(" (select count(1) from SysUser as a  where T.RoleID=a.RoleID)as RoleNum ");
			strSql.Append(" from SysRole T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
			List<Model.SysRole> RoleList = new List<Model.SysRole>();
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					RoleList.Add(SetEntity(dr));
				}
				return RoleList;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 删除指定角色
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="RoleID">Moudle identifier.</param>
		public bool DeleRole(int RoleID)
		{
			List<string> li = new List<string>();
			li.Add(" delete from  SysRoleAndMoudle where RoleID=" + RoleID + " ;");
			li.Add(" delete from  SysRole where RoleID=" + RoleID + " ;");
			int Res= DbHelperSQL.ExecuteSqlTran(li);
			if (Res > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 根据角色ID 获得包含的模块
		/// </summary>
		/// <returns>The moudle IDB y role identifier.</returns>
		/// <param name="RoleID">Role identifier.</param>
		public List<int> GetMoudleIDByRoleID(int RoleID)
		{
			List<int> li = new List<int>();
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" select cast(srm.MoudleID as nvarchar(4))+',' from SysRoleAndMoudle as srm ");
			strSql.Append(" inner join SysMoudle as sm on srm.MoudleID=sm.MoudleID ");
			strSql.Append(" where srm.RoleID="+RoleID);
			strSql.Append(" for xml path('') ");
			object rel = DbHelperSQL.GetSingle(strSql.ToString());
			if (rel != null)
			{
				string temp = rel.ToString().Substring(0, rel.ToString().Length - 1);
				List<string> tp = temp.Split(',').ToList();
				foreach (string i in tp)
				{
					li.Add(int.Parse(i));
				}
			}
			return li;
		}
	}
}

 