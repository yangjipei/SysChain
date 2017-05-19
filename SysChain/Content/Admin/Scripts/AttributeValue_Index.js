$(function(){
	var $html = $('html');
	moudle_common_init($html,false,'该属性值会被删除且无法恢复，<br />是否继续操作？');
	pagePlugin($html);
	var $form = $('#frmNew'),
	validate = f7.validate,
	formError = popup.formError;
	$form.on('focus', 'input:focus', function(evt) {
		formError.init().hide();
	});
	$form.on('submit', function(evt){
			evt.preventDefault();
			evt.stopPropagation();
			formError.init();
			if (!formError.show($("#Name"), validate.textNull($("#Name"),'请输入属性名称'))) return false;
			popup.loading.init();
			$.ajax({  
			    type: "Post",  
			    url: $form.attr("action"),
			    data: $(this).serialize(),  
			    dataType: "json",   
			    success: function (data) {  
			      if (data.Result) {  
						$("#frmSearch").submit();
			      }  
			      else {  
			       popup.info.init().show(data.Msg, false); 
			      }  
			    },   
			    error: function (XMLHttpRequest, textStatus, errorThrown) {  
					  popup.info.init().show("系统错误.", false); 
			    }  
			}); 
	});
	$("#btnEdit").on('click',function(evt){
			evt.preventDefault();
			evt.stopPropagation();
			$("#Name").val($(this).data("name"));
			$("#ValueID").val($(this).data("id"));
			$("#btnsubmit").text("修改属性值");
	});
});
