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
			try
			{
				return View();
			}
			catch (Exception ex)
			{
				ViewBag.Message = ex.Message;
				return View();
			}
		}

	}
}
