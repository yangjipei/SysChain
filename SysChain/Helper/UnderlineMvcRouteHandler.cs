using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SysChain
{
    public class UnderlineMvcRouteHandler : MvcRouteHandler
    {
        public UnderlineMvcRouteHandler() :base()
        {
        }

        public UnderlineMvcRouteHandler(IControllerFactory controllerFactory): base(controllerFactory)
        {
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            UnderlineRoute.ReplaceUnderlineToSubtractionSignInRouteData(requestContext); // 调用 UnderlineRoute 类处理
            return base.GetHttpHandler(requestContext);
        }
    }
}