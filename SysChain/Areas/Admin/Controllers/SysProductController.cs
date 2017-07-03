using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysProductController : Controller
	{
		private SysChain.BLL.SysProductBase Opr { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysProductBase(); }
			base.Initialize(requestContext);
		}
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult SelectCategory()
		{
			return View();
		}
		[HttpPost]
		public ActionResult New(int id,int id1,int id2)
		{
			ViewBag.ID1 = id1;
			ViewBag.ID2 = id2;
			ViewBag.ID3 = id;
			//获得Post信息
			ViewBag.Kw1 = Request["level1"].ToString();
			ViewBag.Kw2 = Request["level2"].ToString();
			ViewBag.Kw3 = Request["level3"].ToString();
			return View();

		}
		public ActionResult Draft()
		{
			return View();
		}
		public JsonResult GetUnit()
		{
			return Json(Opr.GetUnit(),JsonRequestBehavior.AllowGet);
		}
	}
}
 