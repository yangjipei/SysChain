using System;
using System.Web.Routing;

namespace SysChain
{
	public class UnderlineRoute : Route
	{
		public const string UNDERLINE_STRING = "_";
		public const string SUBTRACTION_SIGN = "-";

		public UnderlineRoute(string url, IRouteHandler routeHandler)
			: base(url, routeHandler)
		{
		}

		public UnderlineRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
			: base(url, defaults, routeHandler)
		{
		}

		public UnderlineRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
			: base(url, defaults, constraints, routeHandler)
		{
		}

		public UnderlineRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
			: base(url, defaults, constraints, dataTokens, routeHandler)
		{
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			string action = (string)values["action"];
			if (action != null)
			{
				if (action.Contains(UNDERLINE_STRING))
				{
					values["action"] = action.Replace(UNDERLINE_STRING, SUBTRACTION_SIGN);
				}
			}
			// 以上的这小块代码就是生成 URL 时，把 RequestContext 中的 RouteData 中的 action 参数给替换，
			// 把 action 的名字中带有下划线（_）的替换成减号（-）
			return base.GetVirtualPath(requestContext, values);
		}

		/// <summary>
		/// 替换成下划线
		/// </summary>
		/// <param name="requestContext"></param>
		public static void ReplaceUnderlineToSubtractionSignInRouteData(RequestContext requestContext)
		{
			if (requestContext == null)
			{
				throw new ArgumentNullException("requestContext");
			}
			// 以上的这小块代码就是当处理请求时，把 RequestContext 中的 RouteData 中的 action 参数给替换回来。
			// 把请求的路由中的 action 参数給替换回来，把 action 的名字中带有减号（-）的替换成下划线（_）
			string actionName = (string)requestContext.RouteData.Values["action"];
			if (actionName != null)
			{
				if (actionName.Contains(UnderlineRoute.SUBTRACTION_SIGN))
				{
					requestContext.RouteData.Values["action"] = actionName.Replace(UnderlineRoute.SUBTRACTION_SIGN, UnderlineRoute.UNDERLINE_STRING);
				}
			}
		}
	}
}