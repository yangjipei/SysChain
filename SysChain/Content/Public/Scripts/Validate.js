f7.validate = {}; //验证
// 标准输出
f7.validate.output = (function(info, status) {
	return {
		status: status ? status : false,
		info: info
	};
});//f7.validate.output()


// 用户名
f7.validate.username = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('用户名 不能为空！');
	} else if (len < 5) {
	 	return this.output('用户名 不能小于5个字符！');
	} else if (len > 16) {
		return this.output('用户名 不能大于16个字符！');
	}//if

	return this.output('', true);
});//f7.validate.username()

// 密码
f7.validate.password = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (len === 0) {
		return this.output('密码 不能为空！');
	} else if (len < 6) {
		return this.output('密码 不能小于6个字符！');
	} else if (!(/\d+/).test(value) || !(/[a-z]+/i).test(value)) {
		return this.output('密码 必须含有字母与数字字符！');
	}//if

	return this.output('', true);
});//f7.validate.password()

//必须选择
f7.validate.selectNull=(function($input,$errmsg){
	var value = $.trim($input.val()),
		len = value.length;

	if (len === 0) {
		return this.output($errmsg);
	} 
	if(value<=0)
	{
		return this.output($errmsg);
	}

	return this.output('', true);

});//f7.validate.selectNull()

//必须天蝎
f7.validate.textNull=(function($input,$errmsg){
	var value = $.trim($input.val()),
		len = value.length;

	if (len === 0) {
		return this.output($errmsg);
	} 
	if(value<=0)
	{
		return this.output($errmsg);
	}

	return this.output('', true);

});//f7.validate.textNull()

// 验证 手机号 与 邮箱地址
f7.validate.telAndEmail = (function($input) {
	var value = $.trim($input.val());

	if (value.indexOf('@') == -1) {
		return this.tel($input);
	} else {
		return this.email($input);
	}//if
});

// 判断 并返回 username 填入内容类型 手机号 或 邮箱
f7.validate.isUsernameType = (function($input) {
	var value = $.trim($input.val());

	if (value.indexOf('@') == -1) {
		return 'tel';
	} else {
		return 'email';
	}//if
});

/////////////////////////////////////////////////////////////////////////////////////////////

// 充值金额 验证
f7.validate.rechargeMoney = (function($input, limit) {
	var value = parseInt($.trim($input.val()), 10);

	limit = parseInt(limit);

	if (!parseInt(value)) {
		return this.output('请输入 充值金额！');
	} else if (!/^[0-9]+$/.test(value)) {
		return this.output('充值金额 输入错误！');
	} else if (limit > 0 && value > limit) {
		return output('本次最多可充值：' + limit + ' 元！');
	}//if

	return this.output('', true);
});//f7.validate.rechargeMoney()


// 提现金额 验证
f7.validate.withdrawMoney = (function($input, limit) {
	var value = parseInt($.trim($input.val()), 10);

	limit = parseInt(limit);

	if (!value) {
		return this.output('请输入 提现金额！');
	} else if (!/^[0-9]+$/.test(value)) {
		return this.output('提现金额 输入错误！');
	} else if (limit > 0 && value > limit) {
		return this.output('本次最多可提取：' + limit + ' 元！');
	}//if

	return this.output('', true);
});//f7.validate.withdrawMoney()

/////////////////////////////////////////////////////////////////////////////////////////////

// 绑定银行卡 开户行
f7.validate.bankId = (function($input) {
	var value = $.trim($input.val());

	if (!value) {
		return this.output('请选择 开户银行！');
	}//if

	return this.output('', true);
});//f7.validate.bankId()

