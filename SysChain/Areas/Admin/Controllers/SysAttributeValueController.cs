using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysAttributeValueController : Controller
	{
		private SysChain.BLL.SysAttributeValue Opr { get; set; }
		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysAttributeValue(); }
			base.Initialize(requestContext);
		}
		/// <summary>
		/// Index the specified id and index.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="id">属性ID</param>
		/// <param name="index">分页数.</param>
		public ActionResult Index(int id,int? index)
		{
			ViewBag.ID = id;
			int PageIndex = index == null ? 1 : (int)index;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			ViewBag.Totalcount = Opr.GetCount(" AttributeID= "+id);
			List<Model.SysAttributeValue> list = Opr.GetListByPage(" AttributeID= " + id, "", "ValueID ", (PageIndex - 1) * PageSize + 1, PageIndex * PageSize);
			return View(list);
		}
		[HttpPost]
		public ActionResult New(Model.SysAttributeValue model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			if (ModelState.IsValid)
			{
				if (model.ValueID > 0)
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
			rs.Data = Opr.DeleAttributeValue(id);
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
