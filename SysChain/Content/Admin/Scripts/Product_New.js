$(function(){
	getUnit();
	getGroup();

	$("#Title").blur(function(){
		var value = $.trim($(this).val()),
		len = value.length;
		if(len>0)
		{
			$(".btn-second").removeClass("btn-disabled");
		}else{
			$(".btn-second").addClass("btn-disabled");
			$(this).next().html('产品标题最多20个字');
		}
	});
	$("#Title").keyup(function(){
		var value = $.trim($(this).val()),
		len = value.length;
		if(len>0)
		{
			$(this).next().html('还可以输入 <em style="color:red;">'+(20-len).toString()+'</em> 个字');
		}
	});

});

function getUnit()
{
	$.get("/Admin/SysProduct/GetUnit",function(data,status){
		if(status=="success")
		{
			var strHtml=[];
			strHtml.push('<option value=""></option>');
			if(data!=null)
			{
				$.each(data,function(index,item){
					strHtml.push('<option value="'+item.UnitID+'">'+item.Name+'</option>');
				});
			}
			$("#UnitID").html(strHtml.join(''));
			$("#UnitID").chosen({disable_search_threshold: 7} );
		}
	});
}
function getGroup()
{
	$.get("/Admin/SysGroup/List/0",function(data,status){
		if(status=="success")
		{
			var strHtml=[],
				currentid=$("#parentid").val();
			strHtml.push('<option value=""></option>');
			if(data!=null)
			{
				$.each(data,function(index,item){
					if(item.Layer==1)
					{	
						strHtml.push('<optgroup style="color:gray;" label="'+item.Name+'">');
						$.each(data,function(i,Subitem){
							if(Subitem.ParentID==item.GroupID)
							{
								if(item.GroupID==currentid)
								{	
									strHtml.push('<option value="'+Subitem.GroupID+'" selected="selected">'+Subitem.Name+'</option>');
								}else{
									strHtml.push('<option value="'+Subitem.GroupID+'">'+Subitem.Name+'</option>');
								}
							}
						});
						strHtml.push('</optgroup>');
					}
				});
			}
			$("#GroupID").html(strHtml.join(''));
			$("#GroupID").chosen({disable_search_threshold: 7} );
		}
	});
}