// 银行卡号
f7.validate.bankCardNo = (function($input) {
	var value = $.trim($input.val()),
		strBin = '10,18,30,35,37,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,58,60,62,65,68,69,84,87,88,94,95,98,99',
		first15Num = value.substr(0, value.length - 1),
		lastNum = value.substr(value.length - 1, 1),
		newArr = [],
		arrJiShu = [],
   		arrJiShu2 = [],
    	arrOuShu = [];  //偶数位数组

	if (value.length < 16 || value.length > 19) {
		return this.output('银行卡号 长度必须在16或19位！');
	}//if

	if (!(/^\d{16}|\d{19}$/).exec(value)) {
		return this.output('银行卡号 必须为数字!');
	}//if

	//开头6位
	if (strBin.indexOf(value.substring(0, 2)) === -1) {
		return this.output('银行卡号 格式错误！');
	}//if

    for (var i = first15Num.length - 1; i > -1; i--) {
        newArr.push(first15Num.substr(i, 1));
    }//for

    for (var j = 0; j < newArr.length; j++) {
        if ((j + 1) % 2 === 1) {
        	//奇数位
            if (parseInt(newArr[j]) * 2 < 9) {
            	arrJiShu.push(parseInt(newArr[j]) * 2);
            } else {
            	arrJiShu2.push(parseInt(newArr[j]) * 2);
        	}//if
        } else {
        	//偶数位
        	arrOuShu.push(newArr[j]);
        }//if
    }//for
    
    var jishu_child1 = [],//奇数位*2 >9 的分割之后的数组个位数
    	jishu_child2 = [];//奇数位*2 >9 的分割之后的数组十位数
    for (var h = 0; h < arrJiShu2.length; h++) {
        jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
        jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
    }//for     
    
    var sumJiShu = 0, //奇数位*2 < 9 的数组之和
    	sumOuShu = 0, //偶数位数组之和
    	sumJiShuChild1 = 0, //奇数位*2 >9 的分割之后的数组个位数之和
    	sumJiShuChild2 = 0, //奇数位*2 >9 的分割之后的数组十位数之和
    	sumTotal = 0;
    for (var m = 0; m < arrJiShu.length; m++) {
        sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
    }//for
    
    for (var n = 0; n < arrOuShu.length; n++) {
        sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
    }//for
    
    for (var p = 0; p < jishu_child1.length; p++) {
        sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
        sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
    }//for

    //计算总和
    sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);
    
    //计算Luhm值
    var k = parseInt(sumTotal) % 10 === 0 ? 10 : parseInt(sumTotal) % 10,   //if     
    	luhm = 10 - k;
    
    if (lastNum != luhm) {
    	return this.output('银行卡号 错误！');
    }//if

    return this.output('', true);
});//f7.validate.bankCardNo()


/////////////////////////////////////////////////////////////////////////////////////////////

f7.validate.questionTypeId = (function($input) {
	var value = $.trim($input.val());

	if (!value) {
		return this.output('请选择 类型！');
	}//if

	return this.output('', true);
});//f7.validate.questionTypeId()

f7.validate.questionTitle = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('标题 不能为空！');
	} else if (len < 6) {
		return this.output('标题 不能小于6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.questionTitle()

f7.validate.questionContent = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('内容 不能为空！');
	} else if (len < 16) {
		return this.output('内容 不能小于16个字符！');
	}//if

	return this.output('', true);
});//f7.validate.questionContent()

/////////////////////////////////////////////////////////////////////////////////////////////

f7.validate.futureCustomQuantity = (function($input, maxQuantity) {
	var value = $.trim($input.val()),
		regexp = /^[0-9]+$/;

	if (!regexp.test(value)) {
		return this.output('交易数量 必须为数字！');
	} else if (value > maxQuantity) {
	 	return this.output('交易数量 不能大于' + maxQuantity + '手！');
	} else if (value <= 0) {
		return this.output('交易数量 不能小于1手！');
	}//if

	return this.output('', true);
});//f7.validate.futureCustomQuantity()

