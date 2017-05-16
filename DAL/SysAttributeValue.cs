using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SysChain.DBUtility;
namespace SysChain.DAL
{
	public class SysAttributeValue
	{
		public SysAttributeValue()
		{
		}
		/// <summary>
		/// 插入属性值
		/// </summary>
		/// <returns>The insert.</returns>
		public int Insert(Model.SysAttributeValue Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" if not exists(select Name from SysAttributeValue where Name =@Name ) begin ");
			strSql.Append(" insert into SysAttributeValue(");
			strSql.Append(" AttributeID,Name)");
			strSql.Append(" values (");
			strSql.Append(" @AttributeID,@Name)");
			strSql.Append(" ; select @@IDENTITY; ");
			strSql.Append(" end ELSE begin SELECT -1 END");
			SqlParameter[] parameters = {
					new SqlParameter("@AttributeID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.AttributeID;
			parameters[1].Value = Model.Name;
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
		private Model.SysAttributeValue SetEntity(DataRow dr)
		{
			Model.SysAttributeValue model = new Model.SysAttributeValue();
			if (dr != null)
			{
				if (dr["ValueID"].ToString() != "")
				{
					model.ValueID = int.Parse(dr["ValueID"].ToString());
				}
				if (dr["AttributeID"].ToString() != "")
				{
					model.AttributeID = int.Parse(dr["AttributeID"].ToString());
				}
				if (dr["Name"].ToString() != "")
				{
					model.Name = dr["Name"].ToString();
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
		/// <param name="ValueID">Moudle identifier.</param>
		public bool DeleAttributeValue(int ValueID)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Delete SysAttributeValue  Where ValueID=@ValueID ;");
			SqlParameter[] parameters = {
					new SqlParameter("@ValueID", SqlDbType.Int,4)};
			parameters[0].Value = ValueID;
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
			strSql.Append(" Select count(1) From SysAttributeValue");
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
		public int ModifyModel(Model.SysAttributeValue Model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("Update SysAttributeValue ");
			strSql.Append("Set Name=@Name ");
			strSql.Append("Where ValueID=@ValueID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ValueID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50)};
			parameters[0].Value = Model.ValueID;
			parameters[1].Value = Model.Name;
			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public List<Model.SysAttributeValue> GetListByPage(string strWhere, string OnTable, string orderBy, int startIndex, int endIndex)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT ValueID,AttributeID,Name FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderBy.Trim()))
			{
				strSql.Append("order by T." + orderBy);
			}
			else
			{
				strSql.Append("order by T.ValueID desc");
			}
			strSql.Append(")AS Row, T.*  from SysAttributeValue T ");
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
			List<Model.SysAttributeValue> ModelList = new List<Model.SysAttributeValue>();
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
