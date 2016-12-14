using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysUserController : Controller
	{
		public ActionResult  Index()
		{
			ViewBag.Title = "后端管理系统-用户管理";
			return View();
		}
	}
}
