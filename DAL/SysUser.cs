using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
namespace SysChain.DAL
{
	public class SysUser
	{
		public SysUser()
		{
		}
		/// <summary>
		/// 新增系统用户
		/// </summary>
		/// <returns>The add.</returns>
		/// <param name="Model">Model.</param>
		public int Insert(Model.SysUser Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select LoginName from SysUser where LoginName =@LoginName ) begin ");
			strSql.Append(" insert into SysUser(");
			strSql.Append(" LoginName,LoginPassword,ParentID,RoleID,State)");
			strSql.Append(" values (");
			strSql.Append(" @LoginName,@LoginPassword,@ParentID,@RoleID,@State)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@LoginName", SqlDbType.NVarChar,50),
					new SqlParameter("@LoginPassword", SqlDbType.NVarChar,50),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@RoleID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Bit,1)};
			parameters[0].Value = Model.LoginName;
			parameters[1].Value =DBUtility.DESEncrypt.Encrypt(Model.LoginPassword);
			parameters[2].Value = Model.ParentID;
			parameters[3].Value = Model.RoleID;
			parameters[4].Value = Model.State;
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
		/// 修改密码
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="LoginName">登录账号</param>
		/// <param name="LoginPassword">原登录密码</param>
		/// <param name="NewPassword">新设置密码</param>
		public int ModifyPassword(string LoginName,string LoginPassword,string NewPassword)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Declare  @ID int; ");
			strSql.Append("Set @ID =0; ");
			strSql.Append("SELECT @ID=UserID From SysUser where  LoginName =@LoginName and LoginPassword =@LoginPassword ;");
			strSql.Append("if @ID >0 Begin ");
			strSql.Append("Update SysUser Set LoginPassword =@NewPassword Where UserID=@ID  end ;");
			SqlParameter[] parameters = {
					new SqlParameter("@LoginName", SqlDbType.NVarChar,50),
					new SqlParameter("@LoginPassword", SqlDbType.NVarChar,50),
				    new SqlParameter("@NewPassword", SqlDbType.NVarChar,50)};
			parameters[0].Value = LoginName;
			parameters[1].Value = DBUtility.DESEncrypt.Encrypt(LoginPassword);
			parameters[2].Value = DBUtility.DESEncrypt.Encrypt(NewPassword);
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 更新账号状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="UserID">用户编号</param>
		public int UpdateState(int UserID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Declare  @State int; ");
			strSql.Append("SELECT @State=State From SysUser where  UserID=@UserID ;");
			strSql.Append("IF @State = 0 begin  ");
			strSql.Append("Update SysUser Set State =1 Where UserID=@UserID  end;");
			strSql.Append("else  begin  ");
			strSql.Append("Update SysUser Set State =0 Where UserID=@UserID  end ;");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 获得指定系统用户实体
		/// </summary>
		/// <returns>SysUser实体</returns>
		/// <param name="UserID">用户编号.</param>
		public Model.SysUser GetEntity(int UserID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select  top 1 UserID,LoginName,ParentID,RoleID,State from SysUser ");
			strSql.Append(" where UserID=@UserID");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
			parameters[0].Value = UserID;
			DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
			return SetEntity(dt.Rows[0]);
		}
		public Model.SysUser GetEntity(string LoginName,string LoginPassword)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select  top 1 UserID,LoginName,ParentID,RoleID,State from SysUser ");
			strSql.Append(" where LoginName=@LoginName and LoginPassword=@LoginPassword");
			SqlParameter[] parameters = {
					new SqlParameter("@LoginName", SqlDbType.NVarChar,50),
					new SqlParameter("@LoginPassword", SqlDbType.NVarChar,50)
			};
			parameters[0].Value = LoginName;
			parameters[1].Value = DBUtility.DESEncrypt.Encrypt(LoginPassword);
			DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
			if (dt.Rows.Count > 0)
			{
				return SetEntity(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 设置实体
		/// </summary>
		/// <returns>返回对象实体</returns>
		/// <param name="dr">行</param>
		private Model.SysUser SetEntity(DataRow dr)
		{
			Model.SysUser model = new Model.SysUser();
			if (dr != null)
			{
				if (dr["UserID"].ToString() != "")
				{
					model.UserID = int.Parse(dr["UserID"].ToString());
				}
				model.LoginName = dr["LoginName"].ToString();
				if (dr["ParentID"].ToString() != "")
				{
					model.ParentID = int.Parse(dr["ParentID"].ToString());
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
				model.RoleID = int.Parse(dr["RoleID"].ToString());
				return model;
			}
			else
			{
				return null;
			}
		}
	}
}
