using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysUserController : Controller
	{
		private SysChain.BLL.SysUser Opr { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysUser(); }
			base.Initialize(requestContext);
		}
		/// <summary>
		/// 首页列表
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="index">Index.</param>
		/// <param name="keywords">Keywords.</param>
		public ActionResult  Index(int? index, string keywords)
		{
			int PageIndex = index == null ? 1 : (int)index;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			string strWhere = string.Empty;
			if (!string.IsNullOrEmpty(keywords))
			{
				strWhere += "  I.Name like '%" + keywords + "%'";
			}
			ViewBag.KeyWords = keywords;
			ViewBag.Totalcount = Opr.GetCount(strWhere);
			List<Model.VM_SysUser> list = Opr.GetListByPage(strWhere, "", (PageIndex - 1) * PageSize+1, PageIndex * PageSize);
			return View(list);
		}
		/// <summary>
		/// 新增／修改 展示页面
		/// </summary>
		/// <returns>The new.</returns>
		/// <param name="id">Identifier.</param>
		public ActionResult New(int? id)
		{
			int UserID = id == null ? 0 : (int)id;
			Model.VM_SysUser model = new Model.VM_SysUser();

			if (UserID > 0)
			{
				model = Opr.GetEntity(UserID);
				ViewBag.Title = "正在编辑用户： " + model.Name;
				ViewBag.RoleID = model.RoleID;
			}
			else
			{
				Model.SysUser cm = (Model.SysUser)Session["UserInfo"];
				model.ParentID = cm.UserID;
				model.UserID = 0;
				model.State = true;
				ViewBag.Title = "后端管理系统-新增系统用户";
			}
			return View(model);
		}
		/// <summary>
		/// 新增／修改操作
		/// </summary>
		/// <returns>The new.</returns>
		/// <param name="model">Model.</param>
		[HttpPost]
		public ActionResult New(Model.VM_SysUser model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			JsonResult jr = new JsonResult();
			if (ModelState.IsValid)
			{
				if (model.UserID > 0)
				{
					//修改
					rs.Data = Opr.Update(model);
					if (rs.Data > 0)
					{
						rs.Msg = "更新成功.";
						rs.Result = true;
					}
					else
					{
						rs.Msg = "更新失败.";
						rs.Result = false;
					}
					jr.Data = rs;
					return jr;
				}
				else
				{
					rs.Data = Opr.Insert(model);
					if (rs.Data > 0)
					{
						rs.Msg = "新增成功.";
						rs.Result = true;
					}
					else
					{
						rs.Msg = "新增失败.";
						rs.Result = false;
					}
					jr.Data = rs;
					return jr;
				}
			}
			else
			{
				System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();
				foreach (var item in ModelState.Values)
				{
					if (item.Errors.Count > 0)
					{
						for (int i = item.Errors.Count - 1; i >= 0; i--)
						{
							sbErrors.Append(item.Errors[i].ErrorMessage);
							sbErrors.Append("<br/>");
						}
					}
				}
				rs.Data = 0;
				rs.Msg = sbErrors.ToString();
				rs.Result = false;
				rs.Url = "";
				jr.Data = rs;
				return jr;
			}
		}
		[HttpPost]
		public ActionResult Freeze(int id)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			rs.Data = Opr.UpdateState(id);
			if (rs.Data > 0)
			{
				rs.Msg = "操作成功.";
				rs.Result = true;
				rs.Url = "";
			}
			else
			{
				rs.Msg = "操作失败.";
				rs.Result = false;
				rs.Url = "";
			}
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
		}
		public ActionResult Reset(int id)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			rs.Data = Opr.ModifyPassword(id);
			if (rs.Data > 0)
			{
				rs.Msg = "重置密码成功.";
				rs.Result = true;
				rs.Url = "";
			}
			else
			{
				rs.Msg = "重置密码失败.";
				rs.Result = false;
				rs.Url = "";
			}
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
		}

		public ActionResult ModifyPassword()
		{
			SysChain.Model.SysUser user = (SysChain.Model.SysUser)Session["UserInfo"];
			Model.VM_SysModifyPassword model = new Model.VM_SysModifyPassword();
			model.LoginName = user.LoginName;
			return View(model);
		}
		/// <summary>
		/// 修改密码
		/// </summary>
		/// <returns>The password.</returns>
		[HttpPost]
		public ActionResult ModifyPassword(Model.VM_SysModifyPassword model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			if(ModelState.IsValid)
			{
				
				rs.Data = Opr.ModifyPassword(model);
				if (rs.Data > 0)
				{
					rs.Msg = "修改密码成功,<br />正在为您跳转到登陆页面.";
					rs.Result = true;
					Session["UserInfo"] = null;
					Session["MoudleInfo"] = null;
					rs.Url = Url.Action("Login", "Member", new { area = "" });
				}
				else
				{
					rs.Msg = "修改失败,原密码可能错误？";
					rs.Result = false;
				}
				JsonResult jr = new JsonResult();
				jr.Data = rs;
				return jr;
			}else{
				System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();
				foreach (var item in ModelState.Values)
				{
					if (item.Errors.Count > 0)
					{
						for (int i = item.Errors.Count - 1; i >= 0; i--)
						{
							sbErrors.Append(item.Errors[i].ErrorMessage);
							sbErrors.Append("<br/>");
						}
					}
				}
				rs.Data = 0;
				rs.Msg = sbErrors.ToString();
				rs.Result = false;
				rs.Url = "";
				JsonResult jr = new JsonResult();
				jr.Data = rs;
				return jr;
			}
		}
	}
}
