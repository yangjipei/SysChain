using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class StoreController : Controller
	{
		private SysChain.BLL.SysStroe Opr { get; set; }
		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysStroe(); }
			base.Initialize(requestContext);
		}
		public ActionResult Index()
		{
			SysChain.Model.SysUser user = (SysChain.Model.SysUser)Session["UserInfo"];
			if(!Opr.ExistsByUserID(user.UserID))
			{
				return RedirectToAction("Setting");
			}else
			{
				return View();
			}
		}
		public ActionResult Setting()
		{
			return View();
		}
	}
}
