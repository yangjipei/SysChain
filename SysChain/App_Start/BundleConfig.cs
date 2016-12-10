using System;
using System.Web.Mvc;
using System.Web.Optimization;

namespace SysChain
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/js/tools").Include(
				"~/Content/Public/Scripts/*.js"
			));
			bundles.Add(new ScriptBundle("~/js/login").Include(
				"~/Content/Home/Scripts/Login.js"
			));
			bundles.Add(new ScriptBundle("~/js/admin").Include(
				"~/Content/Admin/Scripts/main.js"
			));
			bundles.Add(new StyleBundle("~/css/home/base").Include(
				"~/Content/Home/Css/main.css"
			));
			bundles.Add(new StyleBundle("~/css/admin/base").Include(
				"~/Content/Admin/Css/main.css"
			));
		    BundleTable.EnableOptimizations = true; 
		}
	}
}
