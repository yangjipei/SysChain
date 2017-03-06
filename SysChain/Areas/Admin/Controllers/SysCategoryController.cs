using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysCategoryController : Controller
	{
		//private SysChain.BLL.SysCategory Opr { get; set; }

		//protected override void Initialize(RequestContext requestContext)
		//{
		//	if (Opr == null) { Opr = new BLL.SysCategory(); }
		//	base.Initialize(requestContext);
		//}
		public ActionResult Index()
		{
			return View();
		}
	}
}
