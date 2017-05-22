$(function () {
	var $form = $('#frmNew'),
	validate = f7.validate,
	formError = popup.formError;
	$form.on('focus', 'input:focus', function(evt) {
		formError.init().hide();
	});
	$btn=$(".btn-primary");
	$form.on('submit', function(evt){
			evt.preventDefault();
			evt.stopPropagation();
			if ($btn.is('.btn-disabled')) {
				return false;
			} 
			formError.init();
			if (!formError.show($("#LoginName"), validate.textNull($("#LoginName"),'登陆账户不能为空.'))) return false;
			if (!formError.show($("#LoginPassword"), validate.textNull($("#LoginPassword"),'登陆密码不能为空'))) return false;
			if (!formError.show($("#NewPassword"), validate.textNull($("#NewPassword"),'新密码不能为空'))) return false;
			if (!formError.show($("#NewPassword"), validate.newOldPassword($("#NewPassword"),$("#LoginPassword")))) return false;
			if (!formError.show($("#ConfirmPassword"), validate.confirmPassword($("#ConfirmPassword"),$("#NewPassword")))) return false;
			$btn.addClass('btn-disabled');
			popup.loading.init();
			$.ajax({  
			    type: "Post",  
			    url: $form.attr("action"),
			    data: $(this).serialize(),  
			    dataType: "json",   
			    success: function (data) {  
			      if (data.Result) {  
					popup.info.init().show(data.Msg, true,data.Url); 
			      }  
			      else {  
			       popup.info.init().show(data.Msg, false); 
				   $btn.removeClass('btn-disabled');
			      }  
			    },   
			    error: function (XMLHttpRequest, textStatus, errorThrown) {  
					$btn.removeClass('btn-disabled');
			    }  
			}); 
	});

});


