$(function(){
	getData();
	$("#pid").on("change",function(evt){
		evt.preventDefault();
		evt.stopPropagation();

		$("#frmSearch").submit();
	});
	var $html = $('html'),
		$pageInit = $('a[data-page-init="on"]', $html);

	if ($pageInit.length) {
		$html.on('click', 'a[data-page-init="on"]', function(evt) {
			evt.stopPropagation();
			evt.preventDefault();
			var $a = $(this),
			page = $a.data('size');
			$("#index").val(page);
			$("#frmSearch").submit();
		});
	};
	$(".btn-sort").on('click',function(evt){
			evt.stopPropagation();
			evt.preventDefault();
			var type=$(this).data("type"),
				 target=$(this).data("tid"),
				 url=$(this).attr('href');
			if(type==1)
			{
				 popup.info.init().show("上升，目标ID："+target, true); 
			}else{
				 popup.info.init().show("下降，目标ID："+target, true); 
			}
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
}