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
			strSql.Append(" ParentID,Name,LinkUrl,Style,OrderCode,State,MoudleDes)");
			strSql.Append(" values (");
			strSql.Append(" @ParentID,@Name,@LinkUrl,@Style,@OrderCode,@State,@MoudleDes)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@Style", SqlDbType.NVarChar,50),
				    new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Bit,1),
					new SqlParameter("@MoudleDes", SqlDbType.NVarChar,500)};
			parameters[0].Value = Model.ParentID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.LinkUrl;
			parameters[3].Value = Model.Style;
			parameters[4].Value = Model.OrderCode;
			parameters[5].Value = Model.State;
			parameters[6].Value = Model.MoudleDes;
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
			strSql.Append("Set ParentID=@ParentID,Name=@Name,LinkUrl=@LinkUrl,Style=@Style,MoudleDes=@MoudleDes ");
			strSql.Append("Where MoudleID=@MoudleID ");
			SqlParameter[] parameters = {
					new SqlParameter("@MoudleID", SqlDbType.Int,4),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@Style", SqlDbType.NVarChar,50),
					new SqlParameter("@MoudleDes", SqlDbType.NVarChar,500)};
			parameters[0].Value = Model.MoudleID;
			parameters[1].Value = Model.ParentID;
			parameters[2].Value = Model.Name;
			parameters[3].Value = Model.LinkUrl;
			parameters[4].Value = Model.Style;
			parameters[5].Value = Model.MoudleDes;
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
			strSql.Append("select  top 1 MoudleID,ParentID,Name,LinkUrl,Style,OrderCode,State,MoudleDes from SysMoudle ");
			strSql.Append(" where MoudleID=@MoudleID");
			SqlParameter[] parameters = {
					new SqlParameter("@MoudleID", SqlDbType.Int,4)
			};
			parameters[0].Value = MoudleID;
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
			strSql.Append(" Select count(1) From SysMoudle");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			object rel= DbHelperSQL.GetSingle(strSql.ToString());
			if (rel != null)
			{
				return (int)rel;
			}
			else {
				return 0;
			}
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
			strSql.Append(" Where ParentID="+ParentID +" ");
			strSql.Append(" Order by OrderCode desc;");
			strSql.Append(" if @Code is null ");
			strSql.Append(" select  @Code=OrderCode From SysMoudle ");
			strSql.Append(" Where MoudleID=" + ParentID + " ;");
			strSql.Append(" select @Code ; ");      
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return "01";
			}
			else
			{
				string Code = obj.ToString();
				string TempCode = "";
				if (ParentID > 0)
				{
					if (Code.Length == 2)
					{
						return Code + "01";
					}
					else
					{
						TempCode = Code.Substring(Code.Length - 2, 2);
						int TempNum = 0;
						int.TryParse(TempCode, out TempNum);
						TempNum = TempNum + 1;
						Code = Code.Substring(0,2) + TempNum.ToString("00");
						return Code;
					}
				}
				else
				{
					int TempNum = 0;
					int.TryParse(Code, out TempNum);
					TempNum = TempNum + 1;
					Code = TempNum.ToString("00");
					return Code;
				}
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
				model.MoudleDes = dr["MoudleDes"].ToString();
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
		/// 获得列表
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="strWhere">String where.</param>
		/// <param name="orderBy">Order by.</param>
		public List<Model.SysMoudle> GetList(string strWhere, string orderBy)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT MoudleID,ParentID,Name,LinkUrl,Style,OrderCode,State,MoudleDes FROM SysMoudle ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
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

		/// <summary>
		/// 根据角色获得模块
		/// </summary>
		/// <returns>The list by role.</returns>
		/// <param name="RoleID">Role identifier.</param>
		public List<Model.SysMoudle> GetListByRole(int RoleID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT sm.MoudleID,sm.ParentID,sm.Name,sm.LinkUrl,sm.Style,sm.OrderCode,sm.State,sm.MoudleDes FROM SysMoudle as sm");
			strSql.Append(" INNER JOIN SysRoleAndMoudle srm on sm.MoudleID=srm.MoudleID ");
			strSql.Append(" where srm.RoleID=@RoleID");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)};
			parameters[0].Value = RoleID;
			DataTable dt = DbHelperSQL.Query(strSql.ToString(),parameters).Tables[0];
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

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysMoudle> GetListByPage(string strWhere,string OnTable, string orderBy, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT MoudleID,ParentID,Name,LinkUrl,Style,OrderCode,State,MoudleDes FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderBy.Trim()))
			{
				strSql.Append("order by T." + orderBy);
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
		/// <summary>
		/// 模块排序
		/// </summary>
		/// <returns><c>true</c>, if moudle was ranked, <c>false</c> otherwise.</returns>
		/// <param name="sourceid">Sourceid.</param>
		/// <param name="targetid">Targetid.</param>
		public bool RankMoudle(int sourceid, int targetid)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Declare  @sCode nvarchar(50); ");
			strSql.Append("Declare  @tCode nvarchar(50); ");
			strSql.Append("SELECT @sCode=OrderCode From SysMoudle where  MoudleID=@sMoudleID ;");
			strSql.Append("SELECT @tCode=OrderCode From SysMoudle where  MoudleID=@tMoudleID ;");
			strSql.Append("Update SysMoudle Set OrderCode =@tCode Where MoudleID=@sMoudleID  ;");
			strSql.Append("Update SysMoudle Set OrderCode =@sCode Where MoudleID=@tMoudleID  ;");
			SqlParameter[] parameters = {
					new SqlParameter("@sMoudleID", SqlDbType.Int,4),
					new SqlParameter("@tMoudleID", SqlDbType.Int,4)};
			parameters[0].Value = sourceid;
			parameters[1].Value = targetid;
			int rel= DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
			if (rel > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 删除指定模块
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="MoudleID">Moudle identifier.</param>
		public bool DeleMoudle(int MoudleID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Delete SysMoudle  Where MoudleID=@MoudleID or ParentID=@MoudleID ;");
			SqlParameter[] parameters = {
					new SqlParameter("@MoudleID", SqlDbType.Int,4)};
			parameters[0].Value = MoudleID;
			int Rel= DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
			if (Rel > 0)
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
