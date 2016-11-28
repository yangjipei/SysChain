using System;
using System.Collections.Generic;
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
			strSql.Append(" ParentID,Name,LinkUrl,Style,OrderCode,State)");
			strSql.Append(" values (");
			strSql.Append(" @ParentID,@Name,@LinkUrl,@Style,@OrderCode,@State)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@Style", SqlDbType.NVarChar,50),
				    new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Bit,1)};
			parameters[0].Value = Model.ParentID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.LinkUrl;
			parameters[3].Value = Model.Style;
			parameters[4].Value = Model.OrderCode;
			parameters[5].Value = Model.State;
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
					new SqlParameter("@MoudleID", SqlDbType.Int,4)};
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
			strSql.Append("select  top 1 MoudleID,ParentID,Name,LinkUrl,Style,OrderCode,State from SysMoudle ");
			strSql.Append(" where MoudleID=@MoudleID");
			SqlParameter[] parameters = {
					new SqlParameter("@MoudleID", SqlDbType.Int,4)
			};
			parameters[0].Value = MoudleID;
			DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
			return SetEntity(dt.Rows[0]);

		}
		/// <summary>
		/// 生成新的排序Code
		/// </summary>
		/// <returns>The new order code.</returns>
		/// <param name="ParentID">Parent identifier.</param>
		public string GetNewOrderCode(int ParentID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" declare @Code nvarchar(50); ");
			strSql.Append(" select Top 1 @Code=OrderCode From SysMoudle ");
			strSql.Append(" Where ParentID="+ParentID +" ;");
			strSql.Append(" if @Code is null ");
			strSql.Append(" select  @Code=OrderCode From SysMoudle ");
			strSql.Append(" Where MoudleID=" + ParentID + " ;");
			strSql.Append(" select @Code ; ");      
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return "";
			}
			else
			{
				string Code=obj.ToString();
				string TempCode = "";
				if (Code.Length == 2)
				{
					Code = Code + "01";
				}
				else
				{
					TempCode = Code.Substring(Code.Length - 2, 2);
					int TempNum = 0;
					int.TryParse(TempCode, out TempNum);
					TempNum = TempNum + 1;
					Code = Code.Substring(Code.Length - 2) + TempNum.ToString("00");
				}
				return Code;
			}
		}
		/// <summary>
		/// 设置实体
		/// </summary>
		/// <returns>返回对象实体</returns>
		/// <param name="dr">行</param>
		private Model.SysMoudle SetEntity(DataRow dr)
		{
			Model.SysMoudle model = new Model.SysMoudle();
			if (dr!=null)
			{
				if (dr["MoudleID"].ToString() != "")
				{
					model.MoudleID = int.Parse(dr["MoudleID"].ToString());
				}
				if (dr["ParentID"].ToString() != "")
				{
					model.ParentID = int.Parse(dr["ParentID"].ToString());
				}
				model.Name = dr["Name"].ToString();
				model.LinkUrl = dr["LinkUrl"].ToString();
				model.Style = dr["Style"].ToString();
				model.OrderCode = dr["OrderCode"].ToString();
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
		public IEnumerable<Model.SysMoudle> GetListByPage(string strWhere,string OnTable, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT MoudleID,ParentID,Name,LinkUrl,Style,OrderCode,State FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby);
			}
			else
			{
				strSql.Append("order by T.MoudleID desc");
			}
			strSql.Append(")AS Row, T.*  from SysMoudle T ");
			if (!string.IsNullOrEmpty(OnTable.Trim()))
			{
				strSql.Append(OnTable);
			}
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			DataTable dt= DbHelperSQL.Query(strSql.ToString()).Tables[0];
			List<Model.SysMoudle> ModelList = new List<Model.SysMoudle>();
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					ModelList.Add(SetEntity(dr));
				}
				return ModelList;
			}
			else
			{
				return null;
			}
		}
	}
}
