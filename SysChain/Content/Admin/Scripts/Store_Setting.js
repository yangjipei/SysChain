
$(function(){
	$("#img1").hide();
	$(".upload-file").click(function(){
		$("#uploadfile").click();
	});
    $("#uploadfile").live("change",function(){
        if ($("#uploadfile").val().length > 0) {
		    ajaxFileUpload();
        }
	});
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
			if (!formError.show($("#Name"), validate.textNull($("#Name"),'请输入店铺名称'))) return false;
			if (!formError.show($("#Description"), validate.textNull($("#Description"),'请输入店铺描述,200字以内'))) return false;
			if($("#LogoLink").val()=="")
			{
				popup.info.init().show("请上传店铺logo", false); 	
				return false;
			}
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


function ajaxFileUpload() {
    $.ajaxFileUpload
    (
        {
            url: '/Admin/Public/UploadImg?type=Logo', //用于文件上传的服务器端请求地址
            secureuri: false, //是否需要安全协议，一般设置为false
            fileElementId: 'uploadfile', //文件上传域的ID
            dataType: 'json', //返回值类型 一般设置为json
            success: function (data, status)  //服务器成功响应处理函数
            {
	       		if(data.Result){
		  	    $("#uploadfile").replaceWith("<input type='file' id='uploadfile' name='uploadfile' style='display:none;' title=" + new Date().getTime() + "' />");
                $(".upload-file").hide();
                $("#LogoLink").val(data.Url);
                $("#img1").attr("src", data.Url+"?d="+ new Date().getTime()).show();
                }else{
                	 popup.info.init().show(data.Msg, false); 
                }
            },
            error: function (data, status, e)//服务器响应失败处理函数
            {
                 popup.info.init().show(e, false); 
            }
        }
    )
    return false;
}
