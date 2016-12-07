using System;
using System.Web.Mvc;

namespace SysChain
{
	public class FilterConfig
	{
		//这个方法是用于注册全局过滤器（在Global中被调用）
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			//filters.Add(new HandleErrorAttribute());
			filters.Add(new LoginCheckFilterAttribute() { IsCheck = true });
			//压缩
			filters.Add(new CompressAttribute());
		}
	}
}
