
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
