﻿using System.Web.Mvc;
using System.Web.Routing;

namespace SysChain
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRouteUnderline(
				name: "Default",
				url: "{controller}/{action}/{id}",
				//defaults: new { controller = "Member", action = "Login", id = UrlParameter.Optional },
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				namespaces: new string[] { "SysChain.Controllers" }
			);
		}
	}
}
