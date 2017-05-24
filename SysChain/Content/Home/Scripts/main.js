/*! 
 * jquery.event.drag - v 2.2
 * Copyright (c) 2010 Three Dub Media - http://threedubmedia.com
 * Open Source MIT License - http://threedubmedia.com/code/license
 */
// Created: 2008-06-04 
// Updated: 2012-05-21
// REQUIRES: jquery 1.7.x

;(function( $ ){

// add the jquery instance method
$.fn.drag = function( str, arg, opts ){
	// figure out the event type
	var type = typeof str == "string" ? str : "",
	// figure out the event handler...
	fn = $.isFunction( str ) ? str : $.isFunction( arg ) ? arg : null;
	// fix the event type
	if ( type.indexOf("drag") !== 0 ) 
		type = "drag"+ type;
	// were options passed
	opts = ( str == fn ? arg : opts ) || {};
	// trigger or bind event handler
	return fn ? this.bind( type, opts, fn ) : this.trigger( type );
};

// local refs (increase compression)
var $event = $.event, 
$special = $event.special,
// configure the drag special event 
drag = $special.drag = {
	
	// these are the default settings
	defaults: {
		which: 1, // mouse button pressed to start drag sequence
		distance: 0, // distance dragged before dragstart
		not: ':input', // selector to suppress dragging on target elements
		handle: null, // selector to match handle target elements
		relative: false, // true to use "position", false to use "offset"
		drop: true, // false to suppress drop events, true or selector to allow
		click: false // false to suppress click events after dragend (no proxy)
	},
	
	// the key name for stored drag data
	datakey: "dragdata",
	
	// prevent bubbling for better performance
	noBubble: true,
	
	// count bound related events
	add: function( obj ){ 
		// read the interaction data
		var data = $.data( this, drag.datakey ),
		// read any passed options 
		opts = obj.data || {};
		// count another realted event
		data.related += 1;
		// extend data options bound with this event
		// don't iterate "opts" in case it is a node 
		$.each( drag.defaults, function( key, def ){
			if ( opts[ key ] !== undefined )
				data[ key ] = opts[ key ];
		});
	},
	
	// forget unbound related events
	remove: function(){
		$.data( this, drag.datakey ).related -= 1;
	},
	
	// configure interaction, capture settings
	setup: function(){
		// check for related events
		if ( $.data( this, drag.datakey ) ) 
			return;
		// initialize the drag data with copied defaults
		var data = $.extend({ related:0 }, drag.defaults );
		// store the interaction data
		$.data( this, drag.datakey, data );
		// bind the mousedown event, which starts drag interactions
		$event.add( this, "touchstart mousedown", drag.init, data );
		// prevent image dragging in IE...
		if ( this.attachEvent ) 
			this.attachEvent("ondragstart", drag.dontstart ); 
	},
	
	// destroy configured interaction
	teardown: function(){
		var data = $.data( this, drag.datakey ) || {};
		// check for related events
		if ( data.related ) 
			return;
		// remove the stored data
		$.removeData( this, drag.datakey );
		// remove the mousedown event
		$event.remove( this, "touchstart mousedown", drag.init );
		// enable text selection
		drag.textselect( true ); 
		// un-prevent image dragging in IE...
		if ( this.detachEvent ) 
			this.detachEvent("ondragstart", drag.dontstart ); 
	},
		
	// initialize the interaction
	init: function( event ){ 
		// sorry, only one touch at a time
		if ( drag.touched ) 
			return;
		// the drag/drop interaction data
		var dd = event.data, results;
		// check the which directive
		if ( event.which != 0 && dd.which > 0 && event.which != dd.which ) 
			return; 
		// check for suppressed selector
		if ( $( event.target ).is( dd.not ) ) 
			return;
		// check for handle selector
		if ( dd.handle && !$( event.target ).closest( dd.handle, event.currentTarget ).length ) 
			return;

		drag.touched = event.type == 'touchstart' ? this : null;
		dd.propagates = 1;
		dd.mousedown = this;
		dd.interactions = [ drag.interaction( this, dd ) ];
		dd.target = event.target;
		dd.pageX = event.pageX;
		dd.pageY = event.pageY;
		dd.dragging = null;
		// handle draginit event... 
		results = drag.hijack( event, "draginit", dd );
		// early cancel
		if ( !dd.propagates )
			return;
		// flatten the result set
		results = drag.flatten( results );
		// insert new interaction elements
		if ( results && results.length ){
			dd.interactions = [];
			$.each( results, function(){
				dd.interactions.push( drag.interaction( this, dd ) );
			});
		}
		// remember how many interactions are propagating
		dd.propagates = dd.interactions.length;
		// locate and init the drop targets
		if ( dd.drop !== false && $special.drop ) 
			$special.drop.handler( event, dd );
		// disable text selection
		drag.textselect( false ); 
		// bind additional events...
		if ( drag.touched )
			$event.add( drag.touched, "touchmove touchend", drag.handler, dd );
		else 
			$event.add( document, "mousemove mouseup", drag.handler, dd );
		// helps prevent text selection or scrolling
		if ( !drag.touched || dd.live )
			return false;
	},	
	
	// returns an interaction object
	interaction: function( elem, dd ){
		var offset = $( elem )[ dd.relative ? "position" : "offset" ]() || { top:0, left:0 };
		return {
			drag: elem, 
			callback: new drag.callback(), 
			droppable: [],
			offset: offset
		};
	},
	
	// handle drag-releatd DOM events
	handler: function( event ){ 
		// read the data before hijacking anything
		var dd = event.data;	
		// handle various events
		switch ( event.type ){
			// mousemove, check distance, start dragging
			case !dd.dragging && 'touchmove': 
				event.preventDefault();
			case !dd.dragging && 'mousemove':
				//  drag tolerance, x� + y� = distance�
				if ( Math.pow(  event.pageX-dd.pageX, 2 ) + Math.pow(  event.pageY-dd.pageY, 2 ) < Math.pow( dd.distance, 2 ) ) 
					break; // distance tolerance not reached
				event.target = dd.target; // force target from "mousedown" event (fix distance issue)
				drag.hijack( event, "dragstart", dd ); // trigger "dragstart"
				if ( dd.propagates ) // "dragstart" not rejected
					dd.dragging = true; // activate interaction
			// mousemove, dragging
			case 'touchmove':
				event.preventDefault();
			case 'mousemove':
				if ( dd.dragging ){
					// trigger "drag"		
					drag.hijack( event, "drag", dd );
					if ( dd.propagates ){
						// manage drop events
						if ( dd.drop !== false && $special.drop )
							$special.drop.handler( event, dd ); // "dropstart", "dropend"							
						break; // "drag" not rejected, stop		
					}
					event.type = "mouseup"; // helps "drop" handler behave
				}
			// mouseup, stop dragging
			case 'touchend': 
			case 'mouseup': 
			default:
				if ( drag.touched )
					$event.remove( drag.touched, "touchmove touchend", drag.handler ); // remove touch events
				else 
					$event.remove( document, "mousemove mouseup", drag.handler ); // remove page events	
				if ( dd.dragging ){
					if ( dd.drop !== false && $special.drop )
						$special.drop.handler( event, dd ); // "drop"
					drag.hijack( event, "dragend", dd ); // trigger "dragend"	
				}
				drag.textselect( true ); // enable text selection
				// if suppressing click events...
				if ( dd.click === false && dd.dragging )
					$.data( dd.mousedown, "suppress.click", new Date().getTime() + 5 );
				dd.dragging = drag.touched = false; // deactivate element	
				break;
		}
	},
		
	// re-use event object for custom events
	hijack: function( event, type, dd, x, elem ){
		// not configured
		if ( !dd ) 
			return;
		// remember the original event and type
		var orig = { event:event.originalEvent, type:event.type },
		// is the event drag related or drog related?
		mode = type.indexOf("drop") ? "drag" : "drop",
		// iteration vars
		result, i = x || 0, ia, $elems, callback,
		len = !isNaN( x ) ? x : dd.interactions.length;
		// modify the event type
		event.type = type;
		// remove the original event
		event.originalEvent = null;
		// initialize the results
		dd.results = [];
		// handle each interacted element
		do if ( ia = dd.interactions[ i ] ){
			// validate the interaction
			if ( type !== "dragend" && ia.cancelled )
				continue;
			// set the dragdrop properties on the event object
			callback = drag.properties( event, dd, ia );
			// prepare for more results
			ia.results = [];
			// handle each element
			$( elem || ia[ mode ] || dd.droppable ).each(function( p, subject ){
				// identify drag or drop targets individually
				callback.target = subject;
				// force propagtion of the custom event
				event.isPropagationStopped = function(){ return false; };
				// handle the event	
				result = subject ? $event.dispatch.call( subject, event, callback ) : null;
				// stop the drag interaction for this element
				if ( result === false ){
					if ( mode == "drag" ){
						ia.cancelled = true;
						dd.propagates -= 1;
					}
					if ( type == "drop" ){
						ia[ mode ][p] = null;
					}
				}
				// assign any dropinit elements
				else if ( type == "dropinit" )
					ia.droppable.push( drag.element( result ) || subject );
				// accept a returned proxy element 
				if ( type == "dragstart" )
					ia.proxy = $( drag.element( result ) || ia.drag )[0];
				// remember this result	
				ia.results.push( result );
				// forget the event result, for recycling
				delete event.result;
				// break on cancelled handler
				if ( type !== "dropinit" )
					return result;
			});	
			// flatten the results	
			dd.results[ i ] = drag.flatten( ia.results );	
			// accept a set of valid drop targets
			if ( type == "dropinit" )
				ia.droppable = drag.flatten( ia.droppable );
			// locate drop targets
			if ( type == "dragstart" && !ia.cancelled )
				callback.update(); 
		}
		while ( ++i < len )
		// restore the original event & type
		event.type = orig.type;
		event.originalEvent = orig.event;
		// return all handler results
		return drag.flatten( dd.results );
	},
		
	// extend the callback object with drag/drop properties...
	properties: function( event, dd, ia ){		
		var obj = ia.callback;
		// elements
		obj.drag = ia.drag;
		obj.proxy = ia.proxy || ia.drag;
		// starting mouse position
		obj.startX = dd.pageX;
		obj.startY = dd.pageY;
		// current distance dragged
		obj.deltaX = event.pageX - dd.pageX;
		obj.deltaY = event.pageY - dd.pageY;
		// original element position
		obj.originalX = ia.offset.left;
		obj.originalY = ia.offset.top;
		// adjusted element position
		obj.offsetX = obj.originalX + obj.deltaX; 
		obj.offsetY = obj.originalY + obj.deltaY;
		// assign the drop targets information
		obj.drop = drag.flatten( ( ia.drop || [] ).slice() );
		obj.available = drag.flatten( ( ia.droppable || [] ).slice() );
		return obj;	
	},
	
	// determine is the argument is an element or jquery instance
	element: function( arg ){
		if ( arg && ( arg.jquery || arg.nodeType == 1 ) )
			return arg;
	},
	
	// flatten nested jquery objects and arrays into a single dimension array
	flatten: function( arr ){
		return $.map( arr, function( member ){
			return member && member.jquery ? $.makeArray( member ) : 
				member && member.length ? drag.flatten( member ) : member;
		});
	},
	
	// toggles text selection attributes ON (true) or OFF (false)
	textselect: function( bool ){ 
		$( document )[ bool ? "unbind" : "bind" ]("selectstart", drag.dontstart )
			.css("MozUserSelect", bool ? "" : "none" );
		// .attr("unselectable", bool ? "off" : "on" )
		document.unselectable = bool ? "off" : "on"; 
	},
	
	// suppress "selectstart" and "ondragstart" events
	dontstart: function(){ 
		return false; 
	},
	
	// a callback instance contructor
	callback: function(){}
	
};

// callback methods
drag.callback.prototype = {
	update: function(){
		if ( $special.drop && this.available.length )
			$.each( this.available, function( i ){
				$special.drop.locate( this, i );
			});
	}
};

// patch $.event.$dispatch to allow suppressing clicks
var $dispatch = $event.dispatch;
$event.dispatch = function( event ){
	if ( $.data( this, "suppress."+ event.type ) - new Date().getTime() > 0 ){
		$.removeData( this, "suppress."+ event.type );
		return;
	}
	return $dispatch.apply( this, arguments );
};

// event fix hooks for touch events...
var touchHooks = 
$event.fixHooks.touchstart = 
$event.fixHooks.touchmove = 
$event.fixHooks.touchend =
$event.fixHooks.touchcancel = {
	props: "clientX clientY pageX pageY screenX screenY".split( " " ),
	filter: function( event, orig ) {
		if ( orig ){
			var touched = ( orig.touches && orig.touches[0] )
				|| ( orig.changedTouches && orig.changedTouches[0] )
				|| null; 
			// iOS webkit: touchstart, touchmove, touchend
			if ( touched ) 
				$.each( touchHooks.props, function( i, prop ){
					event[ prop ] = touched[ prop ];
				});
		}
		return event;
	}
};

// share the same special event configuration with related events...
$special.draginit = $special.dragstart = $special.dragend = drag;

})( jQuery );
/**
 * @name	jQuery.touchSlider
 * @version	201209_2
 * @since	201106
 * @param Object	settings	환경변수 오브젝트
 *		roll			-	순환 (default true)
 *		flexible		-	유동 레이아웃 (default false)
 *		view			-	다중 컬럼 (default 1)
 *		speed			-	애니메이션 속도 (default 75)
 *		range			-	넘김 판정 범위 (default 0.15)
 *		page			-	초기 페이지 (default 1)
 *		transition		-	CSS3 transition 사용 (default false)
 *		btn_prev		-	prev 버튼 (jQuery Object, default null)
 *		btn_next		-	next 버튼 (jQuery Object, default null)
 *		paging			-	page 버튼 (jQuery Object, default null)
 *		initComplete	-	초기화 콜백
 *		counter			-	슬라이드 콜백, 카운터
 *
 * @example
	$("#target").touchSlider({
		flexible : true
	});
*/

