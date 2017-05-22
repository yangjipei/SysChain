using System;
using System.ComponentModel.DataAnnotations;
namespace SysChain.Model
{
	public class VM_SysModifyPassword
	{
		[Required(ErrorMessage = "登录账号不能为空")]
		public string LoginName { set; get; }
		[Required(ErrorMessage = "登录密码不能为空"), MinLength(6, ErrorMessage = "密码不能小于6位字符")]//,RegularExpression(@"(\d+[a-z]+)|([a-z]+\d+)|(\d+[a-z\d]+\d+)",ErrorMessage = "密码必须含有字母与数字字符")
		[DataType(DataType.Password)]
		public string LoginPassword { set; get; }
		[Required(ErrorMessage = "新密码不能为空"), MinLength(6, ErrorMessage = "密码不能小于6位字符")]//,RegularExpression(@"(\d+[a-z]+)|([a-z]+\d+)|(\d+[a-z\d]+\d+)",ErrorMessage = "密码必须含有字母与数字字符")
		[DataType(DataType.Password)]
		public string NewPassword { set; get; }
		[Required(ErrorMessage = "确认密码不能为空"), MinLength(6, ErrorMessage = "密码不能小于6位字符")]//,RegularExpression(@"(\d+[a-z]+)|([a-z]+\d+)|(\d+[a-z\d]+\d+)",ErrorMessage = "密码必须含有字母与数字字符")
		[DataType(DataType.Password)]
		public string ConfirmPassword { set; get; }
	}
}
