using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SysChain.Controllers
{
	[LoginCheckFilterAttribute(IsCheck = false)]
	public class MemberController : Controller
	{
		[HttpGet]
		public ActionResult Login()
		{
			ViewBag.Title = "欢迎登录电商后台管理系统";
			SysChain.Model.VM_SysLogin model = new Model.VM_SysLogin();
			model.LoginName = "admin";
			return View(model);
		}
		[HttpPost]
		public JsonResult Login(SysChain.Model.VM_SysLogin model)
		{
			Helper.ResultInfo<bool> rs = new Helper.ResultInfo<bool>();
			if (ModelState.IsValid)
			{
				BLL.SysUser Operation = new BLL.SysUser();
				SysChain.Model.SysUser user = Operation.GetEntity(model);
				if (user != null)
				{
					if (user.State != true)
					{
						rs.Data = false;
						rs.Msg = "账号已冻结,请联系管理员.";
						rs.Result = false;
						rs.Url = "";
					}
					else
					{
						Session["UserInfo"] = user;
						rs.Data = true;
						rs.Msg = "登录成功,正在进入系统";
						rs.Result = true;
						rs.Url = Url.Action("Index", "Home", new { area = "Admin" });
					}

				}
				else
				{
					rs.Data = false;
					rs.Msg = "账号或密码错误.";
					rs.Result = false;
					rs.Url = "";
				}
				JsonResult jr = new JsonResult();
				jr.Data = rs;
				return jr;
			}
			else
			{
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
		public ActionResult Logout()
		{
			Session["UserInfo"] = null;
			Session["MoudleInfo"] = null;
			return RedirectToAction("Login");
		}
	}
}