(function ($) {
	
	$.fn.touchSlider = function (settings) {
		
		settings.supportsCssTransitions = (function (style) {
			var prefixes = ['Webkit','Moz','Ms'];
			for(var i=0, l=prefixes.length; i < l; i++ ) {
				if( typeof style[prefixes[i] + 'Transition'] !== 'undefined') {
					return true;
				}
			}
			return false;
		})(document.createElement('div').style);
		
		settings = jQuery.extend({
			roll : true,
			flexible : false,
			btn_prev : null,
			btn_next : null,
			paging : null,
			speed : 75,
			view : 1,
			range : 0.15,
			page : 1,
			transition : false,
			initComplete : null,
			counter : null,
			multi : false
		}, settings);
		
		var opts = [];
		opts = $.extend({}, $.fn.touchSlider.defaults, settings);
		
		return this.each(function () {
			
			$.fn.extend(this, touchSlider);
			
			var _this = this;
			
			this.opts = opts;
			this._view = this.opts.view;
			this._speed = this.opts.speed;
			this._tg = $(this);
			this._list = this._tg.children().children();
			this._width = parseInt(this._tg.css("width"));
			this._item_w = parseInt(this._list.css("width"));
			this._len = this._list.length;
			this._range = this.opts.range * this._width;
			this._pos = [];
			this._start = [];
			this._startX = 0;
			this._startY = 0;
			this._left = 0;
			this._top = 0;
			this._drag = false;
			this._scroll = false;
			this._btn_prev;
			this._btn_next;
			
			this.init();
			
			$(this)
			.bind("touchstart", this.touchstart)
			.bind("touchmove", this.touchmove)
			.bind("touchend", this.touchend)
			.bind("dragstart", this.touchstart)
			.bind("drag", this.touchmove)
			.bind("dragend", this.touchend)
			
			$(window).bind("orientationchange resize", function () {
				_this.resize(_this);
			});
		});
	
	};
	
	var touchSlider = {
		
		init : function () {
			var _this = this;
			
			$(this).children().css({
				"width":this._width + "px",
				"overflow":"visible"
			});
			
			if(this.opts.flexible) this._item_w = this._width / this._view;
			if(this.opts.roll) this._len = Math.ceil(this._len / this._view) * this._view;
			
			var page_gap = (this.opts.page > 1 && this.opts.page <= this._len) ? (this.opts.page - 1) * this._item_w : 0;
			
			for(var i=0; i<this._len; ++i) {
				this._pos[i] = this._item_w * i - page_gap;
				this._start[i] = this._pos[i];
				this._list.eq(i).css({
					"float" : "none",
					"display" : "block",
					"position" : "absolute",
					"top" : "0",
					"left" : this._pos[i] + "px",
					"width" : this._item_w + "px"
				});
				if(this.opts.supportsCssTransitions && this.opts.transition) {
					this._list.eq(i).css({
						"-moz-transition" : "0ms",
						"-moz-transform" : "",
						"-ms-transition" : "0ms",
						"-ms-transform" : "",
						"-webkit-transition" : "0ms",
						"-webkit-transform" : "",
						"transition" : "0ms",
						"transform" : ""
					});
				}
			}
			
			if(this.opts.btn_prev && this.opts.btn_next) {
				this.opts.btn_prev.bind("click", function() {
					_this.animate(1, true);
					return false;
				})
				this.opts.btn_next.bind("click", function() {
					_this.animate(-1, true);
					return false;
				});
			}
			
			if(this.opts.paging) {
				$(this._list).each(function (i, el) {
					//var btn_page = _this.opts.paging.eq(0).clone();
					var btn_page = _this.opts.paging.eq(i);
					//_this.opts.paging.before(btn_page);
					
					btn_page.bind("click", function(e) {
						_this.go_page(i, e);
						return false;
					});
				});
				
				//this.opts.paging.remove();
			}
			
			this.counter();
			this.initComplete();
		},
		
		initComplete : function () {
			if(typeof(this.opts.initComplete) == "function") {
				this.opts.initComplete(this);
			}
		},
		
		resize : function (e) {
			if(e.opts.flexible) {
				var tmp_w = e._item_w;
				
				e._width = parseInt(e._tg.css("width"));
				e._item_w = e._width / e._view;
				e._range = e.opts.range * e._width;
				
				for(var i=0; i<e._len; ++i) {
					e._pos[i] = e._pos[i] / tmp_w * e._item_w;
					e._start[i] = e._start[i] / tmp_w * e._item_w;
					e._list.eq(i).css({
						"left" : e._pos[i] + "px",
						"width" : e._item_w + "px"
					});
					if(this.opts.supportsCssTransitions && this.opts.transition) {
						e._list.eq(i).css({
							"-moz-transition" : "0ms",
							"-moz-transform" : "",
							"-ms-transition" : "0ms",
							"-ms-transform" : "",
							"-webkit-transition" : "0ms",
							"-webkit-transform" : "",
							"transition" : "0ms",
							"transform" : ""
						});
					}
				}
			}
			
			this.counter();
		},
		
		touchstart : function (e) { 
			if((e.type == "touchstart" && e.originalEvent.touches.length <= 1) || e.type == "dragstart") {
				this._startX = e.pageX || e.originalEvent.touches[0].pageX;
				this._startY = e.pageY || e.originalEvent.touches[0].pageY;
				this._scroll = false;
				
				this._start = [];
				for(var i=0; i<this._len; ++i) {
					this._start[i] = this._pos[i];
				}
			}
		},
		
		touchmove : function (e) { 
			if((e.type == "touchmove" && e.originalEvent.touches.length <= 1) || e.type == "drag") {
				this._left = (e.pageX || e.originalEvent.touches[0].pageX) - this._startX;
				this._top = (e.pageY || e.originalEvent.touches[0].pageY) - this._startY;
				var w = this._left < 0 ? this._left * -1 : this._left;
				var h = this._top < 0 ? this._top * -1 : this._top;
				
				if (w < h || this._scroll) {
					this._left = 0;
					this._drag = false;
					this._scroll = true;
				} else {
					e.preventDefault();
					this._drag = true;
					this._scroll = false;
					this.position(e);
				}
				
				for(var i=0; i<this._len; ++i) {
					var tmp = this._start[i] + this._left;
					
					if(this.opts.supportsCssTransitions && this.opts.transition) {
						var trans = "translate3d(" + tmp + "px,0,0)";
						this._list.eq(i).css({
							"left" : "",
							"-moz-transition" : "0ms",
							"-moz-transform" : trans,
							"-ms-transition" : "0ms",
							"-ms-transform" : trans,
							"-webkit-transition" : "0ms",
							"-webkit-transform" : trans,
							"transition" : "0ms",
							"transform" : trans
						});
					} else {
						this._list.eq(i).css("left", tmp + "px");
					}
					
					this._pos[i] = tmp;
				}
			}
		},
		
		touchend : function (e) {
			if((e.type == "touchend" && e.originalEvent.touches.length <= 1) || e.type == "dragend") {
				if(this._scroll) {
					this._drag = false;
					this._scroll = false;
					return false;
				}
				
				this.animate(this.direction());
				this._drag = false;
				this._scroll = false;
			}
		},
		
		position : function (d) { 
			var gap = this._view * this._item_w;
			
			if(d == -1 || d == 1) {
				this._startX = 0;
				this._start = [];
				for(var i=0; i<this._len; ++i) {
					this._start[i] = this._pos[i];
				}
				this._left = d * gap;
			} else {
				if(this._left > gap) this._left = gap;
				if(this._left < - gap) this._left = - gap;
			}
			
			if(this.opts.roll) {
				var tmp_pos = [];
				for(var i=0; i<this._len; ++i) {
					tmp_pos[i] = this._pos[i];
				}
				tmp_pos.sort(function(a,b){return a-b;});

				
				var max_chk = tmp_pos[this._len-this._view];
				var p_min = $.inArray(tmp_pos[0], this._pos);
				var p_max = $.inArray(max_chk, this._pos);

				
				if(this._view <= 1) max_chk = this._len - 1;
				if(this.opts.multi) {
					if((d == 1 && tmp_pos[0] >= 0) || (this._drag && tmp_pos[0] >= 100)) {
						for(var i=0; i<this._view; ++i, ++p_min, ++p_max) {
							this._start[p_max] = this._start[p_min] - gap;
							this._list.eq(p_max).css("left", this._start[p_max] + "px");
						}
					} else if((d == -1 && tmp_pos[0] <= 0) || (this._drag && tmp_pos[0] <= -100)) {
						for(var i=0; i<this._view; ++i, ++p_min, ++p_max) {
							this._start[p_min] = this._start[p_max] + gap;
							this._list.eq(p_min).css("left", this._start[p_min] + "px");
						}
					}
				} else {
					if((d == 1 && tmp_pos[0] >= 0) || (this._drag && tmp_pos[0] > 0)) {
						for(var i=0; i<this._view; ++i, ++p_min, ++p_max) {
							this._start[p_max] = this._start[p_min] - gap;
							this._list.eq(p_max).css("left", this._start[p_max] + "px");
						}
					} else if((d == -1 && tmp_pos[max_chk] <= 0) || (this._drag && tmp_pos[max_chk] <= 0)) {
						for(var i=0; i<this._view; ++i, ++p_min, ++p_max) {
							this._start[p_min] = this._start[p_max] + gap;
							this._list.eq(p_min).css("left", this._start[p_min] + "px");
						}
					}
				}
			} else {
				if(this.limit_chk()) this._left = this._left / 2;
			}
		},
		
		animate : function (d, btn_click) {
			if(this._drag || !this._scroll || btn_click) {
				var _this = this;
				var speed = this._speed;
				
				if(btn_click) this.position(d);
				
				var gap = d * (this._item_w * this._view);
				if(this._left == 0 || (!this.opts.roll && this.limit_chk()) ) gap = 0;
				
				this._list.each(function (i, el) {
					_this._pos[i] = _this._start[i] + gap;
					
					if(_this.opts.supportsCssTransitions && _this.opts.transition) {
						var transition = speed + "ms";
						var transform = "translate3d(" + _this._pos[i] + "px,0,0)";
						
						if(btn_click) transition = "0ms";
						
						$(this).css({
							"left" : "",
							"-moz-transition" : transition,
							"-moz-transform" : transform,
							"-ms-transition" : transition,
							"-ms-transform" : transform,
							"-webkit-transition" : transition,
							"-webkit-transform" : transform,
							"transition" : transition,
							"transform" : transform
						});
					} else {
						$(this).stop();
						$(this).animate({"left": _this._pos[i] + "px"}, speed);
					}
				});			
				
				this.counter();
			}
		},
		
		direction : function () { 
			var r = 0;
		
			if(this._left < -(this._range)) r = -1;
			else if(this._left > this._range) r = 1;
			
			if(!this._drag || this._scroll) r = 0;
			
			return r;
		},
		
		limit_chk : function () {
			var last_p = parseInt((this._len - 1) / this._view) * this._view;
			return ( (this._start[0] == 0 && this._left > 0) || (this._start[last_p] == 0 && this._left < 0) );
		},
		
		go_page : function (i, e) {
			var crt = ($.inArray(0, this._pos) / this._view) + 1;
			var cal = crt - (i + 1);
			
			while(cal != 0) {
				if(cal < 0) {
					this.animate(-1, true);
					cal++;
				} else if(cal > 0) {
					this.animate(1, true);
					cal--;
				}
			}
		},
		
		counter : function () {
			if(typeof(this.opts.counter) == "function") {
				var param = {
					total : Math.ceil(this._len / this._view),
					current : ($.inArray(0, this._pos) / this._view) + 1
				};
				this.opts.counter(param);
			}
		}
		
	};

})(jQuery);
/*
 * @Author: wangyinhua
 * @Date:   2016-11-17 11:05:49
 * @Last Modified by:   wangyinhua
 * @Last Modified time: 2016-12-12 19:12:11
 */

