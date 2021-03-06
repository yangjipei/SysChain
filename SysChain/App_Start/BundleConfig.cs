﻿using System;
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
				"~/Content/Admin/Scripts/Login.js"
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
			bundles.Add(new ScriptBundle("~/js/umpw").Include(
				"~/Content/Admin/Scripts/User_ModifyPassword.js"
			));
			bundles.Add(new ScriptBundle("~/js/cindex").Include(
				"~/Content/Admin/Scripts/Category_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/cnew").Include(
				"~/Content/Admin/Scripts/Category_New.js"
			));
			bundles.Add(new ScriptBundle("~/js/gindex").Include(
				"~/Content/Admin/Scripts/Group_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/gnew").Include(
				"~/Content/Admin/Scripts/Group_New.js"
			));
			bundles.Add(new ScriptBundle("~/js/pnew").Include(
				"~/Content/Admin/Scripts/Product_New.js"
			));
			bundles.Add(new ScriptBundle("~/js/avindex").Include(
				"~/Content/Admin/Scripts/AttributeValue_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/aindex").Include(
				"~/Content/Admin/Scripts/Attribute_Index.js"
			));
			bundles.Add(new ScriptBundle("~/js/alist").Include(
				"~/Content/Admin/Scripts/Attribute_List.js"
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
			bundles.Add(new StyleBundle("~/css/market/base").Include(
				"~/Content/Market/Css/main.css"
			));
			bundles.Add(new ScriptBundle("~/js/market").Include(
				"~/Content/Market/Scripts/main.js"
			));
			bundles.Add(new ScriptBundle("~/js/plugin/popwin").Include(
				"~/Content/Public/Plugin/popwin/js/popwin.js",
				"~/Content/Public/Plugin/popwin/js/init.js"
			));
			bundles.Add(new ScriptBundle("~/js/plugin/upload").Include(
				"~/Content/Public/Plugin/ajaxupload/ajaxfileupload.js"
			));
			bundles.Add(new ScriptBundle("~/js/plugin/chosen").Include(
				"~/Content/Public/Plugin/chosen/chosen.jquery.min.js"
			));
			bundles.Add(new ScriptBundle("~/js/sseting").Include(
				"~/Content/Admin/Scripts/Store_Setting.js"
			));
			bundles.Add(new ScriptBundle("~/js/plugin/wang").Include(
				"~/Content/Public/Plugin/wangEditor/wangEditor.min.js"
			));
		    //BundleTable.EnableOptimizations = true; 
		}
	}
}
