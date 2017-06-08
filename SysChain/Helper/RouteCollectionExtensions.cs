using System;
using System.Web.Routing;

namespace SysChain
{
    public static class RouteCollectionExtensions
    {
        #region 扩展 MVC 路由

        public static Route MapRouteUnderline(this RouteCollection routes, string name, string url)
        {
            return routes.MapRouteUnderline(name, url, null, null, null);
        }

        public static Route MapRouteUnderline(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapRouteUnderline(name, url, defaults, null, null);
        }

        public static Route MapRouteUnderline(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return routes.MapRouteUnderline(name, url, null, null, namespaces);
        }

        public static Route MapRouteUnderline(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.MapRouteUnderline(name, url, defaults, constraints, null);
        }

        public static Route MapRouteUnderline(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return routes.MapRouteUnderline(name, url, defaults, null, namespaces);
        }

        public static Route MapRouteUnderline(this RouteCollection routes, 
            string name, 
            string url, 
            object defaults, 
            object constraints, 
            string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            // 注意：
            //       1. 这里使用的 Route 是 UnderlineRoute
            //       2. 这里使用的 RouteHandler 是 UnderlineMvcRouteHandler
            UnderlineRoute item = new UnderlineRoute(url, new UnderlineMvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(namespaces)
            };
            if (namespaces != null && namespaces.Length > 0)
            {
                item.DataTokens["Namespaces"] = namespaces;
            }
            routes.Add(name, item);
            return item;
        }

        #endregion
    }
}