'use strict';

var f7 = {};

f7.popup = {};
f7.plugin = {}; //插件
f7.hack = {}; //补丁
f7.command = {}; //命令
f7.module = {}; //模块
f7.validate = {}; //验证
f7.tool = {}; //工具


// 注册 初始化 插件
f7.plugin._register = (function() {
	return {
		header: f7.plugin.header, //页头
		footer: f7.plugin.footer, //页尾
		// fileAsyncUpdate:f7.plugin.fileAsyncUpdate,//判断是否异步上传文件
		// form: f7.plugin.form,				//表单异步提交
		// moduleInit: f7.plugin.moduleInit, 	//子模块初始化
		preview: f7.plugin.preview, // preview 预览
		carousel: f7.plugin.carousel, // 旋转木马
	};
}); //f7.module._register()

f7.hack._register = (function() {
	return {};
}); //f7.hack._register()

// 公共模块 初始化
f7.module.initialize = (function() {
	var _plugin = f7.plugin._register(),
		_hack = f7.hack._register();

	//自动初始化 插件
	for (var pluginName in _plugin) {
		if (_plugin.hasOwnProperty(pluginName) && typeof _plugin[pluginName] === 'function') {
			_plugin[pluginName]();
		} //if
	} //for in

	//自动初始化 Hack
	for (var hackName in _hack) {
		if (_hack.hasOwnProperty(hackName) && typeof _hack[hackName] === 'function') {
			_hack[hackName]();
		} //if
	} //for in

}); //f7.module.initialize()

