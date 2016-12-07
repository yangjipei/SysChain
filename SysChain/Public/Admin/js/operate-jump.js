// operate-jump.js
(function($) {
	
	$(function(){
		var wait = $('#wait'),
			href = $('#href').attr('href');
			
		var interval = setInterval(function(){
			var time = wait.text();
			wait.text(--time);
			
			if(time <= 0) {
				location.href = href;
				clearInterval(interval);
			};
	
		}, 1000);
	});
	
})(jQuery);