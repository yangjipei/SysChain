using System;
using System.Security.Policy;
using System.Web.Http.Filters;

namespace System.Web.Mvc
{
	public class LoginCheckFilterAttribute:ActionFilterAttribute
	{
		public bool IsCheck { get; set; }
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (IsCheck)
			{
				//校验用户是否已经登录
				if (filterContext.HttpContext.Session["loginUser"] == null)
				{
					string Name = string.Empty;
					var areaName = filterContext.RouteData.DataTokens["area"];
					if (areaName != null)
					{
						 Name = areaName.ToString().ToLower();
					}
					if (Name == "admin")
					{
						//跳转到登陆页
						//filterContext.HttpContext.Response.Redirect("/Admin/Member/Login");
						var Url = new UrlHelper(filterContext.RequestContext);
						var url = Url.Action("Login", "Member", new { area = "Admin" });
						filterContext.Result = new RedirectResult(url);
					}
				}
				//else
				//{
				//	//跳转到首页
				//	filterContext.HttpContext.Response.Redirect("/Home/Index");
				//}
			}
		}

		//public override void OnActionExecuted(ActionExecutedContext filterContext)
		//{
		//	base.OnActionExecuted(filterContext);
		//	filterContext.HttpContext.Response.Write("Action执行之后<br />");
		//}

		//public override void OnResultExecuting(ResultExecutingContext filterContext)
		//{
		//	base.OnResultExecuting(filterContext);
		//	filterContext.HttpContext.Response.Write("返回Result之前<br />");
		//}

		//public override void OnResultExecuted(ResultExecutedContext filterContext)
		//{
		//	base.OnResultExecuted(filterContext);
		//	filterContext.HttpContext.Response.Write("返回Result之后<br />");
		//}
	}
}
