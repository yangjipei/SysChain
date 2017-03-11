using System;
using System.Collections.Generic;

namespace SysChain.BLL
{
	public class SysUser
	{
		private readonly DAL.SysUser dal = new DAL.SysUser();
		public SysUser()
		{
		}
		/// <summary>
		/// 新增系统用户
		/// </summary>
		/// <returns>新增的用户编号,返回值<0:表示是吧</returns>
		/// <param name="Model">用户实体</param>
		public int Insert(Model.VM_SysUser model)
		{
			Model.SysUser m = new Model.SysUser();
			m.LoginName = model.LoginName;
			m.LoginPassword = "mm888888";
			m.ParentID = model.ParentID;
			m.RoleID = model.RoleID;
			m.State = true;
			Model.SysUserInfo ui = new Model.SysUserInfo();
			ui.Gender = model.Gender;
			ui.Department = model.Department;
			ui.Name = model.Name;
			ui.RegisterDate = DateTime.Now;
			ui.Telephone = model.Telephone;
			return dal.Insert(m, ui);
		}
		/// <summary>
		/// 更新用户登陆及基本信息
		/// </summary>
		/// <returns>The update.</returns>
		public int Update(Model.VM_SysUser model)
		{
			Model.SysUser m = new Model.SysUser();
			m.UserID = model.UserID;
			m.LoginName = model.LoginName;
			m.RoleID = model.RoleID;
			Model.SysUserInfo ui = new Model.SysUserInfo();
			ui.Gender = model.Gender;
			ui.Department = model.Department;
			ui.Name = model.Name;
			ui.Telephone = model.Telephone;
			ui.UserID = model.UserID;
			return dal.Update(m, ui);
		}
		/// <summary>
		/// 根据条件获得角色数量
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="strWhere">String where.</param>
		public int GetCount(string strWhere)
		{
			return dal.GetCount(strWhere);
		}
		/// <summary>
		/// 修改密码
		/// </summary>
		/// <returns>返回影响行数</returns>
		/// <param name="LoginName">登录账号</param>
		/// <param name="LoginPassword">原登录密码</param>
		/// <param name="NewPassword">新设置密码</param>
		public int ModifyPassword(string LoginName, string LoginPassword, string NewPassword)
		{
			return dal.ModifyPassword(LoginName,LoginPassword,NewPassword);
		}
		public int ModifyPassword(int UserID)
		{
			return dal.ModifyPassword(UserID);
		}
		/// <summary>
		/// 更新账号状态
		/// </summary>
		/// <returns>大于0表示更新成功</returns>
		/// <param name="UserID">用户编号</param>
		public int UpdateState(int UserID)
		{
			return dal.UpdateState(UserID);
		}
		/// <summary>
		/// 获得指定系统用户实体
		/// </summary>
		/// <returns>SysUser实体</returns>
		/// <param name="UserID">用户编号.</param>
		public Model.VM_SysUser GetEntity(int UserID)
		{
			return dal.GetEntity(UserID);
		}
		/// <summary>
		/// 获得指定系统用户实体
		/// </summary>
		/// <returns>SysUser实体</returns>
		public Model.SysUser GetEntity(Model.VM_SysLogin model)
		{
			return dal.GetEntity(model);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		/// <returns>返回实体集合</returns>
		/// <param name="strWhere">查询条件</param>
		/// <param name="orderby">排序字段</param>
		/// <param name="startIndex">开始数目</param>
		/// <param name="endIndex">结束数目</param>
		public List<Model.VM_SysUser> GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
		}
	}
}
