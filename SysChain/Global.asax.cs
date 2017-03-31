using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Optimization;
using System;

namespace SysChain
{
	public class Global : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
		protected void Application_Error(object sender, EventArgs e)
		{
			//Exception exception = Server.GetLastError();
			//HttpException httpException = exception as HttpException;
			//var routingData = Context.Request.RequestContext.RouteData;
			//var areaName = routingData.DataTokens["area"];
			//string RouteName = string.Empty;
			//Object RouteData = new object();
			//if (areaName != null)
			//{
			//	RouteName = "Admin_default";
			//}
			//switch (httpException.GetHttpCode())
			//{
			//	case 404:
			//		RouteData=new {controller = "Public", action = "Err404"};
			//		break;
			//	case 500:
			//		RouteData = new { controller = "Public", action = "Err500" };
			//	break;
			//}
			//Response.Clear();
			//Server.ClearError();
			//Response.TrySkipIisCustomErrors = true;
			//Response.RedirectToRoute(RouteName, RouteData);
		}
	}
}
