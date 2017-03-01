
$(function(){
	var $html = $('html');
	moudle_delete_init($html,false);
	moudle_update_init($html);
	pagePlugin($html);
});

showMessage=function(msg){
	 popup.info.init().show(msg, true); 
	 $("#frmSearch").submit();

};