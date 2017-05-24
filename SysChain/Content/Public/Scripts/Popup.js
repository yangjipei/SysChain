
var f7 = {};

f7.popup = {};
f7.plugin = {}; //插件
f7.module={};
var popup = {
	config:{
		isInitLoading:false,
		isInitInfo:false,
		isInitAlert:false,
		isInitPanel:false,
		isInitFormError:false,
		isInitToolTip:false,
	}//config{}
};//popup{}

f7.plugin._register = (function() {
	return {
		carousel:f7.plugin.carousel,
		accordion:f7.plugin.accordion,
		chooseTab:f7.plugin.chooseTab,
		header: f7.plugin.header, //页头
		systemError: f7.plugin.systemError,//页尾
		footer: f7.plugin.footer,//页尾
	};
}); 
// 公共模块 初始化
f7.module.initialize = (function() {
	var _plugin = f7.plugin._register();

	//自动初始化 插件
	for (var pluginName in _plugin) {
		if (_plugin.hasOwnProperty(pluginName) && typeof _plugin[pluginName] === 'function') {
			_plugin[pluginName]();
		} //if
	} //for in

}); //f7.module.initialize()

(function($) {

	$(function() {
		f7.module.initialize();
	});

})(jQuery);
popup.loading = {
	init:function() {
		var config = popup.config;

		if (!config.isInitLoading) {
			config.isInitLoading = true;

			$.extend(this, f7.popup.loading());
		}//if

		return this;
	}//init()
};//popup.loading{}

popup.info = {
	init:function() {
		var config = popup.config;

		if (!config.isInitInfo) {
			config.isInitInfo = true;

			$.extend(this, f7.popup.info());
		}//if

		return this;
	}//init()
};//popup.info{}

popup.alert = {
	init:function() {
		var config = popup.config;

		if (!config.isInitAlert) {
			config.isInitAlert = true;

			$.extend(this, f7.popup.alert());
		}//if

		return this;
	}//init()
};//popup.alert{}

popup.panel = {
	init:function() {
		var config = popup.config;

		if (!config.isInitPanel) {
			config.isInitPanel = true;

			$.extend(this, f7.popup.panel());
		}//if

		return this;
	}//init()
};//popup.panel{}

popup.formError = {
	init:function() {
		var config = popup.config;

		if (!config.isInitFormError) {
			config.isInitFormError = true;

			$.extend(this, f7.popup.formError());
		}//if

		return this;
	}//init()
};//popup.formError{}

popup.toolTip = {
	init:function() {
		var config = popup.config;

		if (!config.isInitToolTip) {
			config.isInitToolTip = true;

			$.extend(this, f7.popup.toolTip());
		}//if

		return this;
	}//init()
};//popup.toolTip{}

