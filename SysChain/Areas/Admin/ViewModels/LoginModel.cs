using System;
using System.ComponentModel.DataAnnotations;

namespace SysChain.Admin
{
	public class LoginModel
	{
		[Required(ErrorMessage ="登录账号不能为空")]
		public string LoginName { set; get; }
		[Required(ErrorMessage = "登录密码不能为空"),MinLength(6,ErrorMessage="密码不能小于6位字符")]
		[DataType(DataType.Password)]
		public string LoginPassword { set; get; }
	}
}
