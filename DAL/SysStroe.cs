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
		public int Insert(Model.SysStroe Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysStroe where Name =@Name ) begin ");
			strSql.Append(" insert into SysAttribute(");
			strSql.Append(" UserID,Name,LogoLink,Discription)");
			strSql.Append(" values (");
			strSql.Append(" @UserID,@Name,@LogoLink,@Discription)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@LogoLink", SqlDbType.NVarChar,200),
					new SqlParameter("@Discription", SqlDbType.NVarChar,200)};
			parameters[0].Value = Model.UserID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.LogoLink;
			parameters[3].Value = Model.Discription;
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
		/// 返回该用户是否存在店铺
		/// </summary>
		/// <returns><c>true</c>, if by user identifier was existsed, <c>false</c> otherwise.</returns>
		/// <param name="UserID">User identifier.</param>
		public bool ExistsByUserID(int UserID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT Count(1) From SysStroe where  UserID=@UserID ;");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;
			object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
			if (obj != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