f7.validate.futureDeposit = (function($input, perMoney, perRatio, perDeposit, currentQuantity) {
	var value = $.trim($input.val()).replace(/,/g, ''),
		regexp = /^[0-9]+$/,
		formatMoney = f7.tool.formatMoney;

	if (value === '') {
		// return this.output('保证金 不能为空！');
		return this.output('请输入 保证金！');
	} else if (!regexp.test(value)) {
		return this.output('保证金 输入错误！');
	} else if (value > perMoney * currentQuantity) {
		return this.output('保证金 不能大于 总投资金额！');
	} else if (value < perDeposit * currentQuantity) {
		return this.output('保证金 不能低于 '+ formatMoney(perDeposit * currentQuantity) +'元');
	} else if (value % 100 !== 0) {
		return this.output('保证金 必须为100元的整数倍！');
	}//if

	return this.output('', true);
});//f7.validate.futureDeposit()

f7.validate.futureAccountType = (function($input) {
	$input = $input.filter(':checked:not(:disabled)');
	
	var value = $input.val();

	// console.log('value = '+value);
	if (!$input.length) {
		return this.output('抱歉 申请用户太多，请稍候再来！');
	} else if (!value) {
		return this.output('请选择 操作账户！');
	}//if

	return this.output('', true);
});//f7.validate.futureAccountType()

/////////////////////////////////////////////////////////////////////////////////////////////

//  评论
f7.validate.commentContent = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('评论内容 不能为空！');
	} else if (len > 256) {
	 	return this.output('评论内容 不能小于256个字符！');
	}//if

	return this.output('', true);
});//f7.validate.commentContent()



