
//验证码滑动
	$.fn.drag = function(options) {
		var x, drag = this, isMove = false, defaults = {};
		options = $.extend(defaults, options);

		drag.html('');
		$('.handler').data('done', 0);

		//添加背景，文字，滑块
		var html =  '<div class="drag-bg"></div>'+
					'<div class="drag-text" onselectstart="return false;" unselectable="on">拖动滑块验证</div>'+
					'<div class="handler"><i class="fa fa-angle-double-right"></i></div>';
		this.append(html);
		
		var handler = drag.find('.handler');
		var dragBg = drag.find('.drag-bg');
		var text = drag.find('.drag-text');
		var maxWidth = drag.width() - handler.width();  //能滑动的最大间距

		//鼠标按下时候的x轴的位置
		handler.mousedown(function(e) {
			isMove = true;
			x = e.pageX - parseInt(handler.css('left'), 10);
		});
		
		//鼠标指针在上下文移动时，移动距离大于0小于最大间距，滑块x轴位置等于鼠标移动距离
		$(document).mousemove(function(e) {
			var _x = e.pageX - x;
			if (isMove) {
				if (_x > 0 && _x <= maxWidth) {
					handler.css({'left': _x});
					dragBg.css({'width': _x});
					drag.find('.drag-text').text('');
				} else if (_x > maxWidth) {  //鼠标指针移动距离达到最大时清空事件
					dragOk();
				}
			}//if
		}).mouseup(function(e) {
			isMove = false;
			var _x = e.pageX - x;
			if (_x < maxWidth) { //鼠标松开时，如果没有达到最大距离位置，滑块就返回初始位置
				handler.css({'left': 0});
				dragBg.css({'width': 0});
				drag.find('.drag-text').text('拖动滑块验证');
			}
		});
		
		//完成验证
		function dragOk() {
			$('.drag-text').html('');
			$('.drag-bg').html('<b>验证通过</b>');
			$('.handler').attr('data-done', 1).html('<i class="fa fa-check-circle"></i>');
			//text.text('验证通过');
			handler.unbind('mousedown');
			$(document).unbind('mousemove');
			$(document).unbind('mouseup');
		}
	};

	//加载验证
	$(function(){
		$('#drag').drag();
	})