(function($) {

	$(function() {
		var id = $('main').attr('id'),
			bodyId = null,
			module = null;

		if (id) {
			bodyId = id.replace('-', '_');
			module = bodyId.split('_')[0];
			// console.log('body id = ' + bodyId);

			if (module in f7.module) {
				f7.module[module]();
			} //if

			if (bodyId in f7.module) {
				f7.module[bodyId]();
			} //if
		} //if

		f7.module.initialize();
	});

})(jQuery);
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

/*
* @Author: wangyinhua
* @Date:   2016-11-23 10:11:58
* @Last Modified by:   wangyinhua
* @Last Modified time: 2016-11-24 15:46:44
*/

f7.module.Index_index = (function () {

	// (function () {
	// 	if (!document.getElementById('chart-1')) return ;//if

	// 	var myChart = echarts.init(document.getElementById('chart-1'));

	// 	function randomData() {
	// 	    now = new Date(+now + oneDay);
	// 	    value = value + Math.random() * 21 - 10;
	// 	    return {
	// 	        name: now.toString(),
	// 	        value: [
	// 	            [now.getFullYear(), now.getMonth() + 1, now.getDate()].join('/'),
	// 	            Math.round(value)
	// 	        ]
	// 	    }
	// 	}

	// 	var data = [];
	// 	var now = +new Date(1997, 9, 3);
	// 	var oneDay = 24 * 3600 * 1000;
	// 	var value = Math.random() * 1000;
	// 	for (var i = 0; i < 1000; i++) {
	// 	    data.push(randomData());
	// 	}

	// 	var option = {
	// 	    // title: {
	// 	    //     text: '动态数据 + 时间坐标轴'
	// 	    // },
	// 	    tooltip: {
	// 	        trigger: 'axis',
	// 	        formatter: function (params) {
	// 	            params = params[0];
	// 	            var date = new Date(params.name);
	// 	            return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear() + ' : ' + params.value[1];
	// 	        },
	// 	        axisPointer: {
	// 	            animation: false
	// 	        }
	// 	    },
	// 	    xAxis: {
	// 	        type: 'time',
	// 	        splitLine: {
	// 	            show: false
	// 	        }
	// 	    },
	// 	    yAxis: {
	// 	        type: 'value',
	// 	        boundaryGap: [0, '100%'],
	// 	        splitLine: {
	// 	            show: false
	// 	        }
	// 	    },
	// 	    series: [{
	// 	        name: '模拟数据',
	// 	        type: 'line',
	// 	        showSymbol: false,
	// 	        hoverAnimation: false,
	// 	        data: data
	// 	    }]
	// 	};

	// 	myChart.setOption(option);

	// 	var timeTicket = setInterval(function () {

	// 	    for (var i = 0; i < 5; i++) {
	// 	        data.shift();
	// 	        data.push(randomData());
	// 	    }

	// 	    myChart.setOption({
	// 	        series: [{
	// 	            data: data
	// 	        }]
	// 	    });
	// 	}, 1000);
	// }());

	// (function () {
	// 	if (!document.getElementById('chart-2')) return ;//if;

	// 	var myChart = echarts.init(document.getElementById('chart-2'));
	// 	var option = {
	// 	    // title: {
	// 	    //     // text: '堆叠区域图'
	// 	    // },
	// 	    tooltip : {
	// 	        trigger: 'axis'
	// 	    },
	// 	    legend: {
	// 	        data:['实时数据', '平均数据']
	// 	    },
	// 	    toolbox: {
	// 	        feature: {
	// 	            saveAsImage: {}
	// 	        }
	// 	    },
	// 	    grid: {
	// 	        left: '3%',
	// 	        right: '4%',
	// 	        bottom: '3%',
	// 	        containLabel: true
	// 	    },
	// 	    xAxis : [
	// 	        {
	// 	            type : 'category',
	// 	            boundaryGap : false,
	// 	            data : ['周一','周二','周三','周四','周五','周六','周日']
	// 	        }
	// 	    ],
	// 	    yAxis : [
	// 	        {
	// 	            type : 'value'
	// 	        }
	// 	    ],
	// 	    series : [
	// 	        {
	// 	            name:'实时数据',
	// 	            type:'line',
	// 	            stack: '总量',
	// 	            areaStyle: {normal: {}},
	// 	            data:[120, 132, 101, 134, 90, 230, 210]
	// 	        },
	// 	        {
	// 	            name:'平均数据',
	// 	            type:'line',
	// 	            stack: '总量',
	// 	            areaStyle: {normal: {}},
	// 	            data:[220, 182, 191, 234, 290, 330, 310]
	// 	        },
	// 	        // {
	// 	        //     name:'视频广告',
	// 	        //     type:'line',
	// 	        //     stack: '总量',
	// 	        //     areaStyle: {normal: {}},
	// 	        //     data:[150, 232, 201, 154, 190, 330, 410]
	// 	        // },
	// 	        // {
	// 	        //     name:'直接访问',
	// 	        //     type:'line',
	// 	        //     stack: '总量',
	// 	        //     areaStyle: {normal: {}},
	// 	        //     data:[320, 332, 301, 334, 390, 330, 320]
	// 	        // },
	// 	        // {
	// 	        //     name:'搜索引擎',
	// 	        //     type:'line',
	// 	        //     stack: '总量',
	// 	        //     label: {
	// 	        //         normal: {
	// 	        //             show: true,
	// 	        //             position: 'top'
	// 	        //         }
	// 	        //     },
	// 	        //     areaStyle: {normal: {}},
	// 	        //     data:[820, 932, 901, 934, 1290, 1330, 1320]
	// 	        // }
	// 	    ]
	// 	};

	// 	myChart.setOption(option);
	// }());



});//f7.module.Index_index{}