//alert
f7.popup.alert = (function() {
	//初始化基本结构
	function initialize() {
		$('<div id="popup-alert-mask"></div>').appendTo('html').css({
			opacity: 0.4
		});

		$(['<div id="popup-alert">',
			'<div class="popup-alert-title">',
			'<span>提示：</span>',
			'<a class="popup-alert-btn-close" href="#" title="关闭"><i class="i i-plus2"></i></a>',
			'</div>',
			'<div class="popup-alert-content"></div>',
			'<div class="popup-alert-operate">',
			'<a class="popup-alert-btn-confirm" href="#" title="确定">确　定</a>',
			'</div>',
			'</div>'
		].join('')).appendTo('html');
	} //initialize()
	
	//定位 水平垂直居中
	function locator() {
		var screenWidth	 = $(window).width(),
			screenHeight = $(window).height(),
			_screenHeight = screenHeight,
			width = $('#popup-alert').width(),
			height = $('#popup-alert').height();
		
		if (screenHeight < $(document).height()) {
			screenHeight = $(document).height();
		}//if
		
		$('#popup-alert-mask').css({
			height: screenHeight
		});
		$('#popup-alert').css({
			left: (screenWidth - width) / 2,
			top: (_screenHeight - height) / 2 + $(window).scrollTop()
		});
	}//locator()

	var drag = null,
		confirmHandle = null,
		cancelHandle = null;

	//显示
	function show(content, config, isSuccess, title) {
		//config,isSuccess,title 导出
		//config,isSuccess 清空数据 for index
		var $alert = $('#popup-alert'),
			$title = $('.popup-alert-title', $alert),
			$operate = $('.popup-alert-operate').empty(),
			successClass = 'popup-alert-title-success',
			href = '#';
		
		content = '<p>'+ content +'</p>';
		
		if (config && !$.isEmptyObject(config) && $.isPlainObject(config)) {
			
			if ('confirm' in config && typeof config.confirm == 'function' || 'confirmUrl' in config) {
				if ('confirmUrl' in config) {
					href = config.confirmUrl;
				}//if
				
				if ('confirm' in config) {
					confirmHandle = config.confirm;
				}//if
				
				$operate.append('<a class="btn btn-confirm" href="'+href+'" title="确定">确　定</a>');
			}//if
			
			if ('cancel' in config && typeof config.cancel == 'function') {
				cancelHandle = config.cancel;

				$operate.append('<a class="btn btn-cancel" href="#" title="取消">取　消</a>');
			}//if
		} else {
			$operate.append('<a class="popup-alert-btn-confirm" href="#" title="确定">确　定</a>');
			
			confirmHandle = null;
			cancelHandle = null;
		}//if

		//成功 或 失败
		if (isSuccess) {
			if (!$title.hasClass(successClass)) {
				$title.addClass(successClass);
			}//if
		} else {
			if ($title.hasClass(successClass)) {
				$title.removeClass(successClass);
			}//if
		}//if

		//标题
		if (title) {
			$title.find('span').text(title);
		} else {
			$title.find('span').text('提示：');
		}//if
		
		locator();
		
	    $('#popup-alert-mask').show();
		$alert.find('.popup-alert-content').html(content).end().fadeIn('fast');
		
		setTimeout(locator, 10);
		
		$(window).on({'resize.popup_alert': locator,
					  'scroll.popup_alert': locator});	
					  		  
		$alert.on({click:hide}, '.popup-alert-btn-close,.popup-alert-btn-confirm');
		
		if (confirmHandle) {
			$alert.on({click:confirmExecute}, '.btn-confirm');
			$alert.on({click:cancelExecute}, '.btn-cancel');
		}//if
		
		drag = f7.popup.drag('#popup-alert', '#popup-alert .popup-alert-title');
	}//show()
	
	//隐藏
	function hide(evt) {
		var $a = $(evt.target).closest('a');
		// console.log($a.attr('href'));
		if ($a.length && $a.attr('href') == '#') {
			evt.stopPropagation();
			evt.preventDefault();
		}//if
		
		var $alert = $('#popup-alert');
		
		$('#popup-alert-mask').fadeOut('fast');
		$alert.hide();

		$(window).off('.popup_alert');
					     
		$alert.off({click:hide}, '.popup-alert-btn-close,.popup-alert-btn-confirm');
		
		if (confirmHandle) {
			$alert.off({click: confirmExecute}, '.btn-confirm');
			$alert.off({click: cancelExecute}, '.btn-cancel');
		}//if
		
		drag.unregister();
		//return false;
	}//hide()
	
	//确定
	function confirmExecute(evt) {
		//evt.stopPropagation();
		//evt.preventDefault();
		
		if (confirmHandle) confirmHandle(evt);//if
		hide(evt);
	}//confirmExecute()
	
	//取消
	function cancelExecute(evt) {
		evt.stopPropagation();
		evt.preventDefault();
		
		if (cancelHandle) cancelHandle(evt);//if
		hide(evt);
	}//cancelExecute()
	
	initialize();

	return {show:show, hide:hide};
});//f7.popup.alert()
// formError
f7.popup.formError = (function() {
	var $input = null;	//表单元素

	//初始化基本结构
	function initialize() {
		$(['<div id="popup-form-error">',
			   '<span class="arrow"></span>',
			   '<p class="popup-form-error-content"></p>',
		   '</div>'].join('')).appendTo('html');
	}//initialize()

	//定位 相对于 表单元素
	function locator() {
		var $popupFormError = $('#popup-form-error'),
			width = $input.innerWidth(),
			height = $input.innerHeight(),
			offset = $input.offset(),
			fontSize = $input.css('font-size');

		// console.log('formError -> width = ' + width);
		// console.log($input);

		if (width < 50) {
			width = 240;
			height = 30;

			$popupFormError.css({
				left: offset.left,
				top: offset.top + height,
				width: width,
				minHeight: height,
				// height: height,
				fontSize: fontSize,
				lineHeight: height + 'px',
				borderRadius: '3px'
			});

		} else if ($input.is('textarea')) {
			$popupFormError.css({
				left: offset.left,
				top: offset.top + height,
				width: width,
				minHeight: 30,
				// height: height,
				lineHeight: 30 + 'px',
				fontSize: fontSize,
				borderRadius: '0 0 3px 3px'
			});

		} else {
			$popupFormError.css({
				left: offset.left,
				top: offset.top + height,
				width: width,
				minHeight: height,
				// height: height,
				lineHeight: height + 'px',
				fontSize: fontSize,
				borderRadius: '0 0 3px 3px'
			});
		}//if

	}//locator()

	//显示
	function show($element, result) {
		// result = {status,info}
		var $popupFormError = $('#popup-form-error'),
			errorClass = 'popup-form-error',
			html = '<i class="fa fa-times-circle-o fa-fw fa-lg"></i>' + result.info;

		$input = $element;
		if (result.status) {
			// 真
			if ($input.hasClass('error')) $input.removeClass('error');//if

			if ($popupFormError.hasClass(errorClass)) $popupFormError.removeClass(errorClass);//if

			return true;
		} else {
			if (!$input.hasClass('error')) $input.addClass('error');//if

			if (!$popupFormError.hasClass(errorClass)) $popupFormError.addClass(errorClass);//if
		}//if

		setTimeout(locator, 10);
		setTimeout(function() {
			// $popupFormError.find('.popup-form-error-content').html(html).end().slideDown('fast');
			$popupFormError.find('.popup-form-error-content').html(html).end().fadeIn('fast');
		}, 12);

		$(window).on({
			'resize.popup_error_form': locator,
			'scroll.popup_error_form': locator
		});

		return false;
	}//show()

	//隐藏
	//isNotAnimate = true 不需要动画
	function hide(isNotAnimate) {
		var $popupFormError = $('#popup-form-error');

		if (isNotAnimate) {
			$popupFormError.hide();
		} else {
			// $popupFormError.slideUp('fast');
			$popupFormError.fadeOut('fast');

			if ($input && $input.hasClass('error')) $input.removeClass('error');//if
		}//if
		// $('#popup-form-error').slideUp('fast');
		// if ($input && $input.hasClass('error')) $input.removeClass('error');//if

		$(window).off('.popup_error_form');
	}//hide()

	// 返回构造 显示的数据
	function data(obj) {
		if ('_status' in obj && '_info' in obj) {
			return {
				status: obj._status,
				info: obj._info
			};
		} else {
			return {
				status: false,
				info: 'Error description is not exist !'
			};
		}//if
	}//data()

	initialize();

	return {
		show: show,
		hide: hide,
		data: data
	};
});//f7.popup.formError()
// info
f7.popup.info = (function() {
	//初始化基本结构
	function initialize() {
		$(['<div id="popup-info">',
			'<div class="popup-info-content"></div>',
			'</div>'
		].join('')).appendTo('html');
	} //initialize()

	//定位 水平垂直居中
	function locator() {
		var screenWidth = $(window).width(),
			screenHeight = $(window).height(),
			_screenHeight = screenHeight,
			width = $('#popup-info').width(),
			height = $('#popup-info').height();

		if (screenHeight < $(document).height()) {
			screenHeight = $(document).height();
		} //if

		$('#popup-info').css({
			left: (screenWidth - width) / 2,
			top: (_screenHeight - height) / 2 + $(window).scrollTop()
		});
	} //locator()

	var _tagHide, _redirectUrl, _intervalTag, _times = f7.popup.info.config.hideTime / 1000;
	//显示
	function show(content, isSuccess, redirectUrl) {
		if (typeof isSuccess != 'boolean') isSuccess = true; //if
		_redirectUrl = redirectUrl;
		locator();

		content += '<em>（<i>' + _times + '</i>s）</em>';

		var $popupInfo = $('#popup-info'),
			errorClass = 'popup-info-error';

		if (!isSuccess) {
			if (!$popupInfo.hasClass(errorClass)) $popupInfo.addClass(errorClass); //if
		} else {
			if ($popupInfo.hasClass(errorClass)) $popupInfo.removeClass(errorClass); //if
		} //if

		$popupInfo.find('.popup-info-content').html(content).end().fadeIn('fast');

		setTimeout(locator, 10);
		_tagHide = setTimeout(hide, f7.popup.info.config.hideTime);
		_intervalTag = setInterval(intervalHandle, 1000);

		$(window).on({
			'resize.popup_info': locator,
			'scroll.popup_info': locator
		});
	} //show()

	function intervalHandle() {
		var $popupInfo = $('#popup-info');
		if (_times > 1) {
			--_times;
			$popupInfo.find('.popup-info-content em > i').html(_times);
		} else {
			clearInterval(_intervalTag);
		} //if
	} //intervalHandle()

	//隐藏
	function hide(evt) {
		if (_tagHide) {
			clearTimeout(_tagHide);
		} //if

		$('#popup-info').fadeOut('fast', function() {
			if (_redirectUrl) {
				location.replace(_redirectUrl);
			} //if
		});

		$(window).off('.popup_info');
	} //hide()

	initialize();

	return {
		show: show,
		hide: hide
	};
}); //f7.popup.info()

