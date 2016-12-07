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
			if (ModelState.IsValid)
			{
				rs.Data = true;
				rs.Msg = "登录成功.";
				rs.Result = true;
				rs.Url = Url.Action("Index", "Home");
				JsonResult jr = new JsonResult();
				jr.Data = rs;
				return jr;
			}
			else {
				System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();
				foreach (var item in ModelState.Values)
				{
					if (item.Errors.Count > 0)
					{
						for (int i = item.Errors.Count - 1; i >= 0; i--)
						{
							sbErrors.Append(item.Errors[i].ErrorMessage);
							sbErrors.Append("<br/>");
						}
					}
				}
				rs.Data = false;
				rs.Msg = sbErrors.ToString();
				rs.Result = false;
				rs.Url = "";
				JsonResult jr = new JsonResult();
				jr.Data = rs;
				return jr;
			}
		}

	}
}
