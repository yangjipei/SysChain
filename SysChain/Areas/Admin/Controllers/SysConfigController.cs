using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysConfigController : Controller
	{
		public ActionResult  Index()
		{
			ViewBag.Title = "后端管理系统-系统管理";
			return View();
		}
		public ActionResult Moudle()
		{
			ViewBag.Title = "后端管理系统-模块管理";
			return View();
		}
	}
}