//info.config
f7.popup.info.config = {
	//显示5秒后隐藏
	hideTime: 3 * 1000
}; //f7.popup.info.config{}
// loading
f7.popup.loading = (function() {
	var _isLoadingTrack = true; // 默认显示 进度条

	//初始化基本结构
	function initialize(type) {
		_isLoadingTrack = type;

		$('<div id="popup-loading-mask"></div>').appendTo('html').css({opacity: 0.4});
		$(['<div id="popup-loading">',
				'<div class="popup-loading-content"></div>',
		   '</div>'].join('')).appendTo('html');

		$(['<div id="loading-track"></div>'].join('')).appendTo('html');

		$('#popup-loading').ajaxStart(function() {
			//console.log('ajaxStart');
			if (_isLoadingTrack) {
				show();
			} else {
				show('正在处理...请稍后...');
			}//if
		}).ajaxSend(function(evt, request, settings) {
			//console.log('ajaxSend');
			setContent('正在发送数据！');
		}).ajaxStop(function() {
			//console.log('ajaxStop');
			hide();
		}).ajaxSuccess(function(evt, request, settings) {
			//console.log('ajaxSuccess');
			//setContent('操作成功！');
		}).ajaxComplete(function(){
			//console.log('ajaxComplete');
		}).ajaxError(function(evt, request, settings) {
			// console.log('ajaxError');
			popup.info.init().show('系统错误.请稍后再重试!', false); 
		});
	}//initialize()
	
	//定位 水平垂直居中
	function locator() {
		var screenWidth	 = $(window).width(),
			screenHeight = $(window).height(),
			_screenHeight = screenHeight,
			width = $('#popup-loading').width(),
			height = $('#popup-loading').height();

		if (screenHeight < $(document).height()) {
			screenHeight = $(document).height();
		}//if

		$('#popup-loading-mask').css({
			height: screenHeight
		});
		$('#popup-loading').css({
			left: (screenWidth - width) / 2,
			top: (_screenHeight - height) / 2 + $(window).scrollTop()
		});

		$('#loading-track').css({
			left: 0,
			top: $(window).scrollTop()
		});
	}//locator()

	function setContent(value) {
		if (!value) {
			value = '<i class="fa fa-spinner fa-spin fa-lg"></i>正在处理...请稍后...';
		} else {
			value = '<i class="fa fa-spinner fa-spin fa-lg"></i>' + value;
		}//if

		$('#popup-loading').find('.popup-loading-content').html(value);
	}//setContent()

	function show(content) {
		locator();

		if (!_isLoadingTrack) {
			setContent(content);

			$('#popup-loading-mask').show();
			$('#popup-loading').fadeIn('fast');
		}//if

		$('#loading-track').css('visibility', 'visible').animate({width:$(window).width() * 0.8}, 800, function() {
			/* stuff to do after animation is complete */
		});

		$(window).on({
			'resize.popup_loading': locator,
			'scroll.popup_loading': locator});
	}//show()

	function hide() {
		if (!_isLoadingTrack) {
			$('#popup-loading-mask').fadeOut('fast');
			$('#popup-loading').fadeOut('fast');

			// $('#popup-loading-mask').hide();
			// $('#popup-loading').hide();
		}//if

		$('#loading-track').stop().animate({width:$(window).width()}, 300, function() {
			$(this).css({
				width: 0,
				visibility: 'hidden'});
		});

		$(window).off('.popup_loading');
	}//hide()

	initialize(true);

	return {
		show: show,
		hide: hide,
	};
});//f7.popup.loading()
//drag
f7.popup.drag = (function(box, bar) {
	var pageHeight = $(document).height(),
		formError = popup.formError,
		isDrag = false,
	    startX = 0,
		startY = 0;
		 
	function mouseDown(evt) {
		evt.stopPropagation();
		evt.preventDefault();
		
		isDrag = true;

		var offset = $(box).offset();

		startX = evt.clientX - offset.left;
		startY = evt.clientY - offset.top;

		//return false;
	}//mouseDown()
	
	function mouseMove(evt) {
		if (isDrag) {
			evt.stopPropagation();
			evt.preventDefault();
			
		  	var $box = $(box),
				boxWidth = $box.width(),
				boxHeight = $box.height(),
				
				$screen = $(window),
				screenWidth = $screen.width(),
				screenHeight = $screen.height(),
				
			    leftX = evt.clientX - startX,
			    topY = evt.clientY - startY;

			
			leftX = leftX < 0 ? 0 : leftX;//if
			topY = topY < 0 ? 0 : topY;//if

			if (screenHeight < pageHeight) {
				screenHeight = pageHeight;
			}//if

			leftX = leftX + boxWidth > screenWidth ? screenWidth - boxWidth : leftX;//if
			topY = topY + boxHeight > screenHeight ? screenHeight - boxHeight : topY;//if
			
		  	$(box).css({left:leftX, top:topY});
			
			formError.init().hide(true);
			//return false;
		}//if
	}//mouseMove()
	
	function mouseUp(evt) {
		isDrag = false;
	}//mouseUp()
	
	function register() {
		$(bar).on({'mousedown.popup_move':mouseDown});
		$(document).on({'mousemove.popup_move':mouseMove,
						'mouseup.popup_move':mouseUp});
	}//register()
	 
	function unregister() {
		$(bar).off('.popup_move');
		$(document).off('.popup_move');
	}
	
	register();
	
	return {register:register, unregister:unregister};
});//f7.popup.drag()
//panel
f7.popup.panel = (function() {
	//初始化基本结构
	function initialize() {
		$('<div id="popup-panel-mask"></div>').appendTo('html').css({opacity:0.5});
		$('<div id="popup-panel"></div>').appendTo('html');
	}//initialize()
	
	//定位 水平垂直居中
	function locator() {
		var screenWidth	 = $(window).width(),
			screenHeight = $(window).height(),
			_screenHeight = screenHeight,
			width = $('#popup-panel').width(),
			height = $('#popup-panel').height();
		
		if (screenHeight < $(document).height()) {
			screenHeight = $(document).height();
		}//if

		// console.log('scrollTop = ' + $(window).scrollTop());
		
		$('#popup-panel-mask').css({height:screenHeight});
		$('#popup-panel').css({left:(screenWidth - width) / 2,
						       top:(_screenHeight - height) / 2 + $(window).scrollTop()});
	}//locator()

	//显示
	var drag = null;
	function show(content) {
		locator();
		
		$('#popup-panel-mask').show();
		$('#popup-panel').html(content).fadeIn('fast');
	
		setTimeout(locator, 10);
	   
		$(window).on({'resize.popup_panel':locator,
					  'scroll.popup_panel':locator});

		// $(document).on('click.popup_panel', btnCancel);
		
		$('#popup-panel').on({click:btnCancel}, '.btn-close,.btn-close-x,.btn-cancel')
						 .on({click:btnConfirm}, '.btn-confirm')
						 .find('form[data-ajax="off"]').on('submit', btnConfirm);

		// console.log($('.btn-confirm', $('#popup-panel')));

		if ($('#popup-panel').find('header').length) {
			// 存在 header 标签时
			drag = f7.popup.drag('#popup-panel', 'header');
		} else {
			drag = f7.popup.drag('#popup-panel', '#popup-panel');
		}//if
	}//show()
	
	//隐藏
	function hide() {
		$('#popup-panel-mask').fadeOut('fast', function() {
			$('#popup-panel').html('');
		});
		$('#popup-panel').hide();
		
		$(window).off('.popup_panel'); 
		
		// $('#popup-panel').off({click:btnCancel}, '.btn-close,.btn-close-x,.btn-cancel')
		// 				 .off({click:btnConfirm}, '.btn-confirm');
						
		$('#popup-panel').off('click').off('submit');

		drag.unregister();
	}//hide()

	//确定
	function btnConfirm(evt) {
		evt.preventDefault();
		evt.stopPropagation();
		
		hide();
		// console.log('btnConfirm');

		var $form = $(evt.target).closest('form[data-ajax="off"]');
		if ($form.length) {
			(function() {
				var command = f7.command,
					_default = command['default'],
					// $form = $(evt.target).parents('form'),
					action = $form.attr('action'),
					moduleId = $('body').attr('id').replace('-', '_'),
					method = action.split('/').pop().replace(/\.html/i, '').replace(/_?handle$/i, 'Handle');
				
				if (moduleId in command && method in command[moduleId]) {
					command[moduleId][method](action, $form);
				} else if(method in _default) {
					_default[method](action, $form);
				} else {
					console.log('f7.command.' + moduleId + '.' + method + '() 方法不存在！');
				}//if
			}());
		}//if

	}//btnConfirm()
	
	//取消/关闭
	function btnCancel(evt) {
		evt.preventDefault();
		evt.stopPropagation();
		
		// 关闭/隐藏 表单 错误
		popup.formError.init().hide(true);

		hide();
		// console.log('btnCancel');

		var $form = $(evt.target).closest('form[data-ajax="off"]');
		if ($form.length) {
			(function() {
				var command = f7.command,
					_default = command['default'],
					// $form = $(evt.target).parents('form'),
					action = $form.attr('action'),
					moduleId = $('body').attr('id').replace('-', '_'),
					method = action.split('/').pop().replace(/\.html/i, '').replace(/_?handle$/i, 'Cancel');
					
				if (moduleId in command && method in command[moduleId]) {
					command[moduleId][method](action, $form);
				} else if(method in _default) {
					_default[method](action, $form);
				} else {
					console.log('f7.command.' + moduleId + '.' + method + '() 方法不存在！');
				}//if
			}());
		}//if

	}//btnCancel()


	initialize();

	return {show:show, hide:hide};
});//f7.popup.panel()
// toolTip 只能用于 表单元素
f7.popup.toolTip = (function() {
	var $element = null;	//表单元素

	//初始化基本结构
	function initialize() {
		$(['<div id="popup-tool-tip">',
			   '<span class="arrow"></span>',
			   '<p class="popup-tool-tip-content"></p>',
		   '</div>'].join('')).appendTo('html');
	}//initialize()

	//定位 相对于 表单元素
	function locator() {
		var $popupToolTip = $('#popup-tool-tip'),
			width = $element.outerWidth(),
			height = $element.outerHeight(),
			offset = $element.offset(),
			fontSize = $element.css('font-size');

		// console.log('toolTip -> width = ' + width);
		// console.log($element);
		$popupToolTip.css({
			left: offset.left + (width - $popupToolTip.outerWidth()) / 2,
			top: offset.top + height + 10,
			fontSize: fontSize,
			lineHeight: height + 'px',
			borderRadius: '3px'
		});
	}//locator()

	//显示
	function show($target, content) {
		// console.log(content);

		var $popupToolTip = $('#popup-tool-tip');
		$element = $target;

		setTimeout(locator, 10);
		setTimeout(function() {
			$popupToolTip.find('.popup-tool-tip-content').html(content).end().show();
		}, 12);
		setTimeout(locator, 20);

		$(window).on({
			'resize.popup_tool_tip': locator,
			'scroll.popup_tool_tip': locator
		});

		return false;
	}//show()

	//隐藏
	//isNotAnimate = true 不需要动画
	function hide() {
		var $popupToolTip = $('#popup-tool-tip');

		$popupToolTip.hide();

		$(window).off('.popup_tool_tip');
	}//hide()

	initialize();

	return {
		show: show,
		hide: hide
	};
});//f7.popup.toolTip()

