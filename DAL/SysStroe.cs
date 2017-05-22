using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
namespace SysChain.DAL
{
	public class SysStroe
	{
		public SysStroe()
		{
		}
		/// <summary>
		/// 插入店铺信息
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysStore Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysStore where Name =@Name ) begin ");
			strSql.Append(" insert into SysStore(");
			strSql.Append(" UserID,Name,LogoLink,Description,CreateTime)");
			strSql.Append(" values (");
			strSql.Append(" @UserID,@Name,@LogoLink,@Description,@CreateTime)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@LogoLink", SqlDbType.NVarChar,200),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)
			};
			parameters[0].Value = Model.UserID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.LogoLink;
			parameters[3].Value = Model.Description;
			parameters[4].Value = Model.CreateTime;
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
		public int ModifyModel(Model.SysStore Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysStore ");
			strSql.Append("Set LogoLink=@LogoLink,Description=@Description ");
			strSql.Append("Where StoreID=@StoreID ");
			SqlParameter[] parameters = {
					new SqlParameter("@StoreID", SqlDbType.Int,4),
					new SqlParameter("@LogoLink", SqlDbType.NVarChar,200),
					new SqlParameter("@Description", SqlDbType.NVarChar,200)};
			parameters[0].Value = Model.StoreID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.Description;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 返回该用户是否存在店铺
		/// </summary>
		/// <returns><c>true</c>, if by user identifier was existsed, <c>false</c> otherwise.</returns>
		/// <param name="UserID">User identifier.</param>
		public bool ExistsByUserID(int UserID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT Count(1) From SysStore where  UserID=@UserID ;");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;
			object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
			if ((int)obj >0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="StoreID">模块编号</param>
		public Model.SysStore GetEntity(int StoreID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select  top 1 StoreID,Name,LogoLink,Description from SysStore ");
			strSql.Append(" where RoleID=@RoleID");
			SqlParameter[] parameters = {
					new SqlParameter("@StoreID", SqlDbType.Int,4)
			};
			parameters[0].Value = StoreID;
			DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
			return SetEntity(dt.Rows[0]);

		}
		/// <summary>
		/// 设置实体
		/// </summary>
		/// <returns>返回对象实体</returns>
		/// <param name="dr">行</param>
		private Model.SysStore SetEntity(DataRow dr)
		{
			Model.SysStore model = new Model.SysStore();
			if (dr != null)
			{
				if (dr["StoreID"].ToString() != "")
				{
					model.StoreID = int.Parse(dr["StoreID"].ToString());
				}
				model.Name = dr["Name"].ToString();
				model.LogoLink = dr["LogoLink"].ToString();
				model.Description = dr["Description"].ToString();

				return model;
			}
			else
			{
				return null;
			}
		}

	}
}
