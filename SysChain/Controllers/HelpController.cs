using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysChain.Controllers
{
	[LoginCheckFilterAttribute(IsCheck = false)]
    public class HelpController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }
		public ActionResult List()
		{
			return View();
		}
		public ActionResult Detail()
		{
			return View();
		}
		public ActionResult Search()
		{
			return View();
		}
    }
}
