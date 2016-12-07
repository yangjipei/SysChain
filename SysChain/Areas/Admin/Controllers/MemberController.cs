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
		[HttpGet]
		[LoginCheckFilterAttribute(IsCheck = false)]
		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[LoginCheckFilterAttribute(IsCheck = false)]
		public JsonResult Login(SysChain.Admin.LoginModel model)
		{
			Helper.ResultInfo<bool> rs = new Helper.ResultInfo<bool>();
			rs.Data = true;
			rs.Msg = "登录成功.";
			rs.Result = true;
			rs.Url = Url.Action("Index","Home");
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
		}

	}
}
