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
				BLL.SysMoudle Operation = new BLL.SysMoudle();
				ViewBag.Code = Operation.GetNewOrderCode(2);
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