// 真实姓名
f7.validate.realUsername = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('姓名 不能为空！');
	} else if (len < 2) {
	 	return this.output('姓名 不能小于2个字符！');
	} else if (len > 6) {
		return this.output('姓名 不能大于6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.realUsername()

// 验证 生日
f7.validate.birthday = (function($input) {
	var value = $.trim($input.val()),
		regexp = /^\d{4}(\/|\-)\d{1,2}(\/|\-)\d{1,2}$/i;

	// console.log('birthday = ' + value);
	if (value === '') {
		return this.output('生日 不能为空！');
	} else if (!regexp.test(value)) {
		return this.output('生日 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.birthday();

// 验证 性别
f7.validate.gender = (function($input) {
	if (!$input.filter(':checked').length) {
		return this.output('请选择 性别！');
	}//if

	return this.output('', true);
});//f7.validate.gender();

// 验证 外观 内饰
f7.validate.appearance = (function($input, name) {
	if (!$input.filter(':checked').length) {
		return this.output('请选择 '+ name +'！');
	}//if

	return this.output('', true);
});//f7.validate.appearance();

// 验证 省市区
f7.validate.addressId = (function($input, name) {
	var value = $.trim($input.val());

	name = name ? name : '省';//if

	if (value === '') {
		return this.output('请选择 ' + name);
	}//if

	return this.output('', true);
});//f7.validate.addressId();

// 验证 个性签名
f7.validate.intro = (function($input) {
	var value = $.trim($input.val());

	if (value === '') {
		return this.output('个性签名 不能为空！');
	} else if (len < 6) {
	 	return this.output('个性签名 不能小于6个字符！');
	} else if (len > 120) {
		return this.output('个性签名 不能大于120个字符！');
	}//if

	return this.output('', true);
});//f7.validate.intro();





// 邮箱验证
f7.validate.email = (function($input) {
	var regexp = /^([\.a-z0-9_-])+@([a-z0-9_-])+(\.[a-z0-9_-])+/i,
		value = $.trim($input.val());

	if (value === '') {
		return this.output('邮箱地址 不能为空！');
	} else if (!regexp.test(value)) {
		return this.output('邮箱地址 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.email()

// 真实姓名
f7.validate.realName = (function($input) {
	var value = $.trim($input.val());

	if (value === '') {
		return this.output('姓名 不能为空！');
	} else if (/\w+/.test(value)) {
		return this.output('姓名 不能含有字母数字字符！');
	}//if

	return this.output('', true);
});//f7.validate.realName()

// 身份证号
f7.validate.certificateCardNo = (function($input) {
	var value = $.trim($input.val()),
		identityCodeValid = (function(code) { 
			var city = {11:'北京', 12:'天津', 13:'河北', 14:'山西', 15:'内蒙古', 
						21:'辽宁', 22:'吉林', 23:'黑龙江', 31:'上海', 32:'江苏',
						33:'浙江', 34:'安徽', 35:'福建', 36:'江西', 37:'山东',
						41:'河南', 42:'湖北', 43:'湖南', 44:'广东', 45:'广西',
						46:'海南', 50:'重庆', 51:'四川', 52:'贵州', 53:'云南',
						54:'西藏', 61:'陕西', 62:'甘肃', 63:'青海', 64:'宁夏',
						65:'新疆', 71:'台湾', 81:'香港', 82:'澳门', 91:'国外'};

			if (!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i.test(code)) {
				return false;
				// return this.output('身份证号 格式错误！');
			} else if (!city[code.substr(0,2)]) {
				return false;
				// return this.output('身份证号 地址编码错误！');
			} else {
				//18位身份证需要验证最后一位校验位
				if (code.length == 18) {
				    code = code.split('');
				    //∑(ai×Wi)(mod 11)
				    var factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2], //加权因子
				    	parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2], //校验位
				    	sum = 0,
				    	ai = 0,
				    	wi = 0;
				    for (var i = 0; i < 17; i++) {
				        ai = code[i];
				        wi = factor[i];
				        sum += ai * wi;
				    }//for 
				    var last = parity[sum % 11];
				    if (parity[sum % 11] != code[17]) {
				    	return false;
				    	// return this.output('身份证号 校验位错误！');
				    }//if
				}//if
			}//if

			return true;
        });//identityCodeValid()

	if (value === '') {
		return this.output('身份证号 不能为空！');
	} else if (!identityCodeValid(value)) {
		return this.output('身份证号 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.certificateCardNo()

// 手机号
f7.validate.tel = (function($input) {
	var value = $.trim($input.val()),
		regexp = /^1[3|4|5|8][0-9]\d{8}$/;

	if (value === '') {
		return this.output('手机号 不能为空！');
	} else if (!regexp.test(value)) {
	 	return this.output('手机号 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.tel()

// 短信验证码
f7.validate.verify = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('验证码 不能为空！');
	} else if (len < 6) {
		return this.output('验证码 必需为6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.verify()

// 图片验证码
f7.validate.checkcode = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('验证码 不能为空！');
	} else if (len < 5) {
		return this.output('验证码 必需为5个字符！');
	}//if

	return this.output('', true);
});

// 原始密码
f7.validate.oldPassword = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (len === 0) {
		return this.output('原始密码 不能为空！');
	} else if (len < 6) {
		return this.output('原始密码 不能小于6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.oldPassword()



// 确认密码
f7.validate.confirmPassword = (function($input1, $input2) {
	var value1 = $.trim($input1.val()),
		value2 = $.trim($input2.val());

	if (value2 === '') {
		return this.output('确认密码 不能为空！');
	} else if (value1 != value2) {
		return this.output('确认密码 错误！');
	}//if

	return this.output('', true);
});//f7.validate.confirmPassword()

// 验证 新老密码 是否一至
f7.validate.newOldPassword = (function($input1, $input2) {
	var value1 = $.trim($input1.val()),
		value2 = $.trim($input2.val());

	if (value1 === '') {
		return this.output('新密码 不能为空！');
	} else if (value1 == value2) {
		return this.output('新密码 未变更！');
	}//if

	return this.output('', true);
});//f7.validate.newOldPassword();

// 密码强度
f7.validate.passwordStrength = (function($input, $strength) {
	var value = $.trim($input.val()),
		len = value.length,
		$li = $strength.find('li'),
		index = -1;

	$li.removeClass('current');

	if (len < 6) {
		index = -1;
	} else if (len <= 9) {
		index = 0;
	} else if (len <= 12) {
		index = 1;
	} else if (len >= 12) {
		index = 2;
	}//if

	if (index != -1) {
		for (var i = 0; i <= index; i++) {
			$li.filter(':eq(' + i + ')').addClass('current');
		}//if
	}//if

	this.output(index);
});//f7.validate.passwordStrength()