// 登录界面
f7.module.Login_index = (function() {

	//验证码滑动
	$.fn.drag = function(options) {
		var x, drag = this,
			isMove = false,
			defaults = {};
		options = $.extend(defaults, options);

		drag.html('');
		$('.handler').data('done', 0);

		//添加背景，文字，滑块
		var html = '<div class="drag-bg"></div>' +
			'<div class="drag-text" onselectstart="return false;" unselectable="on">拖动滑块验证</div>' +
			'<div class="handler"><i class="fa fa-angle-double-right"></i></div>';
		this.append(html);

		var handler = drag.find('.handler');
		var dragBg = drag.find('.drag-bg');
		var text = drag.find('.drag-text');
		var maxWidth = drag.width() - handler.width(); //能滑动的最大间距

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
					handler.css({
						'left': _x
					});
					dragBg.css({
						'width': _x
					});
					drag.find('.drag-text').text('');
				} else if (_x > maxWidth) { //鼠标指针移动距离达到最大时清空事件
					dragOk();
				}
			} //if
		}).mouseup(function(e) {
			isMove = false;
			var _x = e.pageX - x;
			if (_x < maxWidth) { //鼠标松开时，如果没有达到最大距离位置，滑块就返回初始位置
				handler.css({
					'left': 0
				});
				dragBg.css({
					'width': 0
				});
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

	$('#drag').drag();

	// 界面切换
	(function() {
		var $form = $('.form'),
			$nav = $form.find('ul');

		$nav.on('click', 'a', function(evt) {
			evt.stopPropagation();
			evt.preventDefault();

			$nav.find('a').removeClass('current');
			var $a = $(evt.target),
				id = $a.data('id');

			$a.addClass('current');

			$form.find('form').hide().end().find('#' + id).show();
		});
	}());

	//登录进程
	// (function() {
	// 	var $form = $('#common'),
	// 		$username = $('input[name="username"]', $form),
	// 		$password = $('input[name="password"]', $form),
	// 		$btn = $(':submit', $form),
	// 		validate = f7.validate,
	// 		formError = popup.formError;

	// 	$form.on('focus', 'input:focus', function(evt) {
	// 		formError.init().hide();
	// 	}).on('_formError.custom', function(evt, value) {
	// 		$('#drag').drag();
	// 		$btn.removeClass('btn-disabled');
	// 	}).on('click', ':submit', function(evt) {
	// 		// 登录 提交 禁用
	// 		if ($btn.is('.btn-disabled')) {
	// 			evt.preventDefault();
	// 			evt.stopPropagation();
	// 		} else {
	// 			formError.init();
	// 			if (!formError.show($username, validate.username($username))) return false; //if
	// 			if (!formError.show($password, validate.password($password))) return false; //if
	// 			if ($('.handler').data('done') != 1) {
	// 				popup.info.init().show('请滑动验证', false);
	// 				return false;
	// 			}
	// 			$btn.addClass('btn-disabled');
	// 		} //if
	// 	});
	// }());

}); //Login_index
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



}); //f7.plugin.carousel()
// data-ajax-check="on" 开启验证
// data-ajax-check="off" 关闭验证
f7.plugin.fileAsyncUpdate = (function() {
	// var $forms = $('section form[method="post"][action*="Handle"]'),
	var $forms = $('form[method="post"][action*="Handle"]'),
		$files = $(':file', $forms);

	// console.log('form = ' + $forms.length + ' | ' + 'file = ' + $files.length);

	if ($forms.length && $files.length && typeof FileReader !== 'undefined') {

		$forms.each(function(index, element) {
			var $form = $(element),
				ajaxCheck = $form.data('ajax-check'),
				$files = $(':file', $form);

			if (ajaxCheck != 'off' && $files.length) {
				register($form);
			}//if
		});
	}//if

	// 读取 数据
	function register($form, dataAjax) {
		if (typeof dataAjax === 'undefined') {
			dataAjax = 'on';
		}//if

		// $form.attr('data-ajax', 'on').removeAttr('enctype')
		$form.attr('data-ajax', dataAjax).removeAttr('enctype')
			 .prepend('<input type="hidden" name="_isDataFile" value="1"/>')
			 .on('change', ':file', function(evt) {

			 	var file = this,
					$file = $(this),
					type = $file.data('type'),
					arr = [],
					_temp = null;

				// console.log(type);
				// console.log(file.files);
				// console.log(file.files.length);

				if (file.files.length) {
					for (var i = 0, len = file.files.length; i < len; i++) {
						_temp = readFile(file.files[i], type);
						if ($.isPlainObject(_temp)) {
							arr.push(_temp);
						}//if
					}//for in
				}//if

				if (arr.length == 1) {
					$file.data('data', arr[0]);
				} else if (arr.length > 1) {
					$file.data('data', arr);
				}//if
				
				// console.log(arr);

			 	///////////////老版本 只支持读取单文件//////////////////
				// var file = this,
				// 	$file = $(this),
				// 	type = $file.data('type'),
				// 	reader = new FileReader(),
				// 	data = {};

				// // console.log(type);
				// console.log(file.files);
				// console.log(file.files.length);

				// if (file.files.length) {
				// 	file = file.files[0];
				// 	// console.log(file);

				// 	reader.readAsDataURL(file);
				// 	reader.onload = function(evt) {
				// 		console.info('FileReader onload');

				// 		switch (type) {
				// 			case 'image':
				// 			{
				// 				if (!/^image\/\w+$/.test(file.type)) { 
				// 			        popup.alert.init();
				// 					popup.alert.show('上传 文件 必须为图片！');
				// 			        // return ;
				// 			    }//if
				// 			}
				// 				break;
				// 		}//switch

				// 		data.data = this.result.split(',')[1];
				// 		data.name = file.name;
				// 		data.type = file.type;
				// 		data.size = file.size;

				// 		$file.data('data', data);
				// 	};//reader.onload()

				// 	reader.onerror = function(evt) {
				// 		// console.info('FileReader onerror');
				// 		// console.log(this.error);
						
				// 		popup.alert.init();
				// 		popup.alert.show('文件读取错误！');
				// 	};//reader.onerror()
				// }//if
		});
	}//register()

	function readFile(file, type) {
		// file = file.files[0];
		// console.log(file);
		var reader = new FileReader(),
			data = {};

		reader.readAsDataURL(file);
		reader.onload = function(evt) {
			// console.info('FileReader onload');

			switch (type) {
				case 'image':
				{
					if (!/^image\/\w+$/.test(file.type)) { 
				        popup.alert.init();
						popup.alert.show('上传 文件 必须为图片！');
				        // return ;
				    }//if
				}
					break;
			}//switch

			data.data = this.result.split(',')[1];
			data.name = file.name;
			data.type = file.type;
			data.size = file.size;

			// $file.data('data', data);
			return data;
		};//reader.onload()

		reader.onerror = function(evt) {
			// console.info('FileReader onerror');
			// console.log(this.error);
			
			popup.alert.init();
			popup.alert.show('文件读取错误！');
		};//reader.onerror()

		return data;
	}//readFile()

	f7.plugin.fileAsyncUpdate.register = register;
	
});//f7.plugin.fileAsyncUpdate();

// footer 部分
f7.plugin.footer = (function() {
	// console.log('plugin.footer');
	// var screenHeight = $(window).height(),
	// 	height = $('body').height();

	// // console.log(screenHeight, height);
	// if (height < screenHeight) {
	// 	var $main = $('main');
	// 	$main.css({
	// 		height: $main.height() + (screenHeight - height) + 'px',
	// 	});
	// }//if

	// popup.info.init().show('看一下效果');
	// popup.info.init().show('看一下效果', false);
	// popup.alert.init().show('看一下Alert 警告框');
	// setTimeout(function () {
	// 	// popup.toolTip.init().show($('input[name="keyword"]'), '这是测试内容');
	// 	// popup.formError.init().show($('input[name="keyword"]'), {status: false, info: '搜索内容不能为空！'});
	// }, 1000);

});//f7.plugin.footer()



// 表单异步提交
f7.plugin.form = (function f7_plugin_form() {
	var $form = $('form'),
		$html = $('html'),
		data = {},	//要提交的数据
		tagData = 'bindSubmit';//用于判断是否绑定过提交事件
	
	if (!$form.length || $html.data(tagData)) return ;//if

	// console.log('cs.form');
	if (!$html.data(tagData)) {
		$html.data(tagData, true);
	}//if

	popup.loading.init();
	// $html.on('submit', '[method="post"][action$="Handle"]', function(evt) {
	// $html.on('submit', '[method="post"][action*="Handle"],[method="post"][action*="/payment"]', function(evt) {
	// $html.on('submit', '[method="post"][action*="Handle"],[method="post"][action*="/create"]', function(evt) {
	$html.on('submit', 'form[method="post"][action*="Handle"],form[method="post"][data-force-async="on"]', function(evt) {
		// console.info('submit');
		// return false;
		var $target = $(evt.target),
			ajax = $target.data('ajax'),
			$arr = $target.find('input[name!=""],select[name!=""],textarea[name!=""]'),
			url = $target.attr('action');
			// data = {};

		if (ajax == 'off') return ;//if
		//初始化 data 数据
		data = {};

		//
		evt.stopPropagation();
		evt.preventDefault();
		
		if (!$target.data('is-post-data')) {
			// 不存在 提交数据时 进行 收集表单数据 准备提交
			$arr.each(function(index, element) {
				var $i = $(this),
					name = $i.attr('name'),
					value = $i.val(),
					regexp = '',
					_tempName = '',
					_temp = null;
				
				if (!name) return true;//if
				
				// if($i.is(':checkbox') && $i.prop('checked')) {
				// 	if(!(name in data)) data[name] = [];//if
				// 	data[name].push(value);
				// }else if($i.is(':radio') && $i.prop('checked')) {
				// 	data[name] = value;
				// }else if(!$i.is(':radio') && !$i.is(':checkbox')) {
				// 	if(/\[\]$/.test(name)) {
				// 		arrName = name.replace(/\[\]$/,'');
						
				// 		if(!(arrName in data)) {
				// 			data[arrName] = [];
				// 		}//if
						
				// 		data[arrName].push(value);
				// 	}else{
				// 		data[name] = value;
				// 	}//if
				// }//if

				if ($i.is(':checkbox:checked')) {
					if (!(name in data)) data[name] = [];//if
					data[name].push(value);
				} else if ($i.is(':radio:checked')) {
					// data[name] = value;
					regexp = /\[(.+)\]$/;

					if (regexp.test(name)) {
						_tempName = name.replace(regexp,'');
						_temp = name.match(regexp);

						if (!(_tempName in data)) {
							data[_tempName] = {};
						}//if
						
						data[_tempName][_temp[1]] = value;
					} else {
						data[name] = value;
					}//if
					
				} else if (!$i.is(':radio') && !$i.is(':checkbox') && !$i.is(':file')) {
					regexp = /\[\]$/;

					if (regexp.test(name)) {
						_tempName = name.replace(regexp,'');
						
						if (!(_tempName in data)) {
							data[_tempName] = [];
						}//if
						
						data[_tempName].push(value);
					} else {
						data[name] = value;
					}//if
				} else if ($i.is(':file')) {
					(function() {
						var $file = $i,
							type = $i.data('type'),
							_fileDatas = $file.data('data'),
							_fileData = null;
							// _fileData = $file.data('data');

						// console.log('----- form -----');
						// console.log(_fileData);

						// if (_fileData && $.isPlainObject(_fileData)) {
						if (_fileDatas && $.isArray(_fileDatas) && _fileDatas.length) {
							for (var index in _fileDatas) {
								_fileData = _fileDatas[index];

								switch (type) {
									case 'image':
									{
										if (!/^image\/\w+$/.test(_fileData.type)) { 
											popup.alert.init().show('上传 文件 必须为图片！');
											return ; 
										}//if
									}
										break;
								}//switch
							}//for in
						}//if

						data[name] = _fileDatas;
						// data[name] = _fileData;

					}());
				}//if
			});

			// console.log(data);

		} else {
			// 从目标 form 上数据已存储的数据
			data = $target.data('post-data');
		}//if

		// console.log(url);
		// console.dir(data);
		// return false;
		
		popup.loading.init();
		$.post(url, data, function(obj) {
			// console.dir(obj);
			// return false;

			var regexp = /^<!DOCTYPE html>/i,
				form = f7.plugin.form;

			if (regexp.test(obj)) {
				// console.log(obj.match(obj));
				// obj 为收到的数据 data 为提交的数据
				form.disposeHtml(obj, data);
			} else {
				// obj 为收到的数据 
				form.disposeJSON(obj);
			}//if

		});
	});
	
});//f7.plugin.form()

