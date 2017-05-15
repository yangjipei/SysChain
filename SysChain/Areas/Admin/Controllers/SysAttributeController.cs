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
		public ActionResult List(int id, int? index,string keywords )
		{
			int ID =id;
			ViewBag.ID = ID;
			int PageIndex = index == null ? 1 : (int)index;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			string strWhere = "CategoryID=" + ID;
			if (!string.IsNullOrEmpty(keywords))
			{
				strWhere += "  And Name like '%" + keywords + "%'";
			}
			//获得Post信息
			ViewBag.Kw1 = Request["level1"].ToString();
			ViewBag.Kw2 = Request["level2"].ToString();
			ViewBag.Kw3 = Request["level3"].ToString();
			ViewBag.KeyWords = keywords;
			ViewBag.Totalcount = Opr.GetCount(strWhere);
			List<Model.SysAttribute>  list = Opr.GetListByPage(strWhere, "", "AttributeID ", (PageIndex - 1) * PageSize+1, PageIndex * PageSize);
			return View(list);

		}
		public ActionResult New(int id,int ?aid)
		{
			Model.SysAttribute model = new Model.SysAttribute();

			int AttributeID = aid == null ? 0 : (int)aid;
			if(AttributeID>0)
			{
				model = Opr.GetEntity(AttributeID);
			}else{
				model.CategoryID = id;
				model.Type = 0;
			}
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
		/// <summary>
		/// 删除模块
		/// </summary>
		/// <returns>The delete.</returns>
		/// <param name="id">Identifier.</param>
		[HttpPost]
		public ActionResult Delete(int id)
		{
			Helper.ResultInfo<bool> rs = new Helper.ResultInfo<bool>();
			rs.Data = Opr.DeleAttribute(id);
			if (rs.Data)
			{
				rs.Msg = "删除成功.";
				rs.Result = true;
				rs.Url = "";
			}
			else
			{
				rs.Msg = "删除失败.";
				rs.Result = false;
				rs.Url = "";
			}
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
		}
	}
}
