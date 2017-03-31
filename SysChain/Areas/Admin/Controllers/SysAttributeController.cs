using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysAttributeController : Controller
	{
		public ActionResult  Index()
		{
			return View();
		}
		public ActionResult List(int? id)
		{
			int ID = id == null ? 0 : (int)id;
			ViewBag.ID = ID;
			return View();
		}
	}
}
