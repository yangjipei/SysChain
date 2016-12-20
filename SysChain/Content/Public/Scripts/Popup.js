
var f7 = {};

f7.popup = {};
f7.plugin = {}; //插件
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
		$('<div id="popup-alert-mask"></div>').appendTo('html').css({opacity: 0.4});
		
		$(['<div id="popup-alert">',
		       '<div class="popup-alert-title">',
			       '<span>提示：</span>',
				   '<a class="popup-alert-btn-close" href="#" title="关闭"><i class="fa fa-times fa-2x"></i></a>',
			   '</div>',
			   '<div class="popup-alert-content"></div>',
			   '<div class="popup-alert-operate">',
				   '<a class="popup-alert-btn-confirm" href="#" title="确定">确　定</a>',
			   '</div>',
		   '</div>'].join('')).appendTo('html');
	}//initialize()
	
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
		   '</div>'].join('')).appendTo('html');
	}//initialize()

	//定位 水平垂直居中
	function locator() {
		var screenWidth	 = $(window).width(),
			screenHeight = $(window).height(),
			_screenHeight = screenHeight,
			width = $('#popup-info').width(),
			height = $('#popup-info').height();

		if (screenHeight < $(document).height()) {
			screenHeight = $(document).height();
		}//if

		$('#popup-info').css({left:(screenWidth - width) / 2,
						      top:(_screenHeight - height) / 2 + $(window).scrollTop()});
	}//locator()

	var _tagHide;
	//显示
	function show(content, isSuccess) {
		if (typeof isSuccess != 'boolean') isSuccess = true;//if
		locator();

		var $popupInfo = $('#popup-info'),
			errorClass = 'popup-info-error';

		if (!isSuccess) {
			if (!$popupInfo.hasClass(errorClass)) $popupInfo.addClass(errorClass);//if
		} else {
			if ($popupInfo.hasClass(errorClass)) $popupInfo.removeClass(errorClass);//if
		}//if

		$popupInfo.find('.popup-info-content').html(content).end().fadeIn('fast');

		setTimeout(locator,10);
		var _tagHide = setTimeout(hide, f7.popup.info.config.hideTime);

		$(window).on({'resize.popup_info': locator,
					  'scroll.popup_info': locator});
	}//show()

	//隐藏
	function hide(evt) {
		if(_tagHide) {
			clearTimeout(_tagHide);
		}//if

		$('#popup-info').fadeOut('fast');

		$(window).off('.popup_info');
	}//hide()

	initialize();

	return {
		show: show,
		hide: hide
	};
});//f7.popup.info()
//info.config
f7.popup.info.config = {
	//显示5秒后隐藏
	hideTime: 3 * 1000
};//f7.popup.info.config{}
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
f7.plugin.header = (function() {

	//顶部点击菜单下拉
	(function() {
		var $header = $('#header'),
			$menuList = $('#menu-list', $header);

		$header.on('click', '.menu', function(evt) {
			evt.preventDefault();
			evt.stopPropagation();

			var $menu = $(evt.target).closest('li'),
				$a = $menu.find('a');

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

}); //f7.plugin.header()

f7.plugin.footer = (function() {
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

})(); //f7.plugin.footer()






