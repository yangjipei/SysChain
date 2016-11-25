using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;

namespace SysChain.DAL
{
	public class SysMoudle
	{
		public SysMoudle()
		{}
		/// <summary>
		/// 新增模块
		/// </summary>
		/// <returns>返回新增模块ID</returns>
		/// <param name="Model">模块实体</param>
		public int Insert(Model.SysMoudle Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysMoudle where Name =@Name ) begin ");
			strSql.Append(" insert into SysMoudle(");
			strSql.Append(" ParentID,Name,LinkUrl,Style,State)");
			strSql.Append(" values (");
			strSql.Append(" @ParentID,@Name,@LinkUrl,@Style,@State)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@Style", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Bit,1)};
			parameters[0].Value = Model.ParentID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.LinkUrl;
			parameters[3].Value = Model.Style;
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
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysMoudle Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysMoudle ");
			strSql.Append("Set ParentID=@ParentID,Name=@Name,LinkUrl=@LinkUrl,Style=@Style ");
			strSql.Append("Where MoudleID=@MoudleID ");
			SqlParameter[] parameters = {
					new SqlParameter("@MoudleID", SqlDbType.Int,4),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@Style", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.MoudleID;
			parameters[1].Value = Model.ParentID;
			parameters[2].Value = Model.Name;
			parameters[3].Value = Model.LinkUrl;
			parameters[4].Value = Model.Style;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 更新模块状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="MoudleID">用户编号</param>
		public int UpdateState(int MoudleID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Declare  @State int; ");
			strSql.Append("SELECT @State=State From SysMoudle where  MoudleID=@MoudleID ;");
			strSql.Append("IF @State = 0 begin  ");
			strSql.Append("Update SysMoudle Set State =1 Where MoudleID=@MoudleID  end;");
			strSql.Append("else  begin  ");
			strSql.Append("Update SysMoudle Set State =0 Where MoudleID=@MoudleID  end ;");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = MoudleID;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="MoudleID">模块编号</param>
		public Model.SysMoudle GetEntity(int MoudleID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select  top 1 MoudleID,ParentID,Name,LinkUrl,Style,State from SysMoudle ");
			strSql.Append(" where MoudleID=@MoudleID");
			SqlParameter[] parameters = {
					new SqlParameter("@MoudleID", SqlDbType.Int,4)
			};
			parameters[0].Value = MoudleID;
			DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
			Model.SysMoudle model = new Model.SysMoudle();
			if (dt.Rows.Count > 0)
			{
				if (dt.Rows[0]["MoudleID"].ToString() != "")
				{
					model.MoudleID = int.Parse(dt.Rows[0]["MoudleID"].ToString());
				}
				if (dt.Rows[0]["ParentID"].ToString() != "")
				{
					model.ParentID = int.Parse(dt.Rows[0]["ParentID"].ToString());
				}
				model.Name = dt.Rows[0]["Name"].ToString();
				model.LinkUrl = dt.Rows[0]["LinkUrl"].ToString();
				model.Style = dt.Rows[0]["Style"].ToString();
				if (dt.Rows[0]["State"].ToString() != "")
				{
					if ((dt.Rows[0]["State"].ToString() == "1") || (dt.Rows[0]["State"].ToString().ToLower() == "true"))
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
	}
}
