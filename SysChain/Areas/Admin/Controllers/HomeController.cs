using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class HomeController : Controller
	{
		
		public ActionResult Index()
		{
			ViewBag.Title = "后台管理系统";
			return View();
		}
		public ActionResult Header()
		{
			List<Model.SysMoudle> ListModel = new List<Model.SysMoudle>();
			return PartialView("~/Areas/Admin/Views/Shared/_Header.cshtml",ListModel);
		}
	}
}
