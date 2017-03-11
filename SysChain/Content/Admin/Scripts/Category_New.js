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
			if (!formError.show($("#Name"), validate.textNull($("#Name"),'请输入品类名称'))) return false;
			$btn.addClass('btn-disabled');
			popup.loading.init();
			$.ajax({  
			    type: "Post",  
			    url: $form.attr("action"),
			    data: $(this).serialize(),  
			    dataType: "json",   
			    success: function (data) {  
			      if (data.Result) {  
			      	 
			      	 if(window.parent.frames.length>0)
			      	 {
			      	 	window.parent.showMessage(data.Msg);
			      	 }else{
			      	 	popup.info.init().show(data.Msg, true); 
			      	 	$form[0].reset();
			      	 }
				    $('#popWinClose',parent.document).click();	
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