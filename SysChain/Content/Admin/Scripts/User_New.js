$(function(){
	getData();
	var $form = $('#frmNew'),
	validate = f7.validate,
	formError = popup.formError;
	$form.on('focus', 'input:focus', function(evt) {
		formError.init().hide();
	});
	$btn=$("btn-primary");
	$form.on('submit', function(evt){
			evt.preventDefault();
			evt.stopPropagation();
			if ($btn.is('.btn-disabled')) {
				return false;
			} 
			formError.init();
			if (!formError.show($("#LoginName"), validate.textNull($("#LoginName"),'请输入登录账号'))) return false;
			if (!formError.show($("#RoleID_chosen"), validate.selectNull($("#RoleID"),'请选择用户角色'))) return false;
			if (!formError.show($("#Name"), validate.textNull($("#Name"),'请输入真实名称'))) return false;
			if (!formError.show($("#Telephone"), validate.tel($("#Telephone"),'请输入手机号码'))) return false;
			if (!formError.show($("#Department"), validate.textNull($("#Department"),'请输入部门名称'))) return false;

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

getData=function(){
	$.post("/Admin/SysRole/Select",function(data,status){
		if(status=="success")
		{
			var strHtml=[],
				currentid=$("#currentid").val();

			strHtml.push('<option value="0">请选择...</option>');
			if(data!=null)
			{
				$.each(data,function(i){
					if(data[i].RoleID==currentid)
					{	
						strHtml.push('<option value="'+data[i].RoleID+'" selected="selected">'+data[i].RoleName+'</option>');
					}else{
						strHtml.push('<option value="'+data[i].RoleID+'">'+data[i].RoleName+'</option>');
					}
				});
			}
			$("#RoleID").html(strHtml.join(''));
			$("#RoleID").chosen({disable_search_threshold: 7} );
		}
	});
};