// 处理返回html
f7.plugin.form.disposeHtml = (function(content, data) {
	// data 原始提交数据 方便处理
	var panel = popup.panel,
		regexp = /(<main[\s\S]+?>[\s\S]+?<\/main>)/gim,
		arr = content.match(regexp),
		$main = arr ? $(arr[0]) : $(content),
		htmlClose = '<a class="btn-close-x" href="#" title="关闭"></a>';
	
	$main.append(htmlClose);

	// console.dir(arr);

	panel.init().show($main);

	// data-async-update="true" 存在导步更新标识
	if ($main.data('async-update')) {
		// console.log('abc');
		$('html').trigger('_asyncUpdate.custom', data);

	} else if ($main.data('script-init')) {
		// 脚本初始化
		// 初始化 模块
		(function() {
			var id = $main.attr('id').replace('-', '_'),
				module = f7.module;

			id = id.charAt(0).toUpperCase() + id.slice(1);
			// console.log(id);

			if (id in module) {
				// console.info('存在 '+ id);
				module[id]();
			} else {
				console.info('不存在 '+ id);
			}//if
		}());
	}//if
});//disposeHtml()

// 处理返回json
f7.plugin.form.disposeJSON = (function(obj) {
	if ($.isPlainObject(obj)) {
		if ('_type' in obj && obj._type == 'field' && !obj._status) {
			(function() {
				var $input = $('input[name="' + obj._field + '"]'),
					formError = popup.formError;

				if ($input.length) {
					formError.init().show($input, formError.data(obj));
				} else {
					popup.alert.init().show('Error description is not exist !');
				}//if

				$input.parents('form').triggerHandler('_formError.custom', obj);
			}());
		} else if (obj.status) {
			popup.info.init().show(obj.info);
			
			setTimeout(function() {
				location.href = obj.url;
			}, 1500);
		} else {
			popup.alert.init().show(obj.info);
		}//if
	} else {
		// 调试 不能识别的数据
		// popup.panel.init().show(obj);
		popup.panel.init().show(obj + '<a class="btn-close-x" href="#" title="关闭"></a>');
	}//if
});//disposeJSON()



// header 部分
f7.plugin.header = (function() {



});//f7.plugin.header()


// 子模块 初始化
// a[data-module-init="on"] 在需要保存 当前 所在 a 标签中的 form 中的数据 get 到 指定 链接 用 [data-save-data="on"]
f7.plugin.moduleInit = (function() {
	var $html = $('html'),
		$moduleInit = $('a[data-module-init="on"]', $html);

	if (!$moduleInit.length) {
		return ;
	}//if

	$html.on('click', 'a[data-module-init="on"]', function(evt) {
		// evt.stopPropagation();
		evt.preventDefault();

		var $a = $(this),
			url = $a.attr('href'),
			saveData = {};

		if ($a.data('save-data') == 'on') {
			// console.log($a.data('save-data'));
			saveData = (function () {
				var $arr = $a.parents('form').find('input[name!=""],select[name!=""],textarea[name!=""]'),
					data = {};

				// console.log($arr.length);
				$arr.each(function(index, element) {
					var $i = $(this),
						name = $i.attr('name'),
						value = $i.val(),
						regexp = '',
						_tempName = '',
						_temp = null;
					
					if (!name) return true;//if

					if ($i.is(':checkbox:checked')) {
						if (!(name in data)) data[name] = [];//if
						data[name].push(value);
					} else if ($i.is(':radio:checked')) {
						// data[name] = value;
						regexp = /\[(.+)\]$/;

						if (regexp.test(name)) {
							_tempName = name.replace(regexp,'');
							_temp = name.match(regexp);

							if (!(_tempName in data)) {
								data[_tempName] = {};
							}//if
							
							data[_tempName][_temp[1]] = value;
						} else {
							data[name] = value;
						}//if
						
					} else if (!$i.is(':radio') && !$i.is(':checkbox') && !$i.is(':file')) {
						regexp = /\[\]$/;

						if (regexp.test(name)) {
							_tempName = name.replace(regexp,'');
							
							if (!(_tempName in data)) {
								data[_tempName] = [];
							}//if
							
							data[_tempName].push(value);
						} else {
							data[name] = value;
						}//if
					} else if ($i.is(':file')) {
						(function() {
							var $file = $i,
								type = $i.data('type'),
								_fileDatas = $file.data('data'),
								_fileData = null;
								// _fileData = $file.data('data');

							// console.log('----- form -----');
							// console.log(_fileData);

							// if (_fileData && $.isPlainObject(_fileData)) {
							if (_fileData && $.isArray(_fileData) && _fileData.length) {
								for (var index in _fileDatas) {
									_fileData = _fileDatas[index];

									switch (type) {
										case 'image':
										{
											if (!/^image\/\w+$/.test(_fileData.type)) { 
												popup.alert.init().show('上传 文件 必须为图片！');
												return ; 
											}//if
										}
											break;
									}//switch
									
								}//for in
							}//if

							data[name] = _fileDatas;
							// data[name] = _fileData;

						}());
					}//if
				});

				return data;
			}());

			// console.log(saveData);
		}//if

		popup.loading.init();
		$.get(url, saveData, function(content) {
			if ($.isPlainObject(content)) {
				// console.dir(content);
				popup.alert.init().show(content.info);
			} else {
				var panel = popup.panel,
					regexp = /(<main[\s\S]+?>[\s\S]+?<\/main>)/gim,
					arr = content.match(regexp),
					$main = arr ? $(arr[0]) : $(content),
					htmlClose = '<a class="btn-close-x" href="#" title="关闭"></a>';
				
				// console.dir(arr);
				$main.append(htmlClose);

				panel.init().show($main);

				// 
				if (arr && arr.length) {
					// console.log($main);

					// 初始化 模块
					(function() {
						var id = $main.attr('id').replace('-', '_'),
							module = f7.module;

						id = id.charAt(0).toUpperCase() + id.slice(1);
						// console.log(id);

						if (id in module) {
							// console.info('存在 '+ id);
							$('html').off('_asyncUpdate.custom');
							
							module[id]();
							f7.plugin.form();
						} else {
							// console.info('不存在 '+ id);
						}//if

					}());
				}//if
				
			}//if
		});//$.get()

	});

});//f7.plugin.moduleInit()
// preview 预览
// data-preview="true"
f7.plugin.preview = (function() {
	var $previews = $('a[data-preview="true"]');

	if(!$previews.length) return ;//if

	var panel = popup.panel,
		$main = $('main');

 	$main.on('click', 'a[data-preview="true"]', function(evt) {
 		evt.preventDefault();
 		evt.stopPropagation();

		var $target = $(this),
			url = $target.attr('href'),
			previewType = $target.data('preview-type'),
			html = '';

		// console.info(url);
		// console.info(previewType);

		switch (previewType) {
			case 'image':
			default:
				html = ['<div class="preview">',
							'<a class="btn-close-x" href="#" title="关闭"></a>',
							'<img class="preview-image" src="'+url+'" alt="预览" title="预览"/>',
						'</div>'];
				break;
		}//switch

		panel.init().show(html.join(''));
		
		// $('.preview-image').on('load', function(evt) {
		// 	console.log(evt.type);
		// });
 	});

});//f7.plugin.preview();

