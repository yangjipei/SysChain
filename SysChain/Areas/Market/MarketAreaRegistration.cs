using System.Web.Mvc;

namespace SysChain.Areas.Market
{
    public class MarketAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Market";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Market_default",
                "Market/{controller}/{action}/{id}",
                new { controller = "Home",action = "Index", id = UrlParameter.Optional },
				new string[] { "SysChain.Areas.Market.Controllers" }
            );
        }
    }
}
