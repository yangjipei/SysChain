using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysRoleController : Controller
	{
		public ActionResult  Index()
		{
			ViewBag.Title = "后端管理系统-角色管理";
			return View();
		}
		public ActionResult RoleAccess()
		{
			ViewBag.Title = "后端管理系统-角色权限配置";
			return View();
		}
	}
}
