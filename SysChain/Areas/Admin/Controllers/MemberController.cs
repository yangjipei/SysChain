using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class MemberController : Controller
	{
		public ActionResult Login()
		{
			return View();
		}
	}
}
