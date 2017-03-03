﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Caching;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class HomeController : Controller
	{
		private SysChain.BLL.SysMoudle Opr { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysMoudle(); }
			base.Initialize(requestContext);
		}
		public ActionResult Index()
		{
			ViewBag.Title = "电商后台管理系统";
			return View();
		}
		public ActionResult Header()
		{
			List<Model.SysMoudle> ListModel = new List<Model.SysMoudle>();
			Model.SysUser cm = (Model.SysUser)Session["UserInfo"];
			ListModel = Opr.GetListByRole(cm.RoleID);
			Session["MoudleInfo"] = ListModel;
			List<Model.SysMoudle> Li = new List<Model.SysMoudle>();
			foreach (Model.SysMoudle m in ListModel)
			{
				if (m.ParentID == 0)
				{
					Li.Add(m);
				}

			}
			return PartialView("~/Areas/Admin/Views/Shared/_Header.cshtml",Li);
		}
	}
}