// alert
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

	var _tagHide, _redirectUrl, _intervalTag, _times;
	//显示
	function show(content, isSuccess, redirectUrl) {
		if (typeof isSuccess != 'boolean') isSuccess = true; //if
		_redirectUrl = redirectUrl;
		_times = f7.popup.info.config.hideTime / 1000;
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
			//setContent('正在发送数据！');
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
			popup.alert.init();
			popup.alert.show('请求不可达！请稍后再试！');
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



// 数据存储
f7.storage = {
	getModuleName:function() {
		return $('body').attr('id');
	},//getModuleName()
	getData:function(moduleName) {
		if (!moduleName) {
			moduleName = this.getModuleName();
		}//if

		return JSON.parse(localStorage.getItem(moduleName)) || {};
	},//getData()
	setData:function(data, moduleName) {
		if (!moduleName) {
			moduleName = this.getModuleName();
		}//if

		localStorage.setItem(moduleName, JSON.stringify(data));

		return data;
	},//setData()
	removeData:function(moduleName) {
		if (!moduleName) {
			moduleName = this.getModuleName();
		}//if

		var data = {};
		localStorage.setItem(moduleName, JSON.stringify(data));

		return data;
	},//removeData()
	getItem:function(key, moduleName) {
		var data = this.getData(moduleName);

		return data[key];
	},//getItem()
	setItem:function(key, value, moduleName) {
		var data = this.getData(moduleName);

		data[key] = value;
		this.setData(data, moduleName);

		return data;
	},//setItem()
	removeItem:function(key, moduleName) {
		var data = this.getData(moduleName);

		delete data[key];
		this.setData(data, moduleName);

		return data;
	},//removeItem()
	clear:function() {
		localStorage.clear();
	}//clear()
};//f7.storage{}


/**
 * 受保护数据
 * @param  string  $value 原始数据
 * @param  integer $first 保留前几个字符
 * @param  integer $last  保留后面几个字符
 * @return string         返回处理过的字符
 */
f7.tool.safeguardData = (function(value, first, last) {
	value = $.trim(value);
	first = first ? first : 3;
	last = last ? last : 4;

	var len = value.length,
		maskLen = len - first - last,
		pad = '';

	for (var i = 0; i < maskLen; i++) {
		pad += '*';
	}//for

	return value.substr(0, first) + pad + value.substr(-last);
});//f7.tool.safeguardData()

//金额格式化
f7.tool.formatMoney = (function(value, length) {
	value = value.toFixed(length ? length : 0);

	var regexp = /(-?\d+)(\d{3})/;
    while (regexp.test(value)) {
    	value = value.replace(regexp, '$1,$2');
    }//while

    return value;
});//f7.tool.formatMoney()


f7.tool.getUrlParam = (function(name) {
	var regexp = new RegExp("(^|&)"+name+"=([^&]*)(&|$)"),
		result = window.location.search.substr(1).match(regexp);

	if(result) {
		return unescape(result[2]);
	} else {
		return null;
	}//if
});//f7.tool.getUrlParam()

f7.tool.getUrlFragment = (function(name) {
	var regexp = new RegExp("(^|#)"+name+"=([^&#]*)(#|$)"),
		result = window.location.hash.substr(1).match(regexp);

	if(result) {
		return unescape(result[2]);
	} else {
		return null;
	}//if

});//f7.tool.getUrlFragment()

f7.tool.formatTel = (function(value) {
	value += '';
	
	return value.replace(/^(\d{3})(\d{4})(\d{4})$/, '$1 $2 $3');
});//f7.tool.formatTel()

f7.tool.formatQQ = (function(value) {
	value += '';

	return value.replace(/^(\d{4})(\d+)$/, '$1 $2');
});//f7.tool.formatQQ()

f7.tool.formatWechat = (function(value) {
	value += '';

	return value.replace(/^(\d{3})(\d{4})(\d+)$/, '$1 $2 $3');
});//f7.tool.formatWechat()



// 标准输出
f7.validate.output = (function(info, status) {
	return {
		status: status ? status : false,
		info: info
	};
});//f7.validate.output()

// 验证 手机号 与 邮箱地址
f7.validate.telAndEmail = (function($input) {
	var value = $.trim($input.val());

	if (value.indexOf('@') == -1) {
		return this.tel($input);
	} else {
		return this.email($input);
	}//if
});

// 判断 并返回 username 填入内容类型 手机号 或 邮箱
f7.validate.isUsernameType = (function($input) {
	var value = $.trim($input.val());

	if (value.indexOf('@') == -1) {
		return 'tel';
	} else {
		return 'email';
	}//if
});

/////////////////////////////////////////////////////////////////////////////////////////////

// 充值金额 验证
f7.validate.rechargeMoney = (function($input, limit) {
	var value = parseInt($.trim($input.val()), 10);

	limit = parseInt(limit);

	if (!parseInt(value)) {
		return this.output('请输入 充值金额！');
	} else if (!/^[0-9]+$/.test(value)) {
		return this.output('充值金额 输入错误！');
	} else if (limit > 0 && value > limit) {
		return output('本次最多可充值：' + limit + ' 元！');
	}//if

	return this.output('', true);
});//f7.validate.rechargeMoney()


// 提现金额 验证
f7.validate.withdrawMoney = (function($input, limit) {
	var value = parseInt($.trim($input.val()), 10);

	limit = parseInt(limit);

	if (!value) {
		return this.output('请输入 提现金额！');
	} else if (!/^[0-9]+$/.test(value)) {
		return this.output('提现金额 输入错误！');
	} else if (limit > 0 && value > limit) {
		return this.output('本次最多可提取：' + limit + ' 元！');
	}//if

	return this.output('', true);
});//f7.validate.withdrawMoney()

/////////////////////////////////////////////////////////////////////////////////////////////

// 绑定银行卡 开户行
f7.validate.bankId = (function($input) {
	var value = $.trim($input.val());

	if (!value) {
		return this.output('请选择 开户银行！');
	}//if

	return this.output('', true);
});//f7.validate.bankId()

// 银行卡号
f7.validate.bankCardNo = (function($input) {
	var value = $.trim($input.val()),
		strBin = '10,18,30,35,37,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,58,60,62,65,68,69,84,87,88,94,95,98,99',
		first15Num = value.substr(0, value.length - 1),
		lastNum = value.substr(value.length - 1, 1),
		newArr = [],
		arrJiShu = [],
   		arrJiShu2 = [],
    	arrOuShu = [];  //偶数位数组

	if (value.length < 16 || value.length > 19) {
		return this.output('银行卡号 长度必须在16或19位！');
	}//if

	if (!(/^\d{16}|\d{19}$/).exec(value)) {
		return this.output('银行卡号 必须为数字!');
	}//if

	//开头6位
	if (strBin.indexOf(value.substring(0, 2)) === -1) {
		return this.output('银行卡号 格式错误！');
	}//if

    for (var i = first15Num.length - 1; i > -1; i--) {
        newArr.push(first15Num.substr(i, 1));
    }//for

    for (var j = 0; j < newArr.length; j++) {
        if ((j + 1) % 2 === 1) {
        	//奇数位
            if (parseInt(newArr[j]) * 2 < 9) {
            	arrJiShu.push(parseInt(newArr[j]) * 2);
            } else {
            	arrJiShu2.push(parseInt(newArr[j]) * 2);
        	}//if
        } else {
        	//偶数位
        	arrOuShu.push(newArr[j]);
        }//if
    }//for
    
    var jishu_child1 = [],//奇数位*2 >9 的分割之后的数组个位数
    	jishu_child2 = [];//奇数位*2 >9 的分割之后的数组十位数
    for (var h = 0; h < arrJiShu2.length; h++) {
        jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
        jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
    }//for     
    
    var sumJiShu = 0, //奇数位*2 < 9 的数组之和
    	sumOuShu = 0, //偶数位数组之和
    	sumJiShuChild1 = 0, //奇数位*2 >9 的分割之后的数组个位数之和
    	sumJiShuChild2 = 0, //奇数位*2 >9 的分割之后的数组十位数之和
    	sumTotal = 0;
    for (var m = 0; m < arrJiShu.length; m++) {
        sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
    }//for
    
    for (var n = 0; n < arrOuShu.length; n++) {
        sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
    }//for
    
    for (var p = 0; p < jishu_child1.length; p++) {
        sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
        sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
    }//for

    //计算总和
    sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);
    
    //计算Luhm值
    var k = parseInt(sumTotal) % 10 === 0 ? 10 : parseInt(sumTotal) % 10,   //if     
    	luhm = 10 - k;
    
    if (lastNum != luhm) {
    	return this.output('银行卡号 错误！');
    }//if

    return this.output('', true);
});//f7.validate.bankCardNo()


/////////////////////////////////////////////////////////////////////////////////////////////

f7.validate.questionTypeId = (function($input) {
	var value = $.trim($input.val());

	if (!value) {
		return this.output('请选择 类型！');
	}//if

	return this.output('', true);
});//f7.validate.questionTypeId()

f7.validate.questionTitle = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('标题 不能为空！');
	} else if (len < 6) {
		return this.output('标题 不能小于6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.questionTitle()

f7.validate.questionContent = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('内容 不能为空！');
	} else if (len < 16) {
		return this.output('内容 不能小于16个字符！');
	}//if

	return this.output('', true);
});//f7.validate.questionContent()

/////////////////////////////////////////////////////////////////////////////////////////////

f7.validate.futureCustomQuantity = (function($input, maxQuantity) {
	var value = $.trim($input.val()),
		regexp = /^[0-9]+$/;

	if (!regexp.test(value)) {
		return this.output('交易数量 必须为数字！');
	} else if (value > maxQuantity) {
	 	return this.output('交易数量 不能大于' + maxQuantity + '手！');
	} else if (value <= 0) {
		return this.output('交易数量 不能小于1手！');
	}//if

	return this.output('', true);
});//f7.validate.futureCustomQuantity()

