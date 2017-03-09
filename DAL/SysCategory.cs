using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
namespace SysChain.DAL
{
	public class SysCategory
	{
		public SysCategory()
		{
		}
		/// <summary>
		/// 插入品类
		/// </summary>
		/// <returns>The insert.</returns>
		/// <param name="Model">Model.</param>
		public int Insert(Model.SysCategory Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysCategory where Name =@Name ) begin ");
			strSql.Append(" insert into SysCategory(");
			strSql.Append(" ParentID,Name,State,Style,OrderCode)");
			strSql.Append(" values (");
			strSql.Append(" @ParentID,@Name,@State,@Style,@OrderCode)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Bit,1),
					new SqlParameter("@Style", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.ParentID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.State;
			parameters[3].Value = Model.Style;
			parameters[4].Value = Model.OrderCode;
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
		public string GetNewOrderCode(int ParentID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" declare @Code nvarchar(50); ");
			strSql.Append(" select Top 1 @Code=OrderCode From SysCategory ");
			strSql.Append(" Where ParentID=" + ParentID + "");
			strSql.Append(" Order by OrderCode desc;");
			strSql.Append(" if @Code is null ");
			strSql.Append(" select  @Code=OrderCode From SysCategory ");
			strSql.Append(" Where CategoryID=" + ParentID + " ;");
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
							TempCode = Code.Substring(Code.Length - 2, 2);
							int.TryParse(TempCode, out TempNum);
							TempNum = TempNum + 1;
							Code = Code.Substring(0, 2) + TempNum.ToString("00");
							return Code;
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
		/// 设置实体
		/// </summary>
		/// <returns>返回对象实体</returns>
		/// <param name="dr">行</param>
		private Model.SysCategory SetEntity(DataRow dr)
		{
			Model.SysCategory model = new Model.SysCategory();
			if (dr != null)
			{
				if (dr["CategoryID"].ToString() != "")
				{
					model.CategoryID = int.Parse(dr["CategoryID"].ToString());
				}
				if (dr["ParentID"].ToString() != "")
				{
					model.ParentID = int.Parse(dr["ParentID"].ToString());
				}
				model.Name = dr["Name"].ToString();
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
		/// 获得模块实体
		/// </summary>
		/// <returns>返回模块实体</returns>
		/// <param name="CategoryID">模块编号</param>
		public Model.SysCategory GetEntity(int CategoryID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select  top 1 CategoryID,ParentID,Name,Style,OrderCode,State from SysCategory ");
			strSql.Append(" where CategoryID=@CategoryID");
			SqlParameter[] parameters = {
					new SqlParameter("@CategoryID", SqlDbType.Int,4)
			};
			parameters[0].Value = CategoryID;
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
			strSql.Append(" Select count(1) From SysCategory");
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
		public int ModifyModel(Model.SysCategory Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysCategory ");
			strSql.Append("Set Name=@Name,Style=@Style ");
			strSql.Append("Where CategoryID=@CategoryID ");
			SqlParameter[] parameters = {
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Style", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.CategoryID;
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
		public List<Model.SysCategory> GetList(string strWhere, string orderBy)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT CategoryID,ParentID ,Name,State,Style,OrderCode FROM SysCategory ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
			List<Model.SysCategory> ModelList = new List<Model.SysCategory>();
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
		public List<Model.SysCategory> GetListByPage(string strWhere, string OnTable, string orderBy, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT CategoryID,ParentID,Name,State,Style,OrderCode FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderBy.Trim()))
			{
				strSql.Append("order by T." + orderBy);
			}
			else
			{
				strSql.Append("order by T.CategoryID desc");
			}
			strSql.Append(")AS Row, T.*  from SysCategory T ");
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
			List<Model.SysCategory> ModelList = new List<Model.SysCategory>();
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
