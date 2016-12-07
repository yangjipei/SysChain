$(function () {
	var $form = $('#common'),
	validate = f7.validate,
	formError = popup.formError;
	
	$form.on('focus', 'input:focus', function(evt) {
		formError.init().hide();
	});
	
	var $username = $('input[name="LoginName"]', $form);
	var $password = $('input[name="LoginPassword"]', $form);
	var data={
		LoginName:$username.val(),
		LoginPassword:$password.val(),
	};
	$btn=$("#btnSubmit");
	$form.on('submit', function(evt){
			evt.preventDefault();
			evt.stopPropagation();
			if ($btn.is('.btn-disabled')) {
				return false;
			} 
			formError.init();
			if (!formError.show($username, validate.username($username))) return false;//if
			if (!formError.show($password, validate.password($password))) return false;//if
			if ($('.handler').data('done') != 1) { popup.info.init().show('请滑动验证', false);return false; }
			$btn.addClass('btn-disabled');
			popup.loading.init();
			$.ajax({  
			    type: "Post",  
			    url: $form.attr("action"),
			    data: data,  
			    dataType: "json",   
			    success: function (data) {  
			      if (data.Result) {  
			       popup.info.init().show(data.Msg, true); 
			       setTimeout(function() {top.location.replace(data.Url)}, 1500);
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