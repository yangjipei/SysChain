using System;
using System.Collections.Generic;

namespace SysChain.Model
{
	public class VM_SysCategory
	{
		/// <summary>
		/// 品类编号
		/// </summary>
		public int CategoryID { set; get; }
		/// <summary>
		/// 品类父级编号
		/// </summary>
		public int ParentID { set; get; }
		/// <summary>
		/// 品类名称
		/// </summary>
		public string CategoryName { get; set; }
		/// <summary>
		/// 所在层级
		/// </summary>
		public int Layer { set; get; }
		/// <summary>
		/// 子集
		/// </summary>
		private List<VM_SysCategory> _children = new List<VM_SysCategory>();
		public List<VM_SysCategory> Children { 
			set { _children = value; }
			get { return _children;}
		}
	}
}
