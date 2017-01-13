using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysUserController : Controller
	{
		private SysChain.BLL.SysUser Opr { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysUser(); }
			base.Initialize(requestContext);
		}
		public ActionResult  Index(int? index, string keywords)
		{
			ViewBag.Title = "后端管理系统-用户管理";
			int PageIndex = index == null ? 1 : (int)index;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			string strWhere = string.Empty;
			if (!string.IsNullOrEmpty(keywords))
			{
				strWhere += "  I.Name like '%" + keywords + "%'";
			}
			ViewBag.KeyWords = keywords;
			ViewBag.Totalcount = Opr.GetCount(strWhere);
			List<Model.VM_SysUser> list = Opr.GetListByPage(strWhere, "", (PageIndex - 1) * PageSize, PageIndex * PageSize);
			return View(list);
		}
	}
}
