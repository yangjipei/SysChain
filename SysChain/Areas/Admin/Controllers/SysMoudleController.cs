using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysMoudleController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "后端管理系统-模块管理";
			return View();
		}
		public ActionResult New()
		{
			ViewBag.Title = "后端管理系统-新增模块";
			return View();
		}
	}
}
