﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
namespace SysChain.DAL
{
	public class SysGroup
	{
		public SysGroup()
		{
		}
		/// <summary>
		/// 插入品类
		/// </summary>
		/// <returns>The insert.</returns>
		/// <param name="Model">Model.</param>
		public int Insert(Model.SysGroup Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysGroup where Name =@Name ) begin ");
			strSql.Append(" insert into SysGroup(");
			strSql.Append(" UserID,ParentID,Name,Layer,State,Style,OrderCode)");
			strSql.Append(" values (");
			strSql.Append(" @UserID,@ParentID,@Name,@Layer,@State,@Style,@OrderCode)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Layer", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Bit,1),
					new SqlParameter("@Style", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.UserID;
			parameters[1].Value = Model.ParentID;
			parameters[2].Value = Model.Name;
			parameters[3].Value = Model.Layer;
			parameters[4].Value = Model.State;
			parameters[5].Value = Model.Style;
			parameters[6].Value = Model.OrderCode;
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
		/// 生成新的排序Code
		/// </summary>
		/// <returns>The new order code.</returns>
		/// <param name="ParentID">Parent identifier.</param>
		public string GetNewOrderCode(int ParentID, int Layer)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" declare @Code nvarchar(50); ");
			strSql.Append(" select Top 1 @Code=OrderCode From SysGroup ");
			strSql.Append(" Where ParentID=" + ParentID + "");
			strSql.Append(" Order by OrderCode desc;");
			strSql.Append(" if @Code is null ");
			strSql.Append(" select  @Code=OrderCode From SysGroup ");
			strSql.Append(" Where GroupID=" + ParentID + " ;");
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
				int TempNum = 0;
				if (ParentID > 0)
				{
					switch (Code.Length) 
					{ 
						case 2:
							return Code + "01";
						case 4:
							if (Layer == 2)
							{
								TempCode = Code.Substring(Code.Length - 2, 2);
								int.TryParse(TempCode, out TempNum);
								TempNum = TempNum + 1;
								Code = Code.Substring(0, 2) + TempNum.ToString("00");
								return Code;
							}
							else 
							{
								return Code + "01";
							}
						case 6:
							TempCode = Code.Substring(Code.Length - 2, 2);
							int.TryParse(TempCode, out TempNum);
							TempNum = TempNum + 1;
							Code = Code.Substring(0, 4) + TempNum.ToString("00");
							return Code;
					}
					return Code;
				}
				else
				{
					int.TryParse(Code, out TempNum);
					TempNum = TempNum + 1;
					Code = TempNum.ToString("00");
					return Code;
				}
			}
		}
		/// <summary>
		/// 获得层级
		/// </summary>
		/// <returns>The layer.</returns>
		/// <param name="GroupID">Category identifier.</param>
		public int GetLayer(int GroupID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" select  Layer From SysGroup ");
			strSql.Append(" Where GroupID=" + GroupID + "");
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return (int)obj;
			}
		}
		/// <summary>
		/// 模块排序
		/// </summary>
		/// <returns><c>true</c>, if moudle was ranked, <c>false</c> otherwise.</returns>
		/// <param name="sourceid">Sourceid.</param>
		/// <param name="targetid">Targetid.</param>
		public bool RankCategory(int sourceid, int targetid)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Declare  @sCode nvarchar(50); ");
			strSql.Append("Declare  @tCode nvarchar(50); ");
			strSql.Append("SELECT @sCode=OrderCode From SysGroup where  GroupID=@sGroupID ;");
			strSql.Append("SELECT @tCode=OrderCode From SysGroup where  GroupID=@tGroupID ;");
			//strSql.Append("Update SysGroup Set OrderCode =@tCode Where GroupID=@sGroupID  ;");
			//strSql.Append("Update SysGroup Set OrderCode =@sCode Where GroupID=@tGroupID  ;");
			//更新二级
			strSql.Append("Update SysGroup Set OrderCode =REPLACE(OrderCode,@sCode,'temp') Where OrderCode like @sCode+'%';");
			strSql.Append("Update SysGroup Set OrderCode =REPLACE(OrderCode,@tCode,@sCode) Where OrderCode like @tCode+'%';");
			strSql.Append("Update SysGroup Set OrderCode =REPLACE(OrderCode,'temp',@tCode) Where OrderCode like 'temp%';");

			SqlParameter[] parameters = {
					new SqlParameter("@sGroupID", SqlDbType.Int,4),
					new SqlParameter("@tGroupID", SqlDbType.Int,4)};
			parameters[0].Value = sourceid;
			parameters[1].Value = targetid;
			int rel = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
		/// 更新模块状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="GroupID">品类编号</param>
		public int UpdateState(int GroupID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Declare  @State int; ");
			strSql.Append("SELECT @State=State From SysGroup where  GroupID=@GroupID ;");
			strSql.Append("IF @State = 0 begin  ");
			strSql.Append("Update SysGroup Set State =1 Where GroupID=@GroupID  end;");
			strSql.Append("else  begin  ");
			strSql.Append("Update SysGroup Set State =0 Where GroupID=@GroupID  end ;");
			SqlParameter[] parameters = {
					new SqlParameter("@GroupID", SqlDbType.Int,4)};
			parameters[0].Value = GroupID;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 设置实体
		/// </summary>
		/// <returns>返回对象实体</returns>
		/// <param name="dr">行</param>
		private Model.SysGroup SetEntity(DataRow dr)
		{
			Model.SysGroup model = new Model.SysGroup();
			if (dr != null)
			{
				if (dr["GroupID"].ToString() != "")
				{
					model.GroupID = int.Parse(dr["GroupID"].ToString());
				}
				if (dr["UserID"].ToString() != "")
				{
					model.UserID = int.Parse(dr["UserID"].ToString());
				}
				if (dr["ParentID"].ToString() != "")
				{
					model.ParentID = int.Parse(dr["ParentID"].ToString());
				}
				model.Name = dr["Name"].ToString();
				if (dr["Layer"].ToString() != "")
				{
					model.Layer = int.Parse(dr["Layer"].ToString());
				}
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
		/// 删除指定模块
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="GroupID">Moudle identifier.</param>
		public bool DeleCategory(int GroupID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Delete SysGroup  Where GroupID=@GroupID or ParentID=@GroupID ;");
			SqlParameter[] parameters = {
					new SqlParameter("@GroupID", SqlDbType.Int,4)};
			parameters[0].Value = GroupID;
			int Rel = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
			if (Rel > 0)
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
		/// <param name="GroupID">模块编号</param>
		public Model.SysGroup GetEntity(int GroupID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select  top 1 GroupID,UserID,ParentID,Name,Layer,Style,OrderCode,State from SysGroup ");
			strSql.Append(" where GroupID=@GroupID");
			SqlParameter[] parameters = {
					new SqlParameter("@GroupID", SqlDbType.Int,4)
			};
			parameters[0].Value = GroupID;
			DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
			return SetEntity(dt.Rows[0]);

		}
		/// <summary>
		/// 根据条件获得品类数量
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="strWhere">String where.</param>
		public int GetCount(string strWhere)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" Select count(1) From SysGroup");
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
		/// 修改模块
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">模块实体</param>
		public int ModifyModel(Model.SysGroup Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysGroup ");
			strSql.Append("Set Name=@Name,Style=@Style ");
			strSql.Append("Where GroupID=@GroupID ");
			SqlParameter[] parameters = {
					new SqlParameter("@GroupID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Style", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.GroupID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.Style;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 获得列表
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="strWhere">String where.</param>
		/// <param name="orderBy">Order by.</param>
		public List<Model.SysGroup> GetList(string strWhere, string orderBy)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT GroupID,UserID,ParentID ,Name,Layer,State,Style,OrderCode FROM SysGroup ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			if (!string.IsNullOrEmpty(orderBy.Trim()))
			{
				strSql.Append(" Order by  " + orderBy);
			}

			DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
			List<Model.SysGroup> ModelList = new List<Model.SysGroup>();
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
		public DataTable GetList(string strWhere)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT GroupID,UserID,ParentID , Name,Layer FROM SysGroup ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" Order by  OrderCode ");

			return DbHelperSQL.Query(strSql.ToString()).Tables[0];
		}

		public List<Model.SysGroup> GetModelList(string strWhere)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT GroupID,UserID,ParentID , Name,Layer,Style,OrderCode,State FROM SysGroup ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" Order by  OrderCode " );
			DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
			List<Model.SysGroup> ModelList = new List<Model.SysGroup>();
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
		public List<Model.SysGroup> GetListByPage(string strWhere, string OnTable, string orderBy, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT GroupID,UserID,ParentID,Name,Layer,State,Style,OrderCode FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderBy.Trim()))
			{
				strSql.Append("order by T." + orderBy);
			}
			else
			{
				strSql.Append("order by T.GroupID desc");
			}
			strSql.Append(")AS Row, T.*  from SysGroup T ");
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
			DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
			List<Model.SysGroup> ModelList = new List<Model.SysGroup>();
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
