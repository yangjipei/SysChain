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
			bundles.Add(new ScriptBundle("~/js/mnew").Include(
				"~/Content/Admin/Scripts/Moudle_New.js"
			));
			bundles.Add(new ScriptBundle("~/js/mindex").Include(
				"~/Content/Admin/Scripts/Moudle_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/rindex").Include(
				"~/Content/Admin/Scripts/Role_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/rnew").Include(
				"~/Content/Admin/Scripts/Role_New.js"
			));
			bundles.Add(new ScriptBundle("~/js/raccess").Include(
				"~/Content/Admin/Scripts/Role_Access.js"
			));
			bundles.Add(new ScriptBundle("~/js/unew").Include(
				"~/Content/Admin/Scripts/User_New.js"
			));
			bundles.Add(new ScriptBundle("~/js/uindex").Include(
				"~/Content/Admin/Scripts/User_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/cindex").Include(
				"~/Content/Admin/Scripts/Category_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/cnew").Include(
				"~/Content/Admin/Scripts/Category_New.js"
			));
			bundles.Add(new ScriptBundle("~/js/aindex").Include(
				"~/Content/Admin/Scripts/Attribute_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/anew").Include(
				"~/Content/Admin/Scripts/Attribute_New.js"
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
			bundles.Add(new ScriptBundle("~/js/plugin/popwin").Include(
				"~/Content/Public/Plugin/popwin/js/popwin.js",
				"~/Content/Public/Plugin/popwin/js/init.js"
			));
			bundles.Add(new StyleBundle("~/css/plugin/chosen").Include(
				"~/Content/Public/Plugin/chosen/chosen.min.css"
			));
			bundles.Add(new ScriptBundle("~/js/plugin/chosen").Include(
				"~/Content/Public/Plugin/chosen/chosen.jquery.min.js"
			));
		    BundleTable.EnableOptimizations = true; 
		}
	}
}
