namespace SysChain.Helper
{
	/// <summary>  
	/// 调用结果信息类  
	/// </summary>  
	/// <typeparam name="T">泛型类型</typeparam>  
	public class ResultInfo<T> where T : new()
	{
		/// <summary>  
		/// 调用结果是否成功  
		/// </summary>  
		public bool Result { set; get; }
		/// <summary>  
		/// 调用结果返回的数据  
		/// </summary>  
		public T Data { set; get; }
		/// <summary>  
		/// 调用结果返回的相关提示信息  
		/// </summary>  
		public string Msg { set; get; }
		/// <summary>
		/// 跳转Url
		/// </summary>
		public string Url { set; get; }
	}
}
