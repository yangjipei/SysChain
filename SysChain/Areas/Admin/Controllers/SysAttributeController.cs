using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysAttributeController : Controller
	{
		private SysChain.BLL.SysAttribute Opr { get; set; }
		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysAttribute(); }
			base.Initialize(requestContext);
		}
		public ActionResult  Index()
		{
			return View();
		}
		public ActionResult List(int? id, int? index )
		{
			int ID = id == null ? 0 : (int)id;
			ViewBag.ID = ID;
			int PageIndex = index == null ? 1 : (int)index;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			ViewBag.Totalcount = Opr.GetCount("");
			List<Model.SysAttribute>  list = Opr.GetListByPage("", "", "AttributeID", (PageIndex - 1) * PageSize, PageIndex * PageSize);
			return View(list);

		}
		public ActionResult New(int id)
		{
			Model.SysAttribute model = new Model.SysAttribute();
			model.CategoryID = id;
			return View(model);
		}
		[HttpPost]
		public ActionResult New(Model.SysAttribute model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			if (ModelState.IsValid)
			{
				if (model.AttributeID > 0)
				{
					rs.Data = Opr.ModifyModel(model);
					if (rs.Data > 0)
					{
						rs.Msg = "修改成功.";
						rs.Result = true;
					}
					else
					{
						rs.Msg = "修改失败.";
						rs.Result = false;
					}
					JsonResult jr = new JsonResult();
					jr.Data = rs;
					return jr;
				}
				else
				{
					rs.Data = Opr.Insert(model);
					if (rs.Data > 0)
					{
						rs.Msg = "新增成功.";
						rs.Result = true;
					}
					else
					{
						rs.Msg = "新增失败.";
						rs.Result = false;
					}
					JsonResult jr = new JsonResult();
					jr.Data = rs;
					return jr;
				}
			}
			else
			{
				System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();
				foreach (var item in ModelState.Values)
				{
					if (item.Errors.Count > 0)
					{
						for (int i = item.Errors.Count - 1; i >= 0; i--)
						{
							sbErrors.Append(item.Errors[i].ErrorMessage);
							sbErrors.Append("<br/>");
						}
					}
				}
				rs.Data = 0;
				rs.Msg = sbErrors.ToString();
				rs.Result = false;
				rs.Url = "";
				JsonResult jr = new JsonResult();
				jr.Data = rs;
				return jr;
			}
		}
	}
}
