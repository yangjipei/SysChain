using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Controllers
{
	public class HomeController : Controller
	{
		

		public ActionResult Index()
		{
			try
			{
				BLL.SysUser Operation = new BLL.SysUser();
				Model.SysUser model= Operation.GetEntity(1);
				return View(model);
			}
			catch (Exception ex)
			{
				ViewBag.Message = ex.Message;
				return View();
			}

		}
	}
}
