using System;
using System.Collections.Generic;

namespace SysChain.Model
{
	public class VM_SysGroup
	{
		/// <summary>
		/// 品类编号
		/// </summary>
		public int GroupID { set; get; }
		/// <summary>
		/// 品类父级编号
		/// </summary>
		public int ParentID { set; get; }
		/// <summary>
		/// 品类名称
		/// </summary>
		public string GroupName { get; set; }
		/// <summary>
		/// 所在层级
		/// </summary>
		public int Layer { set; get; }
		/// <summary>
		/// 子集
		/// </summary>
		private List<VM_SysGroup> _children = new List<VM_SysGroup>();
		public List<VM_SysGroup> Children { 
			set { _children = value; }
			get { return _children;}
		}
	}
}
