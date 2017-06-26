using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
namespace SysChain.DAL
{
	public class SysProductBase
	{
		public SysProductBase()
		{
		}
		/// <summary>
		/// 插入产品基本信息
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysProductBase Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Title from SysProductBase where Title =@Title ) begin ");
			strSql.Append(" insert into SysProductBase(");
			strSql.Append(" SPUCode,StoreID,Title,KeyWords,MainPic,GroupID,UnitID,UserID,State,EntryDate)");
			strSql.Append(" values (");
			strSql.Append(" @SPUCode,@StoreID,@Title,@KeyWords,@MainPic,@GroupID,@UnitID,@UserID,@State,@EntryDate)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@SPUCode", SqlDbType.NVarChar,50),
					new SqlParameter("@StoreID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWords", SqlDbType.NVarChar,200),
					new SqlParameter("@MainPic", SqlDbType.NVarChar,200),
					new SqlParameter("@GroupID", SqlDbType.Int,4),
					new SqlParameter("@UnitID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@EntryDate", SqlDbType.DateTime)
			};
			parameters[0].Value = Model.SPUCode;
			parameters[1].Value = Model.StoreID;
			parameters[2].Value = Model.Title;
			parameters[3].Value = Model.KeyWords;
			parameters[4].Value = Model.MainPic;
			parameters[5].Value = Model.GroupID;
			parameters[6].Value = Model.UnitID;
			parameters[7].Value = Model.UserID;
			parameters[8].Value = Model.State;
			parameters[9].Value = Model.EntryDate;
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
		/// 设置实体
		/// </summary>
		/// <returns>返回对象实体</returns>
		/// <param name="dr">行</param>
		private Model.SysProductBase SetEntity(DataRow dr)
		{
			Model.SysProductBase model = new Model.SysProductBase();
			if (dr != null)
			{
				if (dr["ProductID"].ToString() != "")
				{
					model.ProductID = int.Parse(dr["ProductID"].ToString());
				}
				if (dr["SPUCode"].ToString() != "")
				{
					model.SPUCode = dr["SPUCode"].ToString();
				}
				if (dr["StoreID"].ToString() != "")
				{
					model.StoreID = int.Parse(dr["StoreID"].ToString());
				}
				if (dr["Title"].ToString() != "")
				{
					model.Title = dr["Title"].ToString();
				}
				if (dr["KeyWords"].ToString() != "")
				{
					model.KeyWords = dr["KeyWords"].ToString();
				}
				if (dr["MainPic"].ToString() != "")
				{
					model.MainPic = dr["MainPic"].ToString();
				}
				if (dr["GroupID"].ToString() != "")
				{
					model.GroupID = int.Parse(dr["GroupID"].ToString());
				}
				if (dr.Table.Columns.Contains("GroupName"))
				{
					if (dr["GroupName"].ToString() != "")
					{
						model.GroupName = dr["GroupName"].ToString();
					}
				}
				if (dr["UnitID"].ToString() != "")
				{
					model.UnitID = int.Parse(dr["UnitID"].ToString());
				}
				if (dr.Table.Columns.Contains("UnitName"))
				{
					if (dr["UnitName"].ToString() != "")
					{
						model.UnitName = dr["UnitName"].ToString();
					}
				}
				if (dr["UserID"].ToString() != "")
				{
					model.UnitID = int.Parse(dr["UserID"].ToString());
				}
				if (dr.Table.Columns.Contains("UserName"))
				{
					if (dr["UserName"].ToString() != "")
					{
						model.UserName = dr["UserName"].ToString();
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
				if (dr["EntryDate"].ToString() != "")
				{
					model.EntryDate = DateTime.Parse(dr["EntryDate"].ToString());
				}

				return model;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 修改产品基础
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="Model">产品基础实体</param>
		public int ModifyModel(Model.SysProductBase Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysProductBase ");
			strSql.Append("Set Title=@Title,SPUCode=@SPUCode,KeyWords=@KeyWords,MainPic=@MainPic,GroupID=@GroupID,UnitID=@UnitID,UserID=@UserID,State=@State,EntryDate=@EntryDate ");
			strSql.Append("Where ProductID=@ProductID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ProductID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SPUCode", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWords", SqlDbType.NVarChar,200),
					new SqlParameter("@MainPic", SqlDbType.NVarChar,200),
					new SqlParameter("@GroupID", SqlDbType.Int,4),
					new SqlParameter("@UnitID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@EntryDate", SqlDbType.DateTime)
			};
			parameters[0].Value = Model.ProductID;
			parameters[1].Value = Model.Title;
			parameters[2].Value = Model.SPUCode;
			parameters[3].Value = Model.KeyWords;
			parameters[4].Value = Model.MainPic;
			parameters[5].Value = Model.GroupID;
			parameters[6].Value = Model.UnitID;
			parameters[7].Value = Model.UserID;
			parameters[8].Value = Model.State;
			parameters[9].Value = Model.EntryDate;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysProductBase> GetListByPage(string strWhere, string orderBy, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT ProductID,SPUCode,StoreID,Title,KeyWords,MainPic,GroupID,GroupName,UnitID,UnitName,UserID,State,EntryDate FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderBy.Trim()))
			{
				strSql.Append("order by T." + orderBy);
			}
			else
			{
				strSql.Append("order by T.EntryDate desc");
			}
			strSql.Append(")AS Row, T.*  from SysProductBase T ");
			strSql.Append(" inner join  SysGroup as g on T.GroupID=g.GroupID  ");
			strSql.Append(" inner join  SysUserInfo as ui on T.UserID=ui.UserID  ");
			strSql.Append(" inner join  SysUnit as un on T.UnitID=un.UnitID  ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
			List<Model.SysProductBase> ModelList = new List<Model.SysProductBase>();
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