f7.validate.futureDeposit = (function($input, perMoney, perRatio, perDeposit, currentQuantity) {
	var value = $.trim($input.val()).replace(/,/g, ''),
		regexp = /^[0-9]+$/,
		formatMoney = f7.tool.formatMoney;

	if (value === '') {
		// return this.output('保证金 不能为空！');
		return this.output('请输入 保证金！');
	} else if (!regexp.test(value)) {
		return this.output('保证金 输入错误！');
	} else if (value > perMoney * currentQuantity) {
		return this.output('保证金 不能大于 总投资金额！');
	} else if (value < perDeposit * currentQuantity) {
		return this.output('保证金 不能低于 '+ formatMoney(perDeposit * currentQuantity) +'元');
	} else if (value % 100 !== 0) {
		return this.output('保证金 必须为100元的整数倍！');
	}//if

	return this.output('', true);
});//f7.validate.futureDeposit()

f7.validate.futureAccountType = (function($input) {
	$input = $input.filter(':checked:not(:disabled)');
	
	var value = $input.val();

	// console.log('value = '+value);
	if (!$input.length) {
		return this.output('抱歉 申请用户太多，请稍候再来！');
	} else if (!value) {
		return this.output('请选择 操作账户！');
	}//if

	return this.output('', true);
});//f7.validate.futureAccountType()

/////////////////////////////////////////////////////////////////////////////////////////////

// 车讯快报 评论
f7.validate.commentContent = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('评论内容 不能为空！');
	} else if (len > 256) {
	 	return this.output('评论内容 不能小于256个字符！');
	}//if

	return this.output('', true);
});//f7.validate.commentContent()

// 用户名
f7.validate.username = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('用户名 不能为空！');
	} else if (len < 6) {
	 	return this.output('用户名 不能小于6个字符！');
	} else if (len > 16) {
		return this.output('用户名 不能大于16个字符！');
	}//if

	return this.output('', true);
});//f7.validate.username()

// 真实姓名
f7.validate.realUsername = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('姓名 不能为空！');
	} else if (len < 2) {
	 	return this.output('姓名 不能小于2个字符！');
	} else if (len > 6) {
		return this.output('姓名 不能大于6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.realUsername()

// 验证 生日
f7.validate.birthday = (function($input) {
	var value = $.trim($input.val()),
		regexp = /^\d{4}(\/|\-)\d{1,2}(\/|\-)\d{1,2}$/i;

	// console.log('birthday = ' + value);
	if (value === '') {
		return this.output('生日 不能为空！');
	} else if (!regexp.test(value)) {
		return this.output('生日 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.birthday();

// 验证 性别
f7.validate.gender = (function($input) {
	if (!$input.filter(':checked').length) {
		return this.output('请选择 性别！');
	}//if

	return this.output('', true);
});//f7.validate.gender();

// 验证 外观 内饰
f7.validate.appearance = (function($input, name) {
	if (!$input.filter(':checked').length) {
		return this.output('请选择 '+ name +'！');
	}//if

	return this.output('', true);
});//f7.validate.appearance();

// 验证 省市区
f7.validate.addressId = (function($input, name) {
	var value = $.trim($input.val());

	name = name ? name : '省';//if

	if (value === '') {
		return this.output('请选择 ' + name);
	}//if

	return this.output('', true);
});//f7.validate.addressId();

// 验证 个性签名
f7.validate.intro = (function($input) {
	var value = $.trim($input.val());

	if (value === '') {
		return this.output('个性签名 不能为空！');
	} else if (len < 6) {
	 	return this.output('个性签名 不能小于6个字符！');
	} else if (len > 120) {
		return this.output('个性签名 不能大于120个字符！');
	}//if

	return this.output('', true);
});//f7.validate.intro();





// 邮箱验证
f7.validate.email = (function($input) {
	var regexp = /^([\.a-z0-9_-])+@([a-z0-9_-])+(\.[a-z0-9_-])+/i,
		value = $.trim($input.val());

	if (value === '') {
		return this.output('邮箱地址 不能为空！');
	} else if (!regexp.test(value)) {
		return this.output('邮箱地址 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.email()

// 真实姓名
f7.validate.realName = (function($input) {
	var value = $.trim($input.val());

	if (value === '') {
		return this.output('姓名 不能为空！');
	} else if (/\w+/.test(value)) {
		return this.output('姓名 不能含有字母数字字符！');
	}//if

	return this.output('', true);
});//f7.validate.realName()

// 身份证号
f7.validate.certificateCardNo = (function($input) {
	var value = $.trim($input.val()),
		identityCodeValid = (function(code) { 
			var city = {11:'北京', 12:'天津', 13:'河北', 14:'山西', 15:'内蒙古', 
						21:'辽宁', 22:'吉林', 23:'黑龙江', 31:'上海', 32:'江苏',
						33:'浙江', 34:'安徽', 35:'福建', 36:'江西', 37:'山东',
						41:'河南', 42:'湖北', 43:'湖南', 44:'广东', 45:'广西',
						46:'海南', 50:'重庆', 51:'四川', 52:'贵州', 53:'云南',
						54:'西藏', 61:'陕西', 62:'甘肃', 63:'青海', 64:'宁夏',
						65:'新疆', 71:'台湾', 81:'香港', 82:'澳门', 91:'国外'};

			if (!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i.test(code)) {
				return false;
				// return this.output('身份证号 格式错误！');
			} else if (!city[code.substr(0,2)]) {
				return false;
				// return this.output('身份证号 地址编码错误！');
			} else {
				//18位身份证需要验证最后一位校验位
				if (code.length == 18) {
				    code = code.split('');
				    //∑(ai×Wi)(mod 11)
				    var factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2], //加权因子
				    	parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2], //校验位
				    	sum = 0,
				    	ai = 0,
				    	wi = 0;
				    for (var i = 0; i < 17; i++) {
				        ai = code[i];
				        wi = factor[i];
				        sum += ai * wi;
				    }//for 
				    var last = parity[sum % 11];
				    if (parity[sum % 11] != code[17]) {
				    	return false;
				    	// return this.output('身份证号 校验位错误！');
				    }//if
				}//if
			}//if

			return true;
        });//identityCodeValid()

	if (value === '') {
		return this.output('身份证号 不能为空！');
	} else if (!identityCodeValid(value)) {
		return this.output('身份证号 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.certificateCardNo()

// 手机号
f7.validate.tel = (function($input) {
	var value = $.trim($input.val()),
		regexp = /^1[3|4|5|8][0-9]\d{8}$/;

	if (value === '') {
		return this.output('手机号 不能为空！');
	} else if (!regexp.test(value)) {
	 	return this.output('手机号 格式错误！');
	}//if

	return this.output('', true);
});//f7.validate.tel()

// 短信验证码
f7.validate.verify = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('验证码 不能为空！');
	} else if (len < 6) {
		return this.output('验证码 必需为6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.verify()

// 图片验证码
f7.validate.checkcode = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (value === '') {
		return this.output('验证码 不能为空！');
	} else if (len < 5) {
		return this.output('验证码 必需为5个字符！');
	}//if

	return this.output('', true);
});

// 原始密码
f7.validate.oldPassword = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (len === 0) {
		return this.output('原始密码 不能为空！');
	} else if (len < 6) {
		return this.output('原始密码 不能小于6个字符！');
	}//if

	return this.output('', true);
});//f7.validate.oldPassword()

// 密码
f7.validate.password = (function($input) {
	var value = $.trim($input.val()),
		len = value.length;

	if (len === 0) {
		return this.output('密码 不能为空！');
	} else if (len < 6) {
		return this.output('密码 不能小于6个字符！');
	} else if (!(/\d+/).test(value) || !(/[a-z]+/i).test(value)) {
		return this.output('密码 必须含有字母与数字字符！');
	}//if

	return this.output('', true);
});//f7.validate.password()

// 确认密码
f7.validate.confirmPassword = (function($input1, $input2) {
	var value1 = $.trim($input1.val()),
		value2 = $.trim($input2.val());

	if (value2 === '') {
		return this.output('确认密码 不能为空！');
	} else if (value1 != value2) {
		return this.output('确认密码 错误！');
	}//if

	return this.output('', true);
});//f7.validate.confirmPassword()

// 验证 新老密码 是否一至
f7.validate.newOldPassword = (function($input1, $input2) {
	var value1 = $.trim($input1.val()),
		value2 = $.trim($input2.val());

	if (value1 === '') {
		return this.output('新密码 不能为空！');
	} else if (value1 == value2) {
		return this.output('新密码 未变更！');
	}//if

	return this.output('', true);
});//f7.validate.newOldPassword();

// 密码强度
f7.validate.passwordStrength = (function($input, $strength) {
	var value = $.trim($input.val()),
		len = value.length,
		$li = $strength.find('li'),
		index = -1;

	$li.removeClass('current');

	if (len < 6) {
		index = -1;
	} else if (len <= 9) {
		index = 0;
	} else if (len <= 12) {
		index = 1;
	} else if (len >= 12) {
		index = 2;
	}//if

	if (index != -1) {
		for (var i = 0; i <= index; i++) {
			$li.filter(':eq(' + i + ')').addClass('current');
		}//if
	}//if

	this.output(index);
});//f7.validate.passwordStrength()


