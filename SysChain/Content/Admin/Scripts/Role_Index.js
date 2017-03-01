$(function(){
	var $html = $('html');
	moudle_delete_init($html,true,'该角色及角色权限配置都会被删除且无法恢复，<br />是否继续操作？');
	moudle_update_init($html);
	pagePlugin($html);
});
showMessage=function(msg){
	 popup.info.init().show(msg, true); 
	 $("#frmSearch").submit();
};