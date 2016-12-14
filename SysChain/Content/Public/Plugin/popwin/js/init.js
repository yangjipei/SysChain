$(function(){
	var $html = $('html'),
		$moduleInit = $('a[data-panel-init="on"]', $html);

	if (!$moduleInit.length) {
		return ;
	}


	$html.on('click', 'a[data-panel-init="on"]', function(evt) {
		evt.stopPropagation();
		evt.preventDefault();
		var $a = $(this),
		url = $a.attr('href');
		popWin.showWin('800','600','新建模块',url);
	});
});