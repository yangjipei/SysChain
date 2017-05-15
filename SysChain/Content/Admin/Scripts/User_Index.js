
$(function(){
	var $html = $('html');
	moudle_common_init($html,false);
	moudle_update_init($html);
	pagePlugin($html);
});

showMessage=function(msg){
	 popup.info.init().show(msg, true); 
	 setTimeout(function(){$("#frmSearch").submit();}, 3000);

};