﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysCategoryController : Controller
	{
		private SysChain.BLL.SysCategory Opr { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysCategory(); }
			base.Initialize(requestContext);
		}
		public ActionResult Index(int? pid, int? index)
		{
			int ParentID = pid == null ? 0 : (int)pid;
			int PageIndex = index == null ? 1 : (int)index;
			ViewBag.ParentID = ParentID;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			string strWhere = "ParentID=" + ParentID;
			ViewBag.Totalcount = Opr.GetCount(strWhere);
			List<Model.SysCategory> list = Opr.GetListByPage(strWhere, "", "OrderCode", (PageIndex - 1) * PageSize, PageIndex * PageSize);
			return View(list);
		}
		[HttpGet]
		public ActionResult New(int? id, int pid)
		{
			int MoudleID = id == null ? 0 : (int)id;
			Model.SysCategory model = new Model.SysCategory();
			model.ParentID = pid;
			if (MoudleID > 0)
			{
				model = Opr.GetEntity(MoudleID);
				ViewBag.Title = "品类管理-正在编辑模块编号： " + model.CategoryID;
			}
			else
			{
				model.CategoryID = 0;
				model.State = true;
				ViewBag.Title = "新增品类";
			}
			return View(model);
		}
		/// <summary>
		/// 接收新增、编辑品类数据处理
		/// </summary>
		/// <returns>The new.</returns>
		/// <param name="model">Model.</param>
		[HttpPost]
		public ActionResult New(Model.SysCategory model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			if (ModelState.IsValid)
			{
				if (model.CategoryID > 0)
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

					model.OrderCode = Opr.GetNewOrderCode(model.ParentID);
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
		/// 获得下拉接口
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="id">父级ID，默认为0.</param>
		public ActionResult List(int? id)
		{
			int ParentID = id == null ? 0 : (int)id;
			return Json(Opr.GetList("ParentID=" + ParentID, "OrderCode"), JsonRequestBehavior.AllowGet);
		}

	}
}
