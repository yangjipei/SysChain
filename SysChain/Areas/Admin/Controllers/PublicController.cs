using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Text;
using System.Collections.Specialized;
using System.IO;

namespace SysChain.Areas.Admin.Controllers
{
	public class PublicController : Controller
	{
		public ActionResult PageInfo(int Totalcount,int? pagesize,int? pageindex,string Url)
		{
			StringBuilder strHtml = new StringBuilder();
			strHtml.Append("<footer class='page'>");
			if (Totalcount > 0)
			{
				int PageIndex = pageindex == null ? 1 : (int)pageindex;
				int PageSize = pagesize == null ? 5 : (int)pagesize;
				int PageNum = 0;
				if (Totalcount % PageSize == 0)
				{
					PageNum = Totalcount / PageSize;
				}
				else
				{
					PageNum = (Totalcount / PageSize) + 1;
				}
				strHtml.Append(String.Format(" <div class='page-info'>每页 {0} 条/共 {1} 条记录 {2}/{3} 页</div>", PageSize, Totalcount, PageIndex, PageNum));
				strHtml.Append("<div class='page-code'>");
				if (pageindex > 1)
				{
					strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='"+(pageindex-1).ToString()+"'>上一页</a>");
				}
				else { 
					strHtml.Append("<span >上一页</span>");
				}
				int LeftPage = 0;
				if (PageNum % 2 == 0)
				{
					LeftPage = PageNum / 2;
				}
				else
				{
					LeftPage = (PageNum / 2) + 1;
				}
				if (PageNum <= 5)
				{
					for (int i = 1; i <= PageNum; i++)
					{
						if (i == PageIndex)
						{
							strHtml.Append("<span class='current' data-size='" + i.ToString() + "'>" + i.ToString() + "</span>");
						}
						else
						{
							strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + i.ToString() + "'>" + i.ToString() + "</a>");
						}
					}
				}
				else { 
				if (PageIndex < LeftPage)
					{
						if (PageIndex < 5)
						{
							for (int i = 1; i <= 5; i++)
							{
								if (i == PageIndex)
								{
									strHtml.Append("<span class='current' data-size='" + i.ToString() + "'>" + i.ToString() + "</span>");
								}
								else
								{
									strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + i.ToString() + "'>" + i.ToString() + "</a>");
								}
							}
							strHtml.Append("<span >...</span>");
							strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + PageNum.ToString() + "'>" + PageNum.ToString() + "</a>");
						}
						else
						{
							if (PageIndex + 5 >= PageNum)
							{
								for (int i = PageIndex; i <= PageNum; i++)
								{
									if (i == PageIndex)
									{
										strHtml.Append("<span class='current' data-size='" + i.ToString() + "'>" + i.ToString() + "</span>");
									}
									else
									{
										strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + i.ToString() + "'>" + i.ToString() + "</a>");
									}
								}
							}
							else
							{
								for (int i = PageIndex; i <= PageIndex + 5; i++)
								{
									if (i == PageIndex)
									{
										strHtml.Append("<span class='current' data-size='" + i.ToString() + "'>" + i.ToString() + "</span>");
									}
									else
									{
										strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + i.ToString() + "'>" + i.ToString() + "</a>");
									}
								}
								strHtml.Append("<span >...</span>");
								strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + PageNum.ToString() + "'>" + PageNum.ToString() + "</a>");
							}
						}
					}
					else
					{
						strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='1'>1</a>");
						strHtml.Append("<span >...</span>");
						if (PageIndex + 5 > PageNum)
						{
							for (int i = PageNum - 4; i <= PageNum; i++)
							{
								if (i == PageIndex)
								{
									strHtml.Append("<span class='current' data-size='" + i.ToString() + "'>" + i.ToString() + "</span>");
								}
								else
								{
									strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + i.ToString() + "'>" + i.ToString() + "</a>");
								}
							}
						}
						else
						{
							for (int i = PageIndex; i <= PageIndex + 5; i++)
							{
								if (i == PageIndex)
								{
									strHtml.Append("<span class='current' data-size='" + i.ToString() + "'>" + i.ToString() + "</span>");
								}
								else
								{
									strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + i.ToString() + "'>" + i.ToString() + "</a>");
								}
							}
							strHtml.Append("<span >...</span>");
							strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + PageNum.ToString() + "'>" + PageNum.ToString() + "</a>");
						}
					}
				}

				if (pageindex < PageNum)
				{
					strHtml.Append("<a href='" + Url + "' data-page-init=\"on\" data-size='" + (pageindex + 1).ToString() + "'>下一页</a>");
				}
				else
				{
					strHtml.Append("<span >下一页</span>");
				}

			}
			else { 
				strHtml.Append(" <div class='page-info'>共 0 条记录</div>");
			}
			strHtml.Append("</footer>");
			return Content(strHtml.ToString());
		}
		public ActionResult Err404()
		{
			return View();
		}
		public ActionResult Err500()
		{
			return View();
		}
		[HttpPost]
		public JsonResult UploadImg(string type, HttpPostedFileBase[] uploadfile)
		{
			Helper.ResultInfo<bool> rs = new Helper.ResultInfo<bool>();
			if(Session["UserInfo"]!=null)
			{
				SysChain.Model.SysUser user = (SysChain.Model.SysUser)Session["UserInfo"];
				const string fileTypes = "gif,jpg,jpeg,png,bmp";
				const int maxSize = 205000;
				string imgPath = "/Content/Public/Images/store/";
				for (int fileId = 0; fileId < uploadfile.Count(); fileId++)
				{
					HttpPostedFileBase curFile = uploadfile[fileId];
					if (curFile.ContentLength < 1) { continue; }
					else if (curFile.ContentLength > maxSize)
					{
						rs.Msg = "上传文件中有图片大小超出200KB!";
						rs.Result = false;
						break;
					}
					string fileExt = Path.GetExtension(curFile.FileName);
					if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
					{
						rs.Msg = "上传文件中包含不支持文件格式!!";
						rs.Result = false;
						break;
					}
					string fullFileName = type + "_" + user.UserID.ToString() + fileExt;//CreateRandomCode(8)
					try
					{
						string filePhysicalPath = Server.MapPath(imgPath + fullFileName);
						if (!System.IO.Directory.Exists(Server.MapPath(imgPath)))
						{
							System.IO.Directory.CreateDirectory(Server.MapPath(imgPath));
						}
						if(System.IO.File.Exists(filePhysicalPath))
						{
							System.IO.File.Delete(filePhysicalPath);
						}
						curFile.SaveAs(filePhysicalPath);
						rs.Data = true;
						rs.Result = true;
						rs.Url = imgPath + fullFileName;
					}
					catch (Exception exception)
					{
						rs.Msg = exception.Message;
						rs.Result = false;
						break;
					}
				}
			}
			else{
				rs.Msg = "上传文件中包含不支持文件格式!!";
				rs.Result = false;
			}
			JsonResult jr = new JsonResult();
			jr.Data = rs;
			return jr;
		}
		private string CreateRandomCode(int length)
		{
			 string[] codes = new string[36] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
             StringBuilder randomCode = new StringBuilder();
             Random rand = new Random();
             for (int i = 0; i<length; i++)
             {
                 randomCode.Append(codes[rand.Next(codes.Length)]);
             }
             return randomCode.ToString();
         }
	}
}
