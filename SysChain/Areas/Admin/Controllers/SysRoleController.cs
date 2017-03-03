using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SysChain.Areas.Admin.Controllers
{

	public class SysRoleController : Controller
	{
		private SysChain.BLL.SysRole Opr { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (Opr == null) { Opr = new BLL.SysRole();}
			base.Initialize(requestContext);
		}
		/// <summary>
		/// 角色首页列表
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="index">Index.</param>
		/// <param name="keywords">Keywords.</param>
		public ActionResult  Index(int? index,string keywords)
		{
			ViewBag.Title = "电商后台管理系统";
			int PageIndex = index == null ? 1 : (int)index;
			ViewBag.PageIndex = PageIndex;
			int PageSize = 5;
			string strWhere =string.Empty;
			if (!string.IsNullOrEmpty(keywords))
			{
				strWhere += "  Name like '%" + keywords + "%'";
			}
			ViewBag.KeyWords = keywords;
			ViewBag.Totalcount = Opr.GetCount(strWhere);
			List<Model.SysRole> list = Opr.GetListByPage(strWhere, "", (PageIndex - 1) * PageSize, PageIndex * PageSize);
			return View(list);
		}
		/// <summary>
		/// 角色赋权
		/// </summary>
		/// <returns>The access.</returns>
		/// <param name="id">Identifier.</param>
		[HttpGet]
		public ActionResult RoleAccess(int id)
		{
			ViewBag.Title = "角色权限配置";
			BLL.SysMoudle moudle = new BLL.SysMoudle();
			List<Model.MoudleForTree> list = moudle.GetTree(" State=1","OrderCode");
			ViewBag.RoleID = id;
			ViewBag.existMoudle=Opr.GetMoudleIDByRoleID(id);
			return View(list);
		}
		[HttpPost]
		public ActionResult RoleAccess(int id,List<int>access)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			List<Model.SysRoleAndMoudle> li = new List<Model.SysRoleAndMoudle>();
			if (access != null)
			{
				foreach (int mid in access)
				{
					Model.SysRoleAndMoudle sam = new Model.SysRoleAndMoudle();
					sam.RoleID = id;
					sam.MoudleID = mid;
					li.Add(sam);
				}
			}
			BLL.SysRoleAndMoudle Opration = new BLL.SysRoleAndMoudle();
			rs.Data = Opration.Insert(li, id);
			if (rs.Data > 0)
			{
				rs.Msg = "操作成功";
				rs.Result = true;
				rs.Url = Url.Action("Index", "SysRole", new { area = "Admin", Index = 1, keywords = "" });
			}
			else
			{
				rs.Msg = "操作失败";
				rs.Result = false;
				rs.Url = "";
			}
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
		}
		/// <summary>
		/// 新增角色
		/// </summary>
		/// <returns>The new.</returns>
		/// <param name="id">Identifier.</param>
		[HttpGet]
		public ActionResult New(int? id)
		{
			int RoleID = id == null ? 0 : (int)id;
			Model.SysRole model = new Model.SysRole();
			if (RoleID > 0)
			{
				model = Opr.GetEntity(RoleID);
				ViewBag.Title = "正在编辑角色编号： " + model.RoleID;
			}
			else
			{
				model.RoleID = 0;
				model.State = true;
				ViewBag.Title = "后端管理系统-新增角色";
			}
			return View(model);
		}
		/// <summary>
		/// 角色提交
		/// </summary>
		/// <returns>The new.</returns>
		/// <param name="model">Model.</param>
		[HttpPost]
		public ActionResult New(Model.SysRole model)
		{
			Helper.ResultInfo<int> rs = new Helper.ResultInfo<int>();
			JsonResult jr = new JsonResult();
			if (ModelState.IsValid)
			{
				if (model.RoleID > 0)
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

				jr.Data = rs;
				return jr;
			}
		}
		/// <summary>
		/// 角色状态更改
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
		/// 角色删除
		/// </summary>
		/// <returns>The delete.</returns>
		/// <param name="id">Identifier.</param>
		[HttpPost]
		public ActionResult Delete(int id)
		{
			Helper.ResultInfo<bool> rs = new Helper.ResultInfo<bool>();
			rs.Data = Opr.DeleRole(id);
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
		/// 角色下来列表
		/// </summary>
		/// <returns>The select.</returns>
		public ActionResult Select()
		{
			return	Json(Opr.GetRoleForSelect(" State=1"));
		}
	}
}
