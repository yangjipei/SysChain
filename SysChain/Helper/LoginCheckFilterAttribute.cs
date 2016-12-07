using System;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace System.Web.Mvc
{
	public class AuthorizationFilter:ActionFilterAttribute
	{
		public bool IsCheck { get; set; }
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			filterContext.HttpContext.Response.Write("Action执行之前" + Message + "<br />");
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			filterContext.HttpContext.Response.Write("Action执行之后" + Message + "<br />");
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			base.OnResultExecuting(filterContext);
			filterContext.HttpContext.Response.Write("返回Result之前" + Message + "<br />");
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			base.OnResultExecuted(filterContext);
			filterContext.HttpContext.Response.Write("返回Result之后" + Message + "<br />");
		}
	}
}
