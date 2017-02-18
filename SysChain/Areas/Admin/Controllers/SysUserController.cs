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
		public ActionResult  Index(int? index, string keywords)
		{
			ViewBag.Title = "后端管理系统-用户管理";
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
			List<Model.VM_SysUser> list = Opr.GetListByPage(strWhere, "", (PageIndex - 1) * PageSize, PageIndex * PageSize);
			return View(list);
		}
		public ActionResult New(int? id)
		{
			int UserID = id == null ? 0 : (int)id;
			Model.VM_SysUser model = new Model.VM_SysUser();

			if (UserID > 0)
			{
				model = Opr.GetEntity(UserID);
				ViewBag.Title = "正在编辑用户： " + model.Name;
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
	}
}
