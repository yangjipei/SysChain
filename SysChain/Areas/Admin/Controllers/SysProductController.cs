using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysProductController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult SelectCategory()
		{
			return View();
		}
		[HttpPost]
		public ActionResult New(int id)
		{
			ViewBag.ID = id;
			//获得Post信息
			ViewBag.Kw1 = Request["level1"].ToString();
			ViewBag.Kw2 = Request["level2"].ToString();
			ViewBag.Kw3 = Request["level3"].ToString();
			return View();

		}
	}
}
 