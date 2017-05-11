$(function(){
	var $html = $('html');
	//moudle_common_init($html,false,'该品类及其子品类都会被删除且无法恢复，<br />是否继续操作？');
	//moudle_update_init($html);
	pagePlugin($html);
});
showMessage=function(msg){
	 popup.info.init().show(msg, true); 
	 setTimeout(function(){$("#frmSearch").submit();}, 3000);
};