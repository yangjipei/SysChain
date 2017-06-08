using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Controllers
{
	[LoginCheckFilterAttribute(IsCheck = false)]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Mall()
		{
			return View();
		}
		public ActionResult Store()
		{
			return View();
		}
		public ActionResult Agent()
		{
			return View();
		}
		public ActionResult Service()
		{
			return View();
		}
		public ActionResult Partner()
		{
			return View();
		}
		public ActionResult About()
		{
			return View();
		}
		public ActionResult ApplyForStore()
		{
			return View();
		}
		public ActionResult ApplyForSupplier()
		{
			return View();
		}
		public ActionResult ComingSoon()
		{
			return View();
		}
	}
}
