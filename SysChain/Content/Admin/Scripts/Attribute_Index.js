$(function(){
	getData();
});
getData=function(){
	$.get("/Admin/SysCategory/SelectCategory",function(data,status){
		if(status=="success")
		{
			var strHtml="";
			$.each(data,function(index,item){
				if(item.ParentID==0)
				{
					strHtml+='<a href="#" data-id="'+item.CategoryID+'" title="'+item.Name+'">'+item.Name+'</a>'; 
				}
			});
			$('div[data-level="1"][class="level-id"]').html(strHtml).on('click', 'a', function(evt){
				evt.preventDefault();	
				$("#id1").val($(this).data("id"));
				$('div[data-level="2"][class="level-id"]').html(SetHtml(data,$(this).data("id"))).on('click', 'a', function(evt){
					evt.preventDefault();	
					$("#id2").val($(this).data("id"));
					$('div[data-level="3"][class="level-id"]').html(SetHtml(data,$(this).data("id"))).on('click', 'a', function(evt){
						evt.preventDefault();	
						//SetHtml(data,$(this).data("id"));
						$("#id").val($(this).data("id"));

					});
				});
			});
			f7.plugin.footer();
		}
	});
};

function SetHtml(sources,id){
	var strHtml="";
	$.each(sources,function(i,item){
		if(item.ParentID==id)
		{
			strHtml+='<a href="#" data-id="'+item.CategoryID+'" title="'+item.Name+'">'+item.Name+'</a>'; 
		}
	});
	//console.log(strHtml);
	return strHtml;
};