// header 部分
f7.plugin.header = function() {
	//顶部点击菜单下拉
	(function() {
		var $header = $('#header'),
			$menuList = $('#menu-list', $header);

		$header.on('click', '.menu', function(evt) {
			evt.preventDefault();
			evt.stopPropagation();

			var $menu = $(evt.target).closest('li'),
				$a = $menu.find('a');
				console.log($menuList.is(':hidden'));
			if ($menuList.is(':hidden')) {
				$a.addClass('current');
				$menuList.slideDown();
			} else {
				$a.removeClass('current');
				$menuList.slideUp();
			} //if
		});

		$(document).on('click', function(evt) {
			// console.log(evt.type);
			if ($menuList.is(':visible')) {
				$header.find('li.menu a').removeClass('current');
				$menuList.slideUp();
			} //if
		});

	}());

}; //f7.plugin.header()

f7.plugin.footer = function() {
	// console.log('plugin.footer');

	// 满屏显示
	(function() {
		var screenHeight = $(window).height(),
			height = $('body').height();

		// console.log(screenHeight, height);
		if (height < screenHeight) {
			var $main = $('main');
			$main.css({
				height: $main.height() + (screenHeight - height) + 'px',
			});
		} //if
	}());

		// iframe 中的 页面隐藏部分结构
	(function() {
		if (window.frames.length != parent.frames.length) {

			var $panel = $('#panel');

			$('div[id^="header"]').hide();
			$('#footer').hide();

			$panel.wrap('<div id="popup-panel"></div>').find('header').hide().end().parent().css({
				display: 'block',
			});
		} //if
	}());

	// popup.info.init().show('看一下效果');
	// popup.info.init().show('看一下效果', false);
	// popup.alert.init().show('看一下Alert 警告框');
	// setTimeout(function () {
	// 	// popup.toolTip.init().show($('input[name="keyword"]'), '这是测试内容');
	// 	// popup.formError.init().show($('input[name="keyword"]'), {status: false, info: '搜索内容不能为空！'});
	// }, 1000);

}; //f7.plugin.footer()
// operate-jump.js
// 出错 界面 倒计时 5秒 回退
f7.plugin.systemError = function() {

	var $systemError = $('.system-error'),
		$strong = $systemError.find('p strong'),
		$a = $systemError.find('p a'),
		href = $a.attr('href'),
		_tagInterval = null;

	if (!$systemError.length) {
		return;
	} //if

	_tagInterval = setInterval(function() {
		var times = parseInt($strong.text());

		$strong.text(--times + '秒');

		if (times <= 0) {
			clearInterval(_tagInterval);
			if (href === '#') {
				window.history.back();
			} else {
				window.location.href = href;
			} //if
		} //if

	}, 1000);

};

