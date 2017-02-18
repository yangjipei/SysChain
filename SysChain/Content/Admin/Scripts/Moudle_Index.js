$(function(){
	getData();
	$("#pid").on("change",function(evt){
		evt.preventDefault();
		evt.stopPropagation();
		$("#frmSearch").submit();
	});

	var $html = $('html');
	moudle_delete_init($html,false,'该模块及其子模块都会被删除且无法恢复，是否继续操作？');
	moudle_update_init($html);
	pagePlugin($html);
	$(".btn-sort").on('click',function(evt){
			evt.stopPropagation();
			evt.preventDefault();
			var type=$(this).data("type"),
				 target=$(this).data("tid"),
				 url=$(this).attr('href');
			popup.loading.init();
			$.ajax({  
			    type: "Post",  
			    url: url,
			    data: {targetid:target},  
			    dataType: "json",   
			    success: function (data) {  
			      if (data.Result) { 
			      popup.info.init().show(data.Msg, true);
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


});
btnInit=function(){
	$("#btnNew").attr("href","/Admin/SysMoudle/New?pid="+$("#parentid").val());
}
getData=function(){
	$.get("/Admin/SysMoudle/List/0",function(data,status){
		if(status=="success")
		{
			var strHtml=[],
				currentid=$("#parentid").val();

			strHtml.push('<option value="0">请选择...</option>');
			if(data!=null)
			{
				$.each(data,function(i){
					if(data[i].MoudleID==currentid)
					{	
						strHtml.push('<option value="'+data[i].MoudleID+'" selected="selected">'+data[i].Name+'</option>');
					}else{
						strHtml.push('<option value="'+data[i].MoudleID+'">'+data[i].Name+'</option>');
					}
				});
			}
			$("#pid").html(strHtml.join(''));
			btnInit();
		}
	});
};
showMessage=function(msg){
	 popup.info.init().show(msg, true); 
	 $("#frmSearch").submit();
};