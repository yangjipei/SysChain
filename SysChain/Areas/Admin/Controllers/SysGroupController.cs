﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Routing;
using SysChain.Model;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysGroupController : Controller
	{
		private SysChain.BLL.SysGroup Opr { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysGroup(); }
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
			List<Model.SysGroup> list = Opr.GetListByPage(strWhere, "", "OrderCode", (PageIndex - 1) * PageSize+1, PageIndex * PageSize);
			return View(list);
		}
		[HttpGet]
		public ActionResult New(int? id, int pid)
		{
			int GroupID = id == null ? 0 : (int)id;
			Model.SysGroup model = new Model.SysGroup();
			model.ParentID = pid;
			if (GroupID > 0)
			{
				model = Opr.GetEntity(GroupID);
				ViewBag.Title = "分组管理-正在编辑分组名称： " + model.Name;
			}
			else
			{
				if (pid == 0)
				{
					model.Layer = 1;
				}
				else {
					model.Layer = Opr.GetLayer(pid) + 1;
				}
				model.GroupID = 0;
				model.State = true;
				ViewBag.Title = "新增分组";
			}
			return View(model);
		}
		/// <summary>
		/// 接收新增、编辑品类数据处理
		/// </summary>
		/// <returns>The new.</returns>
		/// <param name="model">Model.</param>
		[HttpPost]
		public ActionResult New(Model.SysGroup model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			if (ModelState.IsValid)
			{
				if (model.GroupID > 0)
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

					model.OrderCode = Opr.GetNewOrderCode(model.ParentID, model.Layer);
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
		public ActionResult List()
		{
			return Json(Opr.GetList("Layer=1" , "OrderCode"), JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// 更新模块状态
		/// </summary>
		/// <returns>The status.</returns>
		/// <param name="id">Identifier.</param>
		[HttpPost]
		public ActionResult UpdateStatus(int id)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			rs.Data = Opr.UpdateState(id);
			if (rs.Data > 0)
			{
				rs.Msg = "操作成功.";
				rs.Result = true;
				rs.Url = "";
			}
			else
			{
				rs.Msg = "操作失败.";
				rs.Result = false;
				rs.Url = "";
			}
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
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
			rs.Data = Opr.DeleCategory(id);
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
		/// <summary>
		/// 模块排序操作
		/// </summary>
		/// <returns>The rank.</returns>
		/// <param name="id">源模块编号</param>
		/// <param name="targetid">目标模块编号</param>
		[HttpPost]
		public ActionResult Rank(int id, int targetid)
		{
			Helper.ResultInfo<bool> rs = new Helper.ResultInfo<bool>();
			if (id > 0 && targetid > 0)
			{
				if (Opr.RankCategory(id, targetid))
				{
					rs.Data = true;
					rs.Msg = "操作已成功.";
					rs.Result = true;
					rs.Url = "";
				}
				else
				{
					rs.Data = false;
					rs.Msg = "数据库操作失败.";
					rs.Result = true;
					rs.Url = "";
				}
			}
			else
			{
				rs.Data = false;
				rs.Msg = "参数错误.";
				rs.Result = true;
				rs.Url = "";
			}
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
		}
		public JsonResult SelectCategory()
		{
			return Json(Opr.GetModelList("State>0"), JsonRequestBehavior.AllowGet);
		}

	}
}