// accordion 部分
f7.plugin.accordion = (function() {
	var $accordion = $('aside[data-accordion="on"]');
	if (!$accordion.length) {
		return;
	} //if

	close();

	function close() {
		$accordion.find('.panel dl dd').addClass('hide').end().find('.panel dl').addClass('hide');
		$accordion.find('.panel header a').removeClass('current');
	} //close()

	function removeCurrent() {
		$accordion.find('.panel header a').removeClass('current').find('i[class*="fa-angle-"]').removeClass('fa-angle-up').addClass('fa-angle-down');
		$accordion.find('.panel dl dd').addClass('hide').end().find('.panel dl').addClass('hide');
		$accordion.find('.panel dl dt a').removeClass('current').find('i.i, i.fa').removeClass('i-arrow-down5').addClass('i-arrow-right5');
	} //removeCurrent()

	function removeSubCurrent() {
		$accordion.find('.panel dl dd').addClass('hide');
		$accordion.find('.panel dl dt a').removeClass('current').find('i.i, i.fa').removeClass('i-arrow-down5').addClass('i-arrow-right5');
	} //removeSubCurrent()

	$accordion.on('click', '.panel header a', function(evt) {
		evt.preventDefault();
		// evt.stopPropagation();

		var $a = $(evt.target).closest('a'),
			$fa = $a.find('i[class*="fa-angle-"]'),
			$panel = $a.parents('.panel'),
			$dl = $panel.find('dl.child');

		removeCurrent();

		if ($fa.is('.fa-angle-down')) {
			// 展开
			$a.addClass('current');

			$fa.addClass('fa-angle-up').removeClass('fa-angle-down');

			$dl.removeClass('hide');
		} else {
			// 折叠
			$a.removeClass('current');

			$fa.addClass('fa-angle-down').removeClass('fa-angle-up');

			$dl.addClass('hide');
		} //if
	}).on('click', '.panel dl.child dt a', function(evt) {
		evt.preventDefault();
		// evt.stopPropagation();
		var $a = $(evt.target).closest('a'),
			$i = $a.find('i.i, i.fa'),
			$dt = $a.parents('dt'),
			$dd = $dt.next('dd');

		removeSubCurrent();

		if ($i.is('.i-arrow-right5') || $i.is('.fa-angle-right')) {
			$a.addClass('current');

			if ($i.is('.i-arrow-right5')) {
				$i.addClass('i-arrow-down5').removeClass('i-arrow-right5');
			} else {
				$i.addClass('fa-angle-down').removeClass('fa-angle-right');
			} //if

			$dd.removeClass('hide');
		} else {
			$a.removeClass('current');

			if ($i.is('.i-arrow-down5')) {
				$i.addClass('i-arrow-right5').removeClass('i-arrow-down5');
			} else {
				$i.addClass('fa-angle-right').removeClass('fa-angle-right');
			} //if

			$dd.addClass('hide');
		} //if
	});

	(function() {
		// console.log(location);
		var search = location.search.split('?')[1];
		if (search) {
			var args = search.split('&'),
				_temp = [],
				$target = null;

			for (var index in args) {
				_temp = args[index].toLowerCase().split('=');

				// if (['pid', 'sid', 'id'].indexOf(_temp[0]) != -1) {
				// 	$target = $accordion.find('[data-id="' + _temp[1] + '"]');
				// 	$target.click();
				// 	if (!$target.is('.current')) {
				// 		$target.addClass('current');
				// 	} //if
				// } //if

				if (_temp[0] == 'id') {
					clickTarget(_temp[1]);
				} //if
			} //for in
		} //if

		var pathname = /index\/([\d]+)/i.exec(location.pathname);
		// var pathname = /index\/([\d]+)/i.exec('Admin/SysAttribute/Index/5');
		if (pathname && pathname.length) {
			var id = pathname[1];
			if (id != '') {
				clickTarget(id);
			} //if
		} //if

		function clickTarget(id) {
			$target = $accordion.find('[data-id="' + id + '"]');
			if (!$target.is('.current')) {
				$target.addClass('current');
			} //if
			var $dd = $target.parents('dd'),
				$dl = $dd.parents('dl');
			$dl.prev('header').find('a').click();
			$dd.prev('dt').find('a').click();
		} //clickTarget()

	}());

}); //f7.plugin.accordion()
f7.plugin.chooseTab = (function() {
	var $tab = $('.tab-dynamic');
	if (!$tab.length) {
		return false;
	} //if

	var $form = $tab.parents('form'),
		$btn = $('footer :submit, footer .btn', $form),
		$hintFull = $form.find('.hint-full'),
		$stacks = $('.stack', $tab),
		$ol = $('ol', $tab);

	$ol.on('click', 'a', function(evt) {
		evt.preventDefault();
		evt.stopPropagation();
		var $a = $(this),
			stack = $a.data('stack'),
			$currentStack = $('[class*="stack-' + stack + '"]', $tab);

		if ($a.is('.default')) {
			return;
		} //if
		// console.log('stack = ' + stack);

		$a.removeClass('current').parent('li').prev('li').find('a').addClass('current');
		$a.addClass('hide').parent('li').nextAll('li').find('a').addClass('hide').removeClass('current');

		$currentStack.addClass('hide').find('a').removeClass('current').end().next('.stack').addClass('hide').find('a').removeClass('current');
		$currentStack.prev('.stack').removeClass('hide');

		$hintFull.find('[data-stack="' + stack + '"]').text('-').next('span').text('-');

		// 设置按钮
		$btn.addClass('btn-disabled');
		if ($btn.is(':submit')) {
			$btn.prop('disabled', true);
		} //if
	});

	$stacks.on('click', 'a', function(evt) {
		evt.preventDefault();
		evt.stopPropagation();

		var $a = $(this),
			level = $a.parents('div').data('level'),
			label = $a.text(),
			$stack = $a.parents('.stack');

		// console.log('level = ' + level, label);
		$a.addClass('current').siblings('a').removeClass('current');

		if (level + 1 <= 3) {
			// 显示tab
			$ol.find('a.current').removeClass('current');
			$ol.find('a[data-stack="' + (level + 1) + '"]').removeClass('hide').addClass('current').find('span').text(label);

			// 隐藏 当前stack
			$stack.addClass('hide');
			// 显示 下一级stack
			$stack.next('.stack').removeClass('hide');
		} else {
			$btn.removeClass('btn-disabled');
			if ($btn.is(':submit')) {
				$btn.prop('disabled', false);
			} //if
		} //if

		// 显示 对应级别提示文件
		$hintFull.find('span[data-stack="' + level + '"]').text(label);
		$('[name="level'+level+'"]:hidden').val(label);
	});
});
// 旋转木马
f7.plugin.carousel = (function() {
	var $carousel = $('.carousel');
	if (!$carousel.length) return; //if

	$carousel.each(function(index, element) {
		new Carousel($(element));
	});

	function Carousel($carousel) {
		// var $carousel = $('.carousel'),
		var _config = null,
			_dragBln = false,
			_timer = null,
			_isMobile = false;

		// console.log($carousel.length);

		if (!$carousel.length) return; //if

		if (navigator.userAgent.match(/mobile/i)) {
			_isMobile = true;
			$carousel.find('.prev,.next').hide();
		} //if

		// 
		_timer = setInterval(function() {
			$('.next', $carousel).click();
		}, 5000);

		$carousel.hover(function() {
			if (!_isMobile) {
				$('.prev,.next', $carousel).fadeIn();
			} //if

			clearInterval(_timer);
		}, function() {
			if (!_isMobile) {
				$('.prev,.next', $carousel).fadeOut();
			} //if

			_timer = setInterval(function() {
				$('.next', $carousel).click();
			}, 5000);
		});

		_config = {
			flexible: true,
			speed: 450,
			btn_prev: $('.prev', $carousel),
			btn_next: $('.next', $carousel),
			paging: $('.tag a', $carousel),
			counter: function(evt) {
				$('.tag a', $carousel).removeClass('on').eq(evt.current - 1).addClass('on');
			}
		};


		$('.main-image', $carousel).touchSlider(_config).bind('mousedown', function() {
			_dragBln = false;
		}).bind('dragstart', function() {
			_dragBln = true;
		}).find('li a').click(function() {
			if (_dragBln) {
				return false;
			} //if
		}).end().bind('touchstart', function() {
			clearInterval(_timer);
		}).bind('touchend', function() {
			_timer = setInterval(function() {
				$('.next', $carousel).click();
			}, 5000);
		});
	} //Carousel()
}); 






