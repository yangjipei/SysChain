using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysConfigController : Controller
	{
		public ActionResult  Index()
		{
			ViewBag.Title = "电商后台管理系统";
			List<Model.SysMoudle> ListModel = new List<Model.SysMoudle>();
			ListModel = (List<Model.SysMoudle>)Session["MoudleInfo"];
			List<Model.SysMoudle> Li = new List<Model.SysMoudle>();
			foreach (Model.SysMoudle m in ListModel)
			{
				if (m.ParentID == 2)
				{
					Li.Add(m);
				}

			}
			return View(Li);
		}
	}
}
