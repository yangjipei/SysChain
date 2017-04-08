using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysAttributeController : Controller
	{
		private SysChain.BLL.SysAttribute Opr { get; set; }
		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysAttribute(); }
			base.Initialize(requestContext);
		}
		public ActionResult  Index()
		{
			return View();
		}
		public ActionResult List(int? id, int? index )//
		{
			int ID = id == null ? 0 : (int)id;
			ViewBag.ID = ID;
			int PageIndex =  index == null ? 1 : (int)index;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			ViewBag.Totalcount = Opr.GetCount("");
			List<Model.SysAttribute> list = Opr.GetListByPage("", "", "AttributeID", (PageIndex - 1) * PageSize, PageIndex * PageSize);
			return View(list);
		}
	}
}
