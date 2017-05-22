using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{
	public class SysStoreController : Controller
	{
		private SysChain.BLL.SysStroe Opr { get; set; }
		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysStroe(); }
			base.Initialize(requestContext);
		}
		public ActionResult Index()
		{
			SysChain.Model.SysUser user = (SysChain.Model.SysUser)Session["UserInfo"];
			if(!Opr.ExistsByUserID(user.UserID))
			{
				return RedirectToAction("Setting");
			}else
			{
				return View();
			}
		}
		[HttpGet]
		public ActionResult Setting(int ? id)
		{
			int StoreID = id == null ? 0 : (int)id;
			Model.SysStore model = new Model.SysStore();
			if (StoreID > 0)
			{
				model = Opr.GetEntity(StoreID);
			}
			else
			{
				SysChain.Model.SysUser user = (SysChain.Model.SysUser)Session["UserInfo"];
				model.UserID = user.UserID;
				model.StoreID = 0;
			}
			return View(model);
		}
		[HttpPost]
		public ActionResult Setting(Model.SysStore model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			if (ModelState.IsValid)
			{
				if (model.StoreID > 0)
				{
					rs.Data = Opr.ModifyModel(model);
					if (rs.Data > 0)
					{
						rs.Msg = "修改成功.";
						rs.Result = true;
						rs.Url =Url.Action("Index", "SysStore", new { area = "Admin" });
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
					model.CreateTime = DateTime.Now;

					rs.Data = Opr.Insert(model);
					if (rs.Data > 0)
					{
						rs.Msg = "店铺建立成功.";
						rs.Result = true;
						rs.Url = Url.Action("Index", "SysStore", new { area = "Admin" });
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
