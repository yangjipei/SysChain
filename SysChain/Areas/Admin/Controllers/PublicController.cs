using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Text;

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
	}
}
