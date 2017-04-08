using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
namespace SysChain.DAL
{
	public class SysAttribute
	{
		public SysAttribute()
		{
		}
		/// <summary>
		/// 插入属性
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysAttribute Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysAttribute where Name =@Name ) begin ");
			strSql.Append(" insert into SysAttribute(");
			strSql.Append(" CategoryID,Name,Type,IsImportant)");
			strSql.Append(" values (");
			strSql.Append(" @CategoryID,@Name,@Type,@IsImportant)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@CategoryID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@IsImportant", SqlDbType.Bit,1)};
			parameters[0].Value = Model.CategoryID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.Type;
			parameters[3].Value = Model.IsImportant;
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
		private Model.SysAttribute SetEntity(DataRow dr)
		{
			Model.SysAttribute model = new Model.SysAttribute();
			if (dr != null)
			{
				if (dr["CategoryID"].ToString() != "")
				{
					model.CategoryID = int.Parse(dr["CategoryID"].ToString());
				}
				model.Name = dr["Name"].ToString();
				if (dr["Type"].ToString() != "")
				{
					model.Type = int.Parse(dr["Type"].ToString());
				}
				if (dr["IsImportant"].ToString() != "")
				{
					if ((dr["IsImportant"].ToString() == "1") || (dr["IsImportant"].ToString().ToLower() == "true"))
					{
						model.IsImportant = true;
					}
					else
					{
						model.IsImportant = false;
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
		/// 删除指定属性及值
		/// </summary>
		/// <returns><c>true</c>, if moudle was deled, <c>false</c> otherwise.</returns>
		/// <param name="AttributeID">Moudle identifier.</param>
		public bool DeleAttribute(int AttributeID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Delete SysAttribute  Where AttributeID=@AttributeID ;");
			strSql.Append("Delete SysAttributeValue  Where AttributeID=@AttributeID ;");
			SqlParameter[] parameters = {
					new SqlParameter("@AttributeID", SqlDbType.Int,4)};
			parameters[0].Value = AttributeID;
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
		/// 根据条件获得品类数量
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="strWhere">String where.</param>
		public int GetCount(string strWhere)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" Select count(1) From SysAttribute");
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
		public int ModifyModel(Model.SysAttribute Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysAttribute ");
			strSql.Append("Set Name=@Name,Type=@Type,IsImportant=@IsImportant ");
			strSql.Append("Where AttributeID=@AttributeID ");
			SqlParameter[] parameters = {
					new SqlParameter("@AttributeID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@IsImportant", SqlDbType.Bit,4)
			};
			parameters[0].Value = Model.CategoryID;
			parameters[1].Value = Model.Name;
			parameters[2].Value = Model.Type;
			parameters[3].Value = Model.IsImportant;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysAttribute> GetListByPage(string strWhere, string OnTable, string orderBy, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT AttributeID,CategoryID,Name,Type,IsImportant FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderBy.Trim()))
			{
				strSql.Append("order by T." + orderBy);
			}
			else
			{
				strSql.Append("order by T.AttributeID desc");
			}
			strSql.Append(")AS Row, T.*  from SysAttribute T ");
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
			List<Model.SysAttribute> ModelList = new List<Model.SysAttribute>();
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