//通过更新状态
moudle_update_init=function($html){
	var	$updateInit=$('a[data-update-status="on"]', $html);
		if ($updateInit.length) {
		$html.on('click', 'a[data-update-status="on"]', function(evt) {
			evt.stopPropagation();
			evt.preventDefault();
			var url=$(this).attr("href");
			popup.loading.init();
			$.ajax({  
			    type: "Post",  
			    url: url,
			    dataType: "json",   
			    success: function (data) {  
			      if (data.Result) { 
			      popup.info.init().show(data.Msg, true);
			      $("#frmSearch").submit();
			      }  
			      else {  
			       popup.info.init().show(data.Msg, false); 
			      }  
			    },   
			    error: function (XMLHttpRequest, textStatus, errorThrown) {  
					 popup.info.init().show("系统错误.", false); 
			    }  
			}); 
		});
	};
};
//删除通用操作
moudle_common_init=function($html,$flag,tips){
	$delInit=$('a[data-common-moudle="on"]', $html)
		if ($delInit.length) {
		$html.on('click', 'a[data-common-moudle="on"]', function(evt) {
			evt.stopPropagation();
			evt.preventDefault();
			var url=$(this).attr("href");
			if($flag)
			{
				var temp=$(this).data("flag");
				var msg=$(this).data("msg");
				if(temp>0)
				{
					popup.alert.init().show(msg,"",true,"提示");
					return;
				}
			}
			if(tips==null|| tips=="")
			{
				tips=$(this).data("msg");
			}
			popup.loading.init();
			popup.alert.init().show(tips,{confirm:function(evt){
				$.ajax({  
				    type: "Post",  
				    url: url,
				    dataType: "json",   
				    success: function (data) {  
				      if (data.Result) { 
				      showMessage(data.Msg, true);
				      }  
				      else {  
			           showMessage(data.Msg, false);
				      }  
				    },   
				    error: function (XMLHttpRequest, textStatus, errorThrown) {  
						 popup.info.init().show("系统错误.", false); 
				    }  
				}); 
			},cancel:function(evt){
				return;
			}},true,$(this).attr("title"));

		});
	};
};
 //通用分页
pagePlugin=function($html)
 {
 	var $pageInit = $('a[data-page-init="on"]', $html);
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
}

showMessage=function(msg,flag){
	 popup.info.init().show(msg, flag); 
	 setTimeout(function(){$("#frmSearch").submit();}, 3000);
};

