
/*!
 * jQuery UI 1.8.7
 *
 * Copyright 2010, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI
 */
(function(c,j){function k(a){return!c(a).parents().andSelf().filter(function(){return c.curCSS(this,"visibility")==="hidden"||c.expr.filters.hidden(this)}).length}c.ui=c.ui||{};if(!c.ui.version){c.extend(c.ui,{version:"1.8.7",keyCode:{ALT:18,BACKSPACE:8,CAPS_LOCK:20,COMMA:188,COMMAND:91,COMMAND_LEFT:91,COMMAND_RIGHT:93,CONTROL:17,DELETE:46,DOWN:40,END:35,ENTER:13,ESCAPE:27,HOME:36,INSERT:45,LEFT:37,MENU:93,NUMPAD_ADD:107,NUMPAD_DECIMAL:110,NUMPAD_DIVIDE:111,NUMPAD_ENTER:108,NUMPAD_MULTIPLY:106,
NUMPAD_SUBTRACT:109,PAGE_DOWN:34,PAGE_UP:33,PERIOD:190,RIGHT:39,SHIFT:16,SPACE:32,TAB:9,UP:38,WINDOWS:91}});c.fn.extend({_focus:c.fn.focus,focus:function(a,b){return typeof a==="number"?this.each(function(){var d=this;setTimeout(function(){c(d).focus();b&&b.call(d)},a)}):this._focus.apply(this,arguments)},scrollParent:function(){var a;a=c.browser.msie&&/(static|relative)/.test(this.css("position"))||/absolute/.test(this.css("position"))?this.parents().filter(function(){return/(relative|absolute|fixed)/.test(c.curCSS(this,
"position",1))&&/(auto|scroll)/.test(c.curCSS(this,"overflow",1)+c.curCSS(this,"overflow-y",1)+c.curCSS(this,"overflow-x",1))}).eq(0):this.parents().filter(function(){return/(auto|scroll)/.test(c.curCSS(this,"overflow",1)+c.curCSS(this,"overflow-y",1)+c.curCSS(this,"overflow-x",1))}).eq(0);return/fixed/.test(this.css("position"))||!a.length?c(document):a},zIndex:function(a){if(a!==j)return this.css("zIndex",a);if(this.length){a=c(this[0]);for(var b;a.length&&a[0]!==document;){b=a.css("position");
if(b==="absolute"||b==="relative"||b==="fixed"){b=parseInt(a.css("zIndex"),10);if(!isNaN(b)&&b!==0)return b}a=a.parent()}}return 0},disableSelection:function(){return this.bind((c.support.selectstart?"selectstart":"mousedown")+".ui-disableSelection",function(a){a.preventDefault()})},enableSelection:function(){return this.unbind(".ui-disableSelection")}});c.each(["Width","Height"],function(a,b){function d(f,g,l,m){c.each(e,function(){g-=parseFloat(c.curCSS(f,"padding"+this,true))||0;if(l)g-=parseFloat(c.curCSS(f,
"border"+this+"Width",true))||0;if(m)g-=parseFloat(c.curCSS(f,"margin"+this,true))||0});return g}var e=b==="Width"?["Left","Right"]:["Top","Bottom"],h=b.toLowerCase(),i={innerWidth:c.fn.innerWidth,innerHeight:c.fn.innerHeight,outerWidth:c.fn.outerWidth,outerHeight:c.fn.outerHeight};c.fn["inner"+b]=function(f){if(f===j)return i["inner"+b].call(this);return this.each(function(){c(this).css(h,d(this,f)+"px")})};c.fn["outer"+b]=function(f,g){if(typeof f!=="number")return i["outer"+b].call(this,f);return this.each(function(){c(this).css(h,
d(this,f,true,g)+"px")})}});c.extend(c.expr[":"],{data:function(a,b,d){return!!c.data(a,d[3])},focusable:function(a){var b=a.nodeName.toLowerCase(),d=c.attr(a,"tabindex");if("area"===b){b=a.parentNode;d=b.name;if(!a.href||!d||b.nodeName.toLowerCase()!=="map")return false;a=c("img[usemap=#"+d+"]")[0];return!!a&&k(a)}return(/input|select|textarea|button|object/.test(b)?!a.disabled:"a"==b?a.href||!isNaN(d):!isNaN(d))&&k(a)},tabbable:function(a){var b=c.attr(a,"tabindex");return(isNaN(b)||b>=0)&&c(a).is(":focusable")}});
c(function(){var a=document.body,b=a.appendChild(b=document.createElement("div"));c.extend(b.style,{minHeight:"100px",height:"auto",padding:0,borderWidth:0});c.support.minHeight=b.offsetHeight===100;c.support.selectstart="onselectstart"in b;a.removeChild(b).style.display="none"});c.extend(c.ui,{plugin:{add:function(a,b,d){a=c.ui[a].prototype;for(var e in d){a.plugins[e]=a.plugins[e]||[];a.plugins[e].push([b,d[e]])}},call:function(a,b,d){if((b=a.plugins[b])&&a.element[0].parentNode)for(var e=0;e<b.length;e++)a.options[b[e][0]]&&
b[e][1].apply(a.element,d)}},contains:function(a,b){return document.compareDocumentPosition?a.compareDocumentPosition(b)&16:a!==b&&a.contains(b)},hasScroll:function(a,b){if(c(a).css("overflow")==="hidden")return false;b=b&&b==="left"?"scrollLeft":"scrollTop";var d=false;if(a[b]>0)return true;a[b]=1;d=a[b]>0;a[b]=0;return d},isOverAxis:function(a,b,d){return a>b&&a<b+d},isOver:function(a,b,d,e,h,i){return c.ui.isOverAxis(a,d,h)&&c.ui.isOverAxis(b,e,i)}})}})(jQuery);
;

/*!
 * jQuery UI Widget 1.8.7
 *
 * Copyright 2010, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Widget
 */
(function(b,j){if(b.cleanData){var k=b.cleanData;b.cleanData=function(a){for(var c=0,d;(d=a[c])!=null;c++)b(d).triggerHandler("remove");k(a)}}else{var l=b.fn.remove;b.fn.remove=function(a,c){return this.each(function(){if(!c)if(!a||b.filter(a,[this]).length)b("*",this).add([this]).each(function(){b(this).triggerHandler("remove")});return l.call(b(this),a,c)})}}b.widget=function(a,c,d){var e=a.split(".")[0],f;a=a.split(".")[1];f=e+"-"+a;if(!d){d=c;c=b.Widget}b.expr[":"][f]=function(h){return!!b.data(h,
a)};b[e]=b[e]||{};b[e][a]=function(h,g){arguments.length&&this._createWidget(h,g)};c=new c;c.options=b.extend(true,{},c.options);b[e][a].prototype=b.extend(true,c,{namespace:e,widgetName:a,widgetEventPrefix:b[e][a].prototype.widgetEventPrefix||a,widgetBaseClass:f},d);b.widget.bridge(a,b[e][a])};b.widget.bridge=function(a,c){b.fn[a]=function(d){var e=typeof d==="string",f=Array.prototype.slice.call(arguments,1),h=this;d=!e&&f.length?b.extend.apply(null,[true,d].concat(f)):d;if(e&&d.charAt(0)==="_")return h;
e?this.each(function(){var g=b.data(this,a),i=g&&b.isFunction(g[d])?g[d].apply(g,f):g;if(i!==g&&i!==j){h=i;return false}}):this.each(function(){var g=b.data(this,a);g?g.option(d||{})._init():b.data(this,a,new c(d,this))});return h}};b.Widget=function(a,c){arguments.length&&this._createWidget(a,c)};b.Widget.prototype={widgetName:"widget",widgetEventPrefix:"",options:{disabled:false},_createWidget:function(a,c){b.data(c,this.widgetName,this);this.element=b(c);this.options=b.extend(true,{},this.options,
this._getCreateOptions(),a);var d=this;this.element.bind("remove."+this.widgetName,function(){d.destroy()});this._create();this._trigger("create");this._init()},_getCreateOptions:function(){return b.metadata&&b.metadata.get(this.element[0])[this.widgetName]},_create:function(){},_init:function(){},destroy:function(){this.element.unbind("."+this.widgetName).removeData(this.widgetName);this.widget().unbind("."+this.widgetName).removeAttr("aria-disabled").removeClass(this.widgetBaseClass+"-disabled ui-state-disabled")},
widget:function(){return this.element},option:function(a,c){var d=a;if(arguments.length===0)return b.extend({},this.options);if(typeof a==="string"){if(c===j)return this.options[a];d={};d[a]=c}this._setOptions(d);return this},_setOptions:function(a){var c=this;b.each(a,function(d,e){c._setOption(d,e)});return this},_setOption:function(a,c){this.options[a]=c;if(a==="disabled")this.widget()[c?"addClass":"removeClass"](this.widgetBaseClass+"-disabled ui-state-disabled").attr("aria-disabled",c);return this},
enable:function(){return this._setOption("disabled",false)},disable:function(){return this._setOption("disabled",true)},_trigger:function(a,c,d){var e=this.options[a];c=b.Event(c);c.type=(a===this.widgetEventPrefix?a:this.widgetEventPrefix+a).toLowerCase();d=d||{};if(c.originalEvent){a=b.event.props.length;for(var f;a;){f=b.event.props[--a];c[f]=c.originalEvent[f]}}this.element.trigger(c,d);return!(b.isFunction(e)&&e.call(this.element[0],c,d)===false||c.isDefaultPrevented())}}})(jQuery);
;

/*
 * jQuery UI Position 1.8.7
 *
 * Copyright 2010, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Position
 */
(function(c){c.ui=c.ui||{};var n=/left|center|right/,o=/top|center|bottom/,t=c.fn.position,u=c.fn.offset;c.fn.position=function(b){if(!b||!b.of)return t.apply(this,arguments);b=c.extend({},b);var a=c(b.of),d=a[0],g=(b.collision||"flip").split(" "),e=b.offset?b.offset.split(" "):[0,0],h,k,j;if(d.nodeType===9){h=a.width();k=a.height();j={top:0,left:0}}else if(d.setTimeout){h=a.width();k=a.height();j={top:a.scrollTop(),left:a.scrollLeft()}}else if(d.preventDefault){b.at="left top";h=k=0;j={top:b.of.pageY,
left:b.of.pageX}}else{h=a.outerWidth();k=a.outerHeight();j=a.offset()}c.each(["my","at"],function(){var f=(b[this]||"").split(" ");if(f.length===1)f=n.test(f[0])?f.concat(["center"]):o.test(f[0])?["center"].concat(f):["center","center"];f[0]=n.test(f[0])?f[0]:"center";f[1]=o.test(f[1])?f[1]:"center";b[this]=f});if(g.length===1)g[1]=g[0];e[0]=parseInt(e[0],10)||0;if(e.length===1)e[1]=e[0];e[1]=parseInt(e[1],10)||0;if(b.at[0]==="right")j.left+=h;else if(b.at[0]==="center")j.left+=h/2;if(b.at[1]==="bottom")j.top+=
k;else if(b.at[1]==="center")j.top+=k/2;j.left+=e[0];j.top+=e[1];return this.each(function(){var f=c(this),l=f.outerWidth(),m=f.outerHeight(),p=parseInt(c.curCSS(this,"marginLeft",true))||0,q=parseInt(c.curCSS(this,"marginTop",true))||0,v=l+p+parseInt(c.curCSS(this,"marginRight",true))||0,w=m+q+parseInt(c.curCSS(this,"marginBottom",true))||0,i=c.extend({},j),r;if(b.my[0]==="right")i.left-=l;else if(b.my[0]==="center")i.left-=l/2;if(b.my[1]==="bottom")i.top-=m;else if(b.my[1]==="center")i.top-=m/2;
i.left=Math.round(i.left);i.top=Math.round(i.top);r={left:i.left-p,top:i.top-q};c.each(["left","top"],function(s,x){c.ui.position[g[s]]&&c.ui.position[g[s]][x](i,{targetWidth:h,targetHeight:k,elemWidth:l,elemHeight:m,collisionPosition:r,collisionWidth:v,collisionHeight:w,offset:e,my:b.my,at:b.at})});c.fn.bgiframe&&f.bgiframe();f.offset(c.extend(i,{using:b.using}))})};c.ui.position={fit:{left:function(b,a){var d=c(window);d=a.collisionPosition.left+a.collisionWidth-d.width()-d.scrollLeft();b.left=
d>0?b.left-d:Math.max(b.left-a.collisionPosition.left,b.left)},top:function(b,a){var d=c(window);d=a.collisionPosition.top+a.collisionHeight-d.height()-d.scrollTop();b.top=d>0?b.top-d:Math.max(b.top-a.collisionPosition.top,b.top)}},flip:{left:function(b,a){if(a.at[0]!=="center"){var d=c(window);d=a.collisionPosition.left+a.collisionWidth-d.width()-d.scrollLeft();var g=a.my[0]==="left"?-a.elemWidth:a.my[0]==="right"?a.elemWidth:0,e=a.at[0]==="left"?a.targetWidth:-a.targetWidth,h=-2*a.offset[0];b.left+=
a.collisionPosition.left<0?g+e+h:d>0?g+e+h:0}},top:function(b,a){if(a.at[1]!=="center"){var d=c(window);d=a.collisionPosition.top+a.collisionHeight-d.height()-d.scrollTop();var g=a.my[1]==="top"?-a.elemHeight:a.my[1]==="bottom"?a.elemHeight:0,e=a.at[1]==="top"?a.targetHeight:-a.targetHeight,h=-2*a.offset[1];b.top+=a.collisionPosition.top<0?g+e+h:d>0?g+e+h:0}}}};if(!c.offset.setOffset){c.offset.setOffset=function(b,a){if(/static/.test(c.curCSS(b,"position")))b.style.position="relative";var d=c(b),
g=d.offset(),e=parseInt(c.curCSS(b,"top",true),10)||0,h=parseInt(c.curCSS(b,"left",true),10)||0;g={top:a.top-g.top+e,left:a.left-g.left+h};"using"in a?a.using.call(b,g):d.css(g)};c.fn.offset=function(b){var a=this[0];if(!a||!a.ownerDocument)return null;if(b)return this.each(function(){c.offset.setOffset(this,b)});return u.call(this)}}})(jQuery);
;

/*
 * jQuery UI Autocomplete 1.8.7
 *
 * Copyright 2010, AUTHORS.txt (http://jqueryui.com/about)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://docs.jquery.com/UI/Autocomplete
 *
 * Depends:
 *	jquery.ui.core.js
 *	jquery.ui.widget.js
 *	jquery.ui.position.js
 */
(function(d){d.widget("ui.autocomplete",{options:{appendTo:"body",delay:300,minLength:1,position:{my:"left top",at:"left bottom",collision:"none"},source:null},_create:function(){var a=this,b=this.element[0].ownerDocument,f;this.element.addClass("ui-autocomplete-input").attr("autocomplete","off").attr({role:"textbox","aria-autocomplete":"list","aria-haspopup":"true"}).bind("keydown.autocomplete",function(c){if(!(a.options.disabled||a.element.attr("readonly"))){f=false;var e=d.ui.keyCode;switch(c.keyCode){case e.PAGE_UP:a._move("previousPage",
c);break;case e.PAGE_DOWN:a._move("nextPage",c);break;case e.UP:a._move("previous",c);c.preventDefault();break;case e.DOWN:a._move("next",c);c.preventDefault();break;case e.ENTER:case e.NUMPAD_ENTER:if(a.menu.active){f=true;c.preventDefault()}case e.TAB:if(!a.menu.active)return;a.menu.select(c);break;case e.ESCAPE:a.element.val(a.term);a.close(c);break;default:clearTimeout(a.searching);a.searching=setTimeout(function(){if(a.term!=a.element.val()){a.selectedItem=null;a.search(null,c)}},a.options.delay);
break}}}).bind("keypress.autocomplete",function(c){if(f){f=false;c.preventDefault()}}).bind("focus.autocomplete",function(){if(!a.options.disabled){a.selectedItem=null;a.previous=a.element.val()}}).bind("blur.autocomplete",function(c){if(!a.options.disabled){clearTimeout(a.searching);a.closing=setTimeout(function(){a.close(c);a._change(c)},150)}});this._initSource();this.response=function(){return a._response.apply(a,arguments)};this.menu=d("<ul></ul>").addClass("ui-autocomplete").appendTo(d(this.options.appendTo||
"body",b)[0]).mousedown(function(c){var e=a.menu.element[0];d(c.target).closest(".ui-menu-item").length||setTimeout(function(){d(document).one("mousedown",function(g){g.target!==a.element[0]&&g.target!==e&&!d.ui.contains(e,g.target)&&a.close()})},1);setTimeout(function(){clearTimeout(a.closing)},13)}).menu({focus:function(c,e){e=e.item.data("item.autocomplete");false!==a._trigger("focus",c,{item:e})&&/^key/.test(c.originalEvent.type)&&a.element.val(e.value)},selected:function(c,e){var g=e.item.data("item.autocomplete"),
h=a.previous;if(a.element[0]!==b.activeElement){a.element.focus();a.previous=h;setTimeout(function(){a.previous=h;a.selectedItem=g},1)}false!==a._trigger("select",c,{item:g})&&a.element.val(g.value);a.term=a.element.val();a.close(c);a.selectedItem=g},blur:function(){a.menu.element.is(":visible")&&a.element.val()!==a.term&&a.element.val(a.term)}}).zIndex(this.element.zIndex()+1).css({top:0,left:0}).hide().data("menu");d.fn.bgiframe&&this.menu.element.bgiframe()},destroy:function(){this.element.removeClass("ui-autocomplete-input").removeAttr("autocomplete").removeAttr("role").removeAttr("aria-autocomplete").removeAttr("aria-haspopup");
this.menu.element.remove();d.Widget.prototype.destroy.call(this)},_setOption:function(a,b){d.Widget.prototype._setOption.apply(this,arguments);a==="source"&&this._initSource();if(a==="appendTo")this.menu.element.appendTo(d(b||"body",this.element[0].ownerDocument)[0])},_initSource:function(){var a=this,b,f;if(d.isArray(this.options.source)){b=this.options.source;this.source=function(c,e){e(d.ui.autocomplete.filter(b,c.term))}}else if(typeof this.options.source==="string"){f=this.options.source;this.source=
function(c,e){a.xhr&&a.xhr.abort();a.xhr=d.ajax({url:f,data:c,dataType:"json",success:function(g,h,i){i===a.xhr&&e(g);a.xhr=null},error:function(g){g===a.xhr&&e([]);a.xhr=null}})}}else this.source=this.options.source},search:function(a,b){a=a!=null?a:this.element.val();this.term=this.element.val();if(a.length<this.options.minLength)return this.close(b);clearTimeout(this.closing);if(this._trigger("search",b)!==false)return this._search(a)},_search:function(a){this.element.addClass("ui-autocomplete-loading");
this.source({term:a},this.response)},_response:function(a){if(a&&a.length){a=this._normalize(a);this._suggest(a);this._trigger("open")}else this.close();this.element.removeClass("ui-autocomplete-loading")},close:function(a){clearTimeout(this.closing);if(this.menu.element.is(":visible")){this.menu.element.hide();this.menu.deactivate();this._trigger("close",a)}},_change:function(a){this.previous!==this.element.val()&&this._trigger("change",a,{item:this.selectedItem})},_normalize:function(a){if(a.length&&
a[0].label&&a[0].value)return a;return d.map(a,function(b){if(typeof b==="string")return{label:b,value:b};return d.extend({label:b.label||b.value,value:b.value||b.label},b)})},_suggest:function(a){var b=this.menu.element.empty().zIndex(this.element.zIndex()+1);this._renderMenu(b,a);this.menu.deactivate();this.menu.refresh();b.show();this._resizeMenu();b.position(d.extend({of:this.element},this.options.position))},_resizeMenu:function(){var a=this.menu.element;a.outerWidth(Math.max(a.width("").outerWidth(),
this.element.outerWidth()))},_renderMenu:function(a,b){var f=this;d.each(b,function(c,e){f._renderItem(a,e)})},_renderItem:function(a,b){return d("<li></li>").data("item.autocomplete",b).append(d("<a></a>").text(b.label)).appendTo(a)},_move:function(a,b){if(this.menu.element.is(":visible"))if(this.menu.first()&&/^previous/.test(a)||this.menu.last()&&/^next/.test(a)){this.element.val(this.term);this.menu.deactivate()}else this.menu[a](b);else this.search(null,b)},widget:function(){return this.menu.element}});
d.extend(d.ui.autocomplete,{escapeRegex:function(a){return a.replace(/[-[\]{}()*+?.,\\^$|#\s]/g,"\\$&")},filter:function(a,b){var f=new RegExp(d.ui.autocomplete.escapeRegex(b),"i");return d.grep(a,function(c){return f.test(c.label||c.value||c)})}})})(jQuery);
(function(d){d.widget("ui.menu",{_create:function(){var a=this;this.element.addClass("ui-menu ui-widget ui-widget-content ui-corner-all").attr({role:"listbox","aria-activedescendant":"ui-active-menuitem"}).click(function(b){if(d(b.target).closest(".ui-menu-item a").length){b.preventDefault();a.select(b)}});this.refresh()},refresh:function(){var a=this;this.element.children("li:not(.ui-menu-item):has(a)").addClass("ui-menu-item").attr("role","menuitem").children("a").addClass("ui-corner-all").attr("tabindex",
-1).mouseenter(function(b){a.activate(b,d(this).parent())}).mouseleave(function(){a.deactivate()})},activate:function(a,b){this.deactivate();if(this.hasScroll()){var f=b.offset().top-this.element.offset().top,c=this.element.attr("scrollTop"),e=this.element.height();if(f<0)this.element.attr("scrollTop",c+f);else f>=e&&this.element.attr("scrollTop",c+f-e+b.height())}this.active=b.eq(0).children("a").addClass("ui-state-hover").attr("id","ui-active-menuitem").end();this._trigger("focus",a,{item:b})},
deactivate:function(){if(this.active){this.active.children("a").removeClass("ui-state-hover").removeAttr("id");this._trigger("blur");this.active=null}},next:function(a){this.move("next",".ui-menu-item:first",a)},previous:function(a){this.move("prev",".ui-menu-item:last",a)},first:function(){return this.active&&!this.active.prevAll(".ui-menu-item").length},last:function(){return this.active&&!this.active.nextAll(".ui-menu-item").length},move:function(a,b,f){if(this.active){a=this.active[a+"All"](".ui-menu-item").eq(0);
a.length?this.activate(f,a):this.activate(f,this.element.children(b))}else this.activate(f,this.element.children(b))},nextPage:function(a){if(this.hasScroll())if(!this.active||this.last())this.activate(a,this.element.children(".ui-menu-item:first"));else{var b=this.active.offset().top,f=this.element.height(),c=this.element.children(".ui-menu-item").filter(function(){var e=d(this).offset().top-b-f+d(this).height();return e<10&&e>-10});c.length||(c=this.element.children(".ui-menu-item:last"));this.activate(a,
c)}else this.activate(a,this.element.children(".ui-menu-item").filter(!this.active||this.last()?":first":":last"))},previousPage:function(a){if(this.hasScroll())if(!this.active||this.first())this.activate(a,this.element.children(".ui-menu-item:last"));else{var b=this.active.offset().top,f=this.element.height();result=this.element.children(".ui-menu-item").filter(function(){var c=d(this).offset().top-b+f-d(this).height();return c<10&&c>-10});result.length||(result=this.element.children(".ui-menu-item:first"));
this.activate(a,result)}else this.activate(a,this.element.children(".ui-menu-item").filter(!this.active||this.first()?":last":":first"))},hasScroll:function(){return this.element.height()<this.element.attr("scrollHeight")},select:function(a){this._trigger("selected",a,{item:this.active})}})})(jQuery);
;

/**
 * Cookie plugin 1.0
 *
 * Copyright (c) 2006 Klaus Hartl (stilbuero.de)
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 */
jQuery.cookie=function(b,j,m){if(typeof j!="undefined"){m=m||{};if(j===null){j="";m.expires=-1}var e="";if(m.expires&&(typeof m.expires=="number"||m.expires.toUTCString)){var f;if(typeof m.expires=="number"){f=new Date();f.setTime(f.getTime()+(m.expires*24*60*60*1000))}else{f=m.expires}e="; expires="+f.toUTCString()}var l=m.path?"; path="+(m.path):"";var g=m.domain?"; domain="+(m.domain):"";var a=m.secure?"; secure":"";document.cookie=[b,"=",encodeURIComponent(j),e,l,g,a].join("")}else{var d=null;if(document.cookie&&document.cookie!=""){var k=document.cookie.split(";");for(var h=0;h<k.length;h++){var c=jQuery.trim(k[h]);if(c.substring(0,b.length+1)==(b+"=")){d=decodeURIComponent(c.substring(b.length+1));break}}}return d}};
;

/*!
 * jQuery Form Plugin
 * version: 2.52 (07-DEC-2010)
 * @requires jQuery v1.3.2 or later
 *
 * Examples and documentation at: http://malsup.com/jquery/form/
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 */
;(function(b){function q(){if(b.fn.ajaxSubmit.debug){var a="[jquery.form] "+Array.prototype.join.call(arguments,"");if(window.console&&window.console.log)window.console.log(a);else window.opera&&window.opera.postError&&window.opera.postError(a)}}b.fn.ajaxSubmit=function(a){function f(){function t(){var o=i.attr("target"),m=i.attr("action");l.setAttribute("target",u);l.getAttribute("method")!="POST"&&l.setAttribute("method","POST");l.getAttribute("action")!=e.url&&l.setAttribute("action",e.url);e.skipEncodingOverride|| i.attr({encoding:"multipart/form-data",enctype:"multipart/form-data"});e.timeout&&setTimeout(function(){F=true;s()},e.timeout);var v=[];try{if(e.extraData)for(var w in e.extraData)v.push(b('<input type="hidden" name="'+w+'" value="'+e.extraData[w]+'" />').appendTo(l)[0]);r.appendTo("body");r.data("form-plugin-onload",s);l.submit()}finally{l.setAttribute("action",m);o?l.setAttribute("target",o):i.removeAttr("target");b(v).remove()}}function s(){if(!G){r.removeData("form-plugin-onload");var o=true; try{if(F)throw"timeout";p=x.contentWindow?x.contentWindow.document:x.contentDocument?x.contentDocument:x.document;var m=e.dataType=="xml"||p.XMLDocument||b.isXMLDoc(p);q("isXml="+m);if(!m&&window.opera&&(p.body==null||p.body.innerHTML==""))if(--K){q("requeing onLoad callback, DOM not available");setTimeout(s,250);return}G=true;j.responseText=p.documentElement?p.documentElement.innerHTML:null;j.responseXML=p.XMLDocument?p.XMLDocument:p;j.getResponseHeader=function(L){return{"content-type":e.dataType}[L]}; var v=/(json|script)/.test(e.dataType);if(v||e.textarea){var w=p.getElementsByTagName("textarea")[0];if(w)j.responseText=w.value;else if(v){var H=p.getElementsByTagName("pre")[0],I=p.getElementsByTagName("body")[0];if(H)j.responseText=H.textContent;else if(I)j.responseText=I.innerHTML}}else if(e.dataType=="xml"&&!j.responseXML&&j.responseText!=null)j.responseXML=C(j.responseText);J=b.httpData(j,e.dataType)}catch(D){q("error caught:",D);o=false;j.error=D;b.handleError(e,j,"error",D)}if(j.aborted){q("upload aborted"); o=false}if(o){e.success.call(e.context,J,"success",j);y&&b.event.trigger("ajaxSuccess",[j,e])}y&&b.event.trigger("ajaxComplete",[j,e]);y&&!--b.active&&b.event.trigger("ajaxStop");if(e.complete)e.complete.call(e.context,j,o?"success":"error");setTimeout(function(){r.removeData("form-plugin-onload");r.remove();j.responseXML=null},100)}}function C(o,m){if(window.ActiveXObject){m=new ActiveXObject("Microsoft.XMLDOM");m.async="false";m.loadXML(o)}else m=(new DOMParser).parseFromString(o,"text/xml");return m&& m.documentElement&&m.documentElement.tagName!="parsererror"?m:null}var l=i[0];if(b(":input[name=submit],:input[id=submit]",l).length)alert('Error: Form elements must not have name or id of "submit".');else{var e=b.extend(true,{},b.ajaxSettings,a);e.context=e.context||e;var u="jqFormIO"+(new Date).getTime(),E="_"+u;window[E]=function(){var o=r.data("form-plugin-onload");if(o){o();window[E]=undefined;try{delete window[E]}catch(m){}}};var r=b('<iframe id="'+u+'" name="'+u+'" src="'+e.iframeSrc+'" onload="window[\'_\'+this.id]()" />'), x=r[0];r.css({position:"absolute",top:"-1000px",left:"-1000px"});var j={aborted:0,responseText:null,responseXML:null,status:0,statusText:"n/a",getAllResponseHeaders:function(){},getResponseHeader:function(){},setRequestHeader:function(){},abort:function(){this.aborted=1;r.attr("src",e.iframeSrc)}},y=e.global;y&&!b.active++&&b.event.trigger("ajaxStart");y&&b.event.trigger("ajaxSend",[j,e]);if(e.beforeSend&&e.beforeSend.call(e.context,j,e)===false)e.global&&b.active--;else if(!j.aborted){var G=false, F=0,z=l.clk;if(z){var A=z.name;if(A&&!z.disabled){e.extraData=e.extraData||{};e.extraData[A]=z.value;if(z.type=="image"){e.extraData[A+".x"]=l.clk_x;e.extraData[A+".y"]=l.clk_y}}}e.forceSync?t():setTimeout(t,10);var J,p,K=50}}}if(!this.length){q("ajaxSubmit: skipping submit process - no element selected");return this}if(typeof a=="function")a={success:a};var d=this.attr("action");if(d=typeof d==="string"?b.trim(d):"")d=(d.match(/^([^#]+)/)||[])[1];d=d||window.location.href||"";a=b.extend(true,{url:d, type:this.attr("method")||"GET",iframeSrc:/^https/i.test(window.location.href||"")?"javascript:false":"about:blank"},a);d={};this.trigger("form-pre-serialize",[this,a,d]);if(d.veto){q("ajaxSubmit: submit vetoed via form-pre-serialize trigger");return this}if(a.beforeSerialize&&a.beforeSerialize(this,a)===false){q("ajaxSubmit: submit aborted via beforeSerialize callback");return this}var c,h,g=this.formToArray(a.semantic);if(a.data){a.extraData=a.data;for(c in a.data)if(a.data[c]instanceof Array)for(var k in a.data[c])g.push({name:c, value:a.data[c][k]});else{h=a.data[c];h=b.isFunction(h)?h():h;g.push({name:c,value:h})}}if(a.beforeSubmit&&a.beforeSubmit(g,this,a)===false){q("ajaxSubmit: submit aborted via beforeSubmit callback");return this}this.trigger("form-submit-validate",[g,this,a,d]);if(d.veto){q("ajaxSubmit: submit vetoed via form-submit-validate trigger");return this}c=b.param(g);if(a.type.toUpperCase()=="GET"){a.url+=(a.url.indexOf("?")>=0?"&":"?")+c;a.data=null}else a.data=c;var i=this,n=[];a.resetForm&&n.push(function(){i.resetForm()}); a.clearForm&&n.push(function(){i.clearForm()});if(!a.dataType&&a.target){var B=a.success||function(){};n.push(function(t){var s=a.replaceTarget?"replaceWith":"html";b(a.target)[s](t).each(B,arguments)})}else a.success&&n.push(a.success);a.success=function(t,s,C){for(var l=a.context||a,e=0,u=n.length;e<u;e++)n[e].apply(l,[t,s,C||i,i])};c=b("input:file",this).length>0;k=i.attr("enctype")=="multipart/form-data"||i.attr("encoding")=="multipart/form-data";if(a.iframe!==false&&(c||a.iframe||k))a.closeKeepAlive? b.get(a.closeKeepAlive,f):f();else b.ajax(a);this.trigger("form-submit-notify",[this,a]);return this};b.fn.ajaxForm=function(a){if(this.length===0){var f={s:this.selector,c:this.context};if(!b.isReady&&f.s){q("DOM not ready, queuing ajaxForm");b(function(){b(f.s,f.c).ajaxForm(a)});return this}q("terminating; zero elements found by selector"+(b.isReady?"":" (DOM not ready)"));return this}return this.ajaxFormUnbind().bind("submit.form-plugin",function(d){if(!d.isDefaultPrevented()){d.preventDefault(); b(this).ajaxSubmit(a)}}).bind("click.form-plugin",function(d){var c=d.target,h=b(c);if(!h.is(":submit,input:image")){c=h.closest(":submit");if(c.length==0)return;c=c[0]}var g=this;g.clk=c;if(c.type=="image")if(d.offsetX!=undefined){g.clk_x=d.offsetX;g.clk_y=d.offsetY}else if(typeof b.fn.offset=="function"){h=h.offset();g.clk_x=d.pageX-h.left;g.clk_y=d.pageY-h.top}else{g.clk_x=d.pageX-c.offsetLeft;g.clk_y=d.pageY-c.offsetTop}setTimeout(function(){g.clk=g.clk_x=g.clk_y=null},100)})};b.fn.ajaxFormUnbind= function(){return this.unbind("submit.form-plugin click.form-plugin")};b.fn.formToArray=function(a){var f=[];if(this.length===0)return f;var d=this[0],c=a?d.getElementsByTagName("*"):d.elements;if(!c)return f;var h,g,k,i,n,B;h=0;for(n=c.length;h<n;h++){g=c[h];if(k=g.name)if(a&&d.clk&&g.type=="image"){if(!g.disabled&&d.clk==g){f.push({name:k,value:b(g).val()});f.push({name:k+".x",value:d.clk_x},{name:k+".y",value:d.clk_y})}}else if((i=b.fieldValue(g,true))&&i.constructor==Array){g=0;for(B=i.length;g< B;g++)f.push({name:k,value:i[g]})}else i!==null&&typeof i!="undefined"&&f.push({name:k,value:i})}if(!a&&d.clk){a=b(d.clk);c=a[0];if((k=c.name)&&!c.disabled&&c.type=="image"){f.push({name:k,value:a.val()});f.push({name:k+".x",value:d.clk_x},{name:k+".y",value:d.clk_y})}}return f};b.fn.formSerialize=function(a){return b.param(this.formToArray(a))};b.fn.fieldSerialize=function(a){var f=[];this.each(function(){var d=this.name;if(d){var c=b.fieldValue(this,a);if(c&&c.constructor==Array)for(var h=0,g=c.length;h< g;h++)f.push({name:d,value:c[h]});else c!==null&&typeof c!="undefined"&&f.push({name:this.name,value:c})}});return b.param(f)};b.fn.fieldValue=function(a){for(var f=[],d=0,c=this.length;d<c;d++){var h=b.fieldValue(this[d],a);h===null||typeof h=="undefined"||h.constructor==Array&&!h.length||(h.constructor==Array?b.merge(f,h):f.push(h))}return f};b.fieldValue=function(a,f){var d=a.name,c=a.type,h=a.tagName.toLowerCase();if(f===undefined)f=true;if(f&&(!d||a.disabled||c=="reset"||c=="button"||(c=="checkbox"|| c=="radio")&&!a.checked||(c=="submit"||c=="image")&&a.form&&a.form.clk!=a||h=="select"&&a.selectedIndex==-1))return null;if(h=="select"){var g=a.selectedIndex;if(g<0)return null;d=[];h=a.options;var k=(c=c=="select-one")?g+1:h.length;for(g=c?g:0;g<k;g++){var i=h[g];if(i.selected){var n=i.value;n||(n=i.attributes&&i.attributes.value&&!i.attributes.value.specified?i.text:i.value);if(c)return n;d.push(n)}}return d}return b(a).val()};b.fn.clearForm=function(){return this.each(function(){b("input,select,textarea", this).clearFields()})};b.fn.clearFields=b.fn.clearInputs=function(){return this.each(function(){var a=this.type,f=this.tagName.toLowerCase();if(a=="text"||a=="password"||f=="textarea")this.value="";else if(a=="checkbox"||a=="radio")this.checked=false;else if(f=="select")this.selectedIndex=-1})};b.fn.resetForm=function(){return this.each(function(){if(typeof this.reset=="function"||typeof this.reset=="object"&&!this.reset.nodeType)this.reset()})};b.fn.enable=function(a){if(a===undefined)a=true;return this.each(function(){this.disabled= !a})};b.fn.selected=function(a){if(a===undefined)a=true;return this.each(function(){var f=this.type;if(f=="checkbox"||f=="radio")this.checked=a;else if(this.tagName.toLowerCase()=="option"){f=b(this).parent("select");a&&f[0]&&f[0].type=="select-one"&&f.find("option").selected(false);this.selected=a}})}})(jQuery);;
var geoip2=function(){"use strict";var GEOIP_URI_ME="//2y0abj6vp2.execute-api.us-west-2.amazonaws.com/production/me",GEOIP_URI_LOOKUP="//2y0abj6vp2.execute-api.us-west-2.amazonaws.com/production/ip/",exports={};function Lookup(e,t,o,n){this.successCallback=e,this.errorCallback=t,this.geolocation=o&&o.hasOwnProperty("geolocation")?o.geolocation:navigator.geolocation,this.type=n}Lookup.prototype.returnSuccess=function(e){this.successCallback&&"function"==typeof this.successCallback&&this.successCallback(this.fillInObject(this.objectFromJSON(e)))},Lookup.prototype.returnError=function(e){this.errorCallback&&"function"==typeof this.errorCallback&&(e||(e={error:"Unknown error"}),this.errorCallback(e))},Lookup.prototype.objectFromJSON=function(json){return void 0!==window.JSON&&"function"==typeof window.JSON.parse?window.JSON.parse(json):eval("("+json+")")};var fillIns={country:[["continent","Object","names","Object"],["country","Object","names","Object"],["registered_country","Object","names","Object"],["represented_country","Object","names","Object"],["traits","Object"]],city:[["city","Object","names","Object"],["continent","Object","names","Object"],["country","Object","names","Object"],["location","Object"],["postal","Object"],["registered_country","Object","names","Object"],["represented_country","Object","names","Object"],["subdivisions","Array",0,"Object","names","Object"],["traits","Object"]]};return Lookup.prototype.fillInObject=function(t){for(var e="country"===this.type?fillIns.country:fillIns.city,o=0;o<e.length;o++)for(var n=e[o],r=t,s=0;s<n.length;s+=2){var i=n[s];r[i]||(r[i]="Object"===n[s+1]?{}:[]),r=r[i]}try{Object.defineProperty(t.continent,"continent_code",{enumerable:!1,get:function(){return this.code},set:function(e){this.code=e}})}catch(e){t.continent.code&&(t.continent.continent_code=t.continent.code)}if("country"!==this.type)try{Object.defineProperty(t,"most_specific_subdivision",{enumerable:!1,get:function(){return this.subdivisions[this.subdivisions.length-1]},set:function(e){this.subdivisions[this.subdivisions.length-1]=e}})}catch(e){t.most_specific_subdivision=t.subdivisions[t.subdivisions.length-1]}return t},Lookup.prototype.overrideIp=function(){var e=window.location.href.match("smartsheet-ip-override=([0-9]+.[0-9]+.[0-9]+.[0-9]+)($|&)");return!(!e||!e[1])&&e[1]},Lookup.prototype.getGeoIPResult=function(){var o,n=this,e=GEOIP_URI_ME;if(!this.alreadyRan){this.alreadyRan=1,"Microsoft Internet Explorer"===navigator.appName&&window.XDomainRequest&&-1===navigator.appVersion.indexOf("MSIE 1")?(o=new XDomainRequest,e=window.location.protocol+e,o.onprogress=function(){}):(o=new window.XMLHttpRequest,e="https:"+e);var t=this.overrideIp();t&&(e=e.replace(new RegExp(GEOIP_URI_ME),GEOIP_URI_LOOKUP)+t,console.log("Using IP address",t)),o.open("GET",e,!0),o.onload=function(){if(void 0===o.status||200===o.status)n.returnSuccess(o.responseText);else{var t,e=o.hasOwnProperty("contentType")?o.contentType:o.getResponseHeader("Content-Type");if(/json/.test(e)&&o.responseText.length)try{t=n.objectFromJSON(o.responseText)}catch(e){t={code:"HTTP_ERROR",error:"The server returned a "+o.status+" status with an invalid JSON body."}}else t=o.responseText.length?{code:"HTTP_ERROR",error:"The server returned a "+o.status+" status with the following body: "+o.responseText}:{code:"HTTP_ERROR",error:"The server returned a "+o.status+" status but either the server did not return a body or this browser is a version of Internet Explorer that hides error bodies."};n.returnError(t)}},o.ontimeout=function(){n.returnError({code:"HTTP_TIMEOUT",error:"The request to the GeoIP2 web service timed out."})},o.onerror=function(){n.returnError({code:"HTTP_ERROR",error:"There was a network error receiving the response from the GeoIP2 web service."})},o.send(null)}},exports.country=function(e,t,o){new Lookup(e,t,o,"country").getGeoIPResult()},exports.city=function(e,t,o){new Lookup(e,t,o,"city").getGeoIPResult()},exports.insights=function(e,t,o){new Lookup(e,t,o,"insights").getGeoIPResult()},exports}();;
var Smartsheet=Smartsheet||{};Smartsheet.GeoIP=function(e){this.listeners=[],this.errorHandlers=[],this.geoip2=e,this.data=!1,this.errorMessage=!1,this.fetchData()},Smartsheet.GeoIP.prototype.fetchData=function(){this.geoip2.city(this.receiveData.bind(this),this.handleError.bind(this))},Smartsheet.GeoIP.prototype.receiveData=function(e){var t,r=this.listeners;for(this.data=e,t=0;t<r.length;t++)r[t](this.data)},Smartsheet.GeoIP.prototype.handleError=function(e){var t;for(this.errorMessage=e,t=0;t<this.errorHandlers.length;t++)this.errorHandlers[t](e)},Smartsheet.GeoIP.getInstance=function(){return void 0===Smartsheet.GeoIP._instance&&(Smartsheet.GeoIP._instance=new Smartsheet.GeoIP(geoip2)),Smartsheet.GeoIP._instance},Smartsheet.GeoIP.prototype.addListener=function(e){this.listeners.push(e),this.data&&e(this.data)},Smartsheet.GeoIP.prototype.addErrorHandler=function(e){this.errorHandlers.push(e),this.errorMessage&&e(this.errorMessage)};;
var Drupal = Drupal || {};
var Smartsheet = Smartsheet || {};

(function ($, Drupal, Smartsheet) {

  Drupal.behaviors = Drupal.behaviors || {};
  Drupal.behaviors.currencyPreselect = Drupal.behaviors.currencyPreselect || {};
  Drupal.behaviors.currencyPreselect.attach = function (context, settings) {

    // On pages with a currency dropdown, prepopulate with the correct currency.
    var $dropdown = $('.js-geolocation');
    $dropdown.once(function () {
      var $list = $('.interactive-list');

      // Add a change handler to the dropdown.
      $dropdown.change(function(){
        var currencyCode = $(this).val();
        $list.removeClass('js-active');
        $list.filter('.' + currencyCode).addClass('js-active');
      });

      // If/when the geoIP data is ready, use it to set the dropdown value and
      // the list values.
      var currencyHandler = new Smartsheet.Currency($dropdown, $list);
      Smartsheet.GeoIP.getInstance().addListener(currencyHandler.geoipListener.bind(currencyHandler));
    });
  };

  Smartsheet.Currency = function ($dropdown, $list) {
    this.$dropdown = $dropdown;
    this.$list = $list;
  };

  Smartsheet.Currency.prototype.geoipListener = function (data) {
    var countryCode = data.country.iso_code;
    var currencyCode = Smartsheet.Currency.getCurrency(countryCode);
    this.$list.removeClass('js-active');
    this.$list.filter('.' + currencyCode).addClass('js-active');
    this.$dropdown.val(currencyCode);
  };

  Smartsheet.Currency.getCurrency = function (countryCode) {
    var map = {
      'US': 'USD',
      'AU': 'AUD',
      'CA': 'CAD',
      'GB': 'GBP',
      'JP': 'JPY',
      'AL': 'EUR',
      'AD': 'EUR',
      'AT': 'EUR',
      'BE': 'EUR',
      'BA': 'EUR',
      'BG': 'EUR',
      'HR': 'EUR',
      'CY': 'EUR',
      'CZ': 'EUR',
      'DK': 'EUR',
      'EE': 'EUR',
      'FI': 'EUR',
      'FR': 'EUR',
      'GF': 'EUR',
      'DE': 'EUR',
      'GR': 'EUR',
      'GP': 'EUR',
      'HU': 'EUR',
      'IE': 'EUR',
      'IT': 'EUR',
      'LV': 'EUR',
      'LT': 'EUR',
      'LU': 'EUR',
      'MT': 'EUR',
      'MK': 'EUR',
      'MD': 'EUR',
      'MQ': 'EUR',
      'YT': 'EUR',
      'PM': 'EUR',
      'MC': 'EUR',
      'ME': 'EUR',
      'NL': 'EUR',
      'NO': 'EUR',
      'PT': 'EUR',
      'PL': 'EUR',
      'SM': 'EUR',
      'RE': 'EUR',
      'RO': 'EUR',
      'RS': 'EUR',
      'SK': 'EUR',
      'SI': 'EUR',
      'ES': 'EUR',
      'SE': 'EUR',
      'CH': 'EUR',
      'VA': 'EUR'
    };
    if (typeof map[countryCode] !== 'undefined') {
      return map[countryCode];
    }
    return 'USD';
  };

})(jQuery, Drupal, Smartsheet);
;

/*
 * jQuery BBQ: Back Button & Query Library - v1.2.1 - 2/17/2010
 * http://benalman.com/projects/jquery-bbq-plugin/
 *
 * Copyright (c) 2010 "Cowboy" Ben Alman
 * Dual licensed under the MIT and GPL licenses.
 * http://benalman.com/about/license/
 */
(function($,p){var i,m=Array.prototype.slice,r=decodeURIComponent,a=$.param,c,l,v,b=$.bbq=$.bbq||{},q,u,j,e=$.event.special,d="hashchange",A="querystring",D="fragment",y="elemUrlAttr",g="location",k="href",t="src",x=/^.*\?|#.*$/g,w=/^.*\#/,h,C={};function E(F){return typeof F==="string"}function B(G){var F=m.call(arguments,1);return function(){return G.apply(this,F.concat(m.call(arguments)))}}function n(F){return F.replace(/^[^#]*#?(.*)$/,"$1")}function o(F){return F.replace(/(?:^[^?#]*\?([^#]*).*$)?.*/,"$1")}function f(H,M,F,I,G){var O,L,K,N,J;if(I!==i){K=F.match(H?/^([^#]*)\#?(.*)$/:/^([^#?]*)\??([^#]*)(#?.*)/);J=K[3]||"";if(G===2&&E(I)){L=I.replace(H?w:x,"")}else{N=l(K[2]);I=E(I)?l[H?D:A](I):I;L=G===2?I:G===1?$.extend({},I,N):$.extend({},N,I);L=a(L);if(H){L=L.replace(h,r)}}O=K[1]+(H?"#":L||!K[1]?"?":"")+L+J}else{O=M(F!==i?F:p[g][k])}return O}a[A]=B(f,0,o);a[D]=c=B(f,1,n);c.noEscape=function(G){G=G||"";var F=$.map(G.split(""),encodeURIComponent);h=new RegExp(F.join("|"),"g")};c.noEscape(",/");$.deparam=l=function(I,F){var H={},G={"true":!0,"false":!1,"null":null};$.each(I.replace(/\+/g," ").split("&"),function(L,Q){var K=Q.split("="),P=r(K[0]),J,O=H,M=0,R=P.split("]["),N=R.length-1;if(/\[/.test(R[0])&&/\]$/.test(R[N])){R[N]=R[N].replace(/\]$/,"");R=R.shift().split("[").concat(R);N=R.length-1}else{N=0}if(K.length===2){J=r(K[1]);if(F){J=J&&!isNaN(J)?+J:J==="undefined"?i:G[J]!==i?G[J]:J}if(N){for(;M<=N;M++){P=R[M]===""?O.length:R[M];O=O[P]=M<N?O[P]||(R[M+1]&&isNaN(R[M+1])?{}:[]):J}}else{if($.isArray(H[P])){H[P].push(J)}else{if(H[P]!==i){H[P]=[H[P],J]}else{H[P]=J}}}}else{if(P){H[P]=F?i:""}}});return H};function z(H,F,G){if(F===i||typeof F==="boolean"){G=F;F=a[H?D:A]()}else{F=E(F)?F.replace(H?w:x,""):F}return l(F,G)}l[A]=B(z,0);l[D]=v=B(z,1);$[y]||($[y]=function(F){return $.extend(C,F)})({a:k,base:k,iframe:t,img:t,input:t,form:"action",link:k,script:t});j=$[y];function s(I,G,H,F){if(!E(H)&&typeof H!=="object"){F=H;H=G;G=i}return this.each(function(){var L=$(this),J=G||j()[(this.nodeName||"").toLowerCase()]||"",K=J&&L.attr(J)||"";L.attr(J,a[I](K,H,F))})}$.fn[A]=B(s,A);$.fn[D]=B(s,D);b.pushState=q=function(I,F){if(E(I)&&/^#/.test(I)&&F===i){F=2}var H=I!==i,G=c(p[g][k],H?I:{},H?F:2);p[g][k]=G+(/#/.test(G)?"":"#")};b.getState=u=function(F,G){return F===i||typeof F==="boolean"?v(F):v(G)[F]};b.removeState=function(F){var G={};if(F!==i){G=u();$.each($.isArray(F)?F:arguments,function(I,H){delete G[H]})}q(G,2)};e[d]=$.extend(e[d],{add:function(F){var H;function G(J){var I=J[D]=c();J.getState=function(K,L){return K===i||typeof K==="boolean"?l(I,K):l(I,L)[K]};H.apply(this,arguments)}if($.isFunction(F)){H=F;return G}else{H=F.handler;F.handler=G}}})})(jQuery,this);
/*
 * jQuery hashchange event - v1.2 - 2/11/2010
 * http://benalman.com/projects/jquery-hashchange-plugin/
 *
 * Copyright (c) 2010 "Cowboy" Ben Alman
 * Dual licensed under the MIT and GPL licenses.
 * http://benalman.com/about/license/
 */
(function($,i,b){var j,k=$.event.special,c="location",d="hashchange",l="href",f=$.browser,g=document.documentMode,h=f.msie&&(g===b||g<8),e="on"+d in i&&!h;function a(m){m=m||i[c][l];return m.replace(/^[^#]*#?(.*)$/,"$1")}$[d+"Delay"]=100;k[d]=$.extend(k[d],{setup:function(){if(e){return false}$(j.start)},teardown:function(){if(e){return false}$(j.stop)}});j=(function(){var m={},r,n,o,q;function p(){o=q=function(s){return s};if(h){n=$('<iframe src="javascript:0"/>').hide().insertAfter("body")[0].contentWindow;q=function(){return a(n.document[c][l])};o=function(u,s){if(u!==s){var t=n.document;t.open().close();t[c].hash="#"+u}};o(a())}}m.start=function(){if(r){return}var t=a();o||p();(function s(){var v=a(),u=q(t);if(v!==t){o(t=v,u);$(i).trigger(d)}else{if(u!==t){i[c][l]=i[c][l].replace(/#.*/,"")+"#"+u}}r=setTimeout(s,$[d+"Delay"])})()};m.stop=function(){if(!n){r&&clearTimeout(r);r=0}};return m})()})(jQuery,this);
;
(function ($) {
  Drupal.behaviors.persistentParam = {};
  Drupal.behaviors.persistentParam.attach = function (context) {
    var params = {
      // The "iOS," "frame" and "nav" parameters should follow the user
      // around on the site.
      'iOS': 'persistent',
      'frame': 'persistent',
      'nav': 'persistent',
      // The "landing" parameter is only for the first pageview.
      'landing': 'ephemeral'
    };
    var currentQuery = $.deparam.querystring();

    for (var param in params) {
      if (!params.hasOwnProperty(param)) {
        continue;
      }
      // Check the current URL for the given parameter.
      if (typeof currentQuery[param] !== 'undefined') {

        // For params that are supposed to persist, add them to links and forms.
        if (params[param] === 'persistent') {
          // Add the parameter to every link on the page.
          var querystringSettings = {};
          querystringSettings[param] = currentQuery[param];
          $('a').querystring(this.href, querystringSettings);

          // For any forms using GET, add a hidden input so the destination URL gets
          // ?iOS as well.
          $('form[method]').each(function(){
            var $form = $(this);
            if ($form.attr('method').toLowerCase() == 'get' && ($form.find('input[name="' + param + '"]').length == 0)) {
              $form.append($("<input type='hidden' value='" + currentQuery[param] + "' name='" + param +"'>"));
            }
          });

        }

        // For all params, whether persistent or ephemeral,
        // add appropriate classes on <body> so CSS can respond to the
        // parameter and its value.
        $('body').addClass(param);
        if (currentQuery[param] != 0) {
          // For truthy parameter values, add an additional class.
          $('body').addClass(param + '-' + currentQuery[param]);
        }
      }
    }

  };

})(jQuery);
;
var geoip = Smartsheet.GeoIP.getInstance();

geoip.addListener(function (data) {
  //  Redirect countries under embargo.
  var country_code = data.country.iso_code;
  var embargoed_countries = ['CU', 'IR', 'KP', 'SY'];
  if (embargoed_countries.indexOf(country_code) > -1) {
    redirect();
  }
  // Redirect for Crimea.
  if (country_code === 'UA' && data.subdivisions[0]) {
    var subdivision = data.subdivisions[0];
    if (subdivision.iso_code == '40' || subdivision.iso_code == '43') {
      redirect();
    }
  }
  function redirect() {
    if (window.location.pathname === '/restricted') {
      return;
    }
    window.location.pathname = '/restricted';
  }
});
;
!function(i,n,r){r.behaviors.smartsheetSignup={},r.behaviors.smartsheetSignup.attach=function(t,e){if(r.settings.smartsheetSignup){var n=r.smartsheetSignup.findSignupLinks(),a=!!new Smartsheet.Cookies(document.cookie).get("plan_type");n.each(function(){var t=this;r.smartsheetSignup.addUrlCodes(t),a?r.smartsheetSignup.redirectToApp(t):r.smartsheetSignup.changeLinkDestination(t)})}},r.smartsheetSignup=r.smartsheetSignup||{};var e=function(t,e){return-1<i.inArray(e.pathname,r.smartsheetSignup.pages)},a=function(t,e){try{if(e.hostname==n.location.hostname)return!0;if(-1!=e.hostname.indexOf("smartsheet.com"))return!0}catch(t){}return!1};r.smartsheetSignup.findSignupLinks=function(){var t=r.settings.smartsheetSignup.pages;return r.smartsheetSignup.pages=i.map(t,function(t){return"/"+t}).concat(t),i("a").filter(a).filter(e)},r.smartsheetSignup.addUrlCodes=function(t){var e=i(t),n=i.deparam.querystring(),a=r.settings.smartsheetSignup.codes||{},s=i.extend({},a,n);e.querystring(s,1)},r.smartsheetSignup.changeLinkDestination=function(t){var e=r.settings.smartsheetSignup.cta||{};e.from&&e.to&&t.pathname==="/"+e.from&&(t.pathname="/"+e.to)},r.smartsheetSignup.redirectToApp=function(t){t.protocol="https:",t.hostname="app.smartsheet.com",t.pathname="b/launch",t.dataset.launchInApp&&(t.text=t.dataset.launchInApp)}}(jQuery,window,Drupal);;
var Smartsheet=Smartsheet||{},dataLayer=dataLayer||[];function smartsheet_captcha_initialize(){jQuery(document).ready(function(){var t,e=jQuery(".js-captcha-signup").once("signup"),i=jQuery(".js-captcha-signup-email-verification").once("signup");if(e.length||i.length){var n=Drupal.settings.smartsheet_module.grecaptcha.sitekey;if(n){for(t=0;t<e.length;t++)new Smartsheet.SignupForm(jQuery(e[t]),!1,n);for(t=0;t<i.length;t++)new Smartsheet.SignupForm(jQuery(i[t]),!0,n)}else console.log("Missing recaptcha sitekey.")}})}!function(n,t,e){e.SignupForm=function(t,e,i){this.$form=t,this.$container=this.getValidationMessageContainer(t),this.$input=this.$form.find(".email-address-input"),this.lang=n("html").attr("lang"),this.rejectISP=!!e,this.$form.keyup(n.proxy(this.keyupHandler,this)),i?(this.$form.submit(this.captchaValidate.bind(this)),this.renderCaptcha(i)):this.$form.submit(this.submitHandler.bind(this))},e.SignupForm.prototype.renderCaptcha=function(t){var e=this.$form.find("div.g-recaptcha");e.length?this.recaptchaContainer=e.get(0):(this.recaptchaContainer=n('<div class="recaptcha-container"></div>').appendTo(this.$form).get(0),e=n(this.recaptchaContainer));var i={};i.sitekey=t,i.badge=e.attr("data-badge")||"inline",i.callback=this.captchaSuccess.bind(this),i.size="invisible",this.captchaId=grecaptcha.render(this.recaptchaContainer,i)},e.SignupForm.prototype.validate=function(){var t=this.$input.val();return this.validateEmail(t)?this.rejectISP&&this.isISP(t)?(this.displayISPMessage(),!1):(this.closePopup(),this.$form.attr("action",this.getActionUrl(t)),!0):(this.displayValidationMessage(),!1)},e.SignupForm.prototype.captchaValidate=function(t){if(!this.skipValidation)return t.preventDefault(),this.validate()&&grecaptcha.execute(this.captchaId),!1},e.SignupForm.prototype.submitHandler=function(t){return!!this.validate()&&(this.reportSignupIntent(),!0)},e.SignupForm.prototype.captchaSuccess=function(){this.reportSignupIntent(),this.skipValidation=!0,this.$form.submit()},e.SignupForm.prototype.keyupHandler=function(t){13!==t.which&&this.closePopup()},e.SignupForm.prototype.getActionUrl=function(t){var e,i="https://app.smartsheet.com/b/signup?lang="+this.lang+"&lc_explicit=true&email="+t;this.$form.attr("data-include-fields")&&(i+="&"+this.$form.find("input, select, textarea").not('[name^="g-"]').serialize());(i=this.addCodesFromURL(i,["lx","tg","sc"]),this.$form.attr("data-url"))&&(i+="&"+n.trim(this.$form.attr("data-url")));return(e=this.getURLParameter("trp"))?i+="&trp="+e:Drupal&&Drupal.settings&&Drupal.settings.smartsheetSignup&&Drupal.settings.smartsheetSignup.code&&(i+="&trp="+(e=Drupal.settings.smartsheetSignup.code)),i},e.SignupForm.prototype.validateEmail=function(t){return/\S+@\S+\.\S+/.test(t)},e.SignupForm.prototype.addCodesFromURL=function(t,e){for(var i=0;i<e.length;i++){var n=this.getURLParameter(e[i]);n&&(t+="&"+e[i]+"="+n)}return t},e.SignupForm.prototype.isISP=function(t){var e=t.match(/\S+@(\S+\.\S+)/)[1];return-1<["aol.com","att.net","comcast.net","facebook.com","gmail.com","gmx.com","googlemail.com","hotmail.com","hotmail.co.uk","mac.com","me.com","mail.com","msn.com","live.com","sbcglobal.net","verizon.net","yahoo.com","yahoo.co.uk","outlook.com","web.de","hotmail.de","yahoo.de","gmx.net","icloud.com","outlook.de","live.de","mail.ru","freenet.de","yahoo.fr","ymail.com"].indexOf(e)},e.SignupForm.prototype.getISPWarningContent=function(){var t=n(".isp-warning-message");return t.length?t.html():"de"===this.lang?"Bitte geben Sie Ihre Firmen-E-Mail-Adresse ein, um den kostenlosen Test zu beginnen.":"Please enter a work email address to access your free trial."},e.SignupForm.prototype.getValidationMessageContent=function(){switch(this.lang){case"fr":return"Veuillez entrer une adresse courriel valide.";case"it":return"Si prega di inserire un indirizzo email valido.";case"ru":return"Пожалуйста, введите действующий адрес электронной почты.";case"de":return"Bitte geben Sie eine gültige E - Mail - Adresse.";case"es":return"Por favor, introduce una dirección de email válida.";case"pt":return"Por favor insira um endereço de e-mail válido.";case"ja":return"メール アドレスを確認し、もう一度やり直してください。"}return"Please check your email address and try again."},e.SignupForm.prototype.getValidationMessageContainer=function(t){var e=t.find(".alert-message-email-validation-container");return e.length||(e=n('<div class="alert-message-email-validation-container"></div>').appendTo(t)),e},e.SignupForm.prototype.displayValidationMessage=function(){var t="";t+='<div id="alert-message-email-validation">',t+=this.getValidationMessageContent(),t+="</div>",this.showPopup(t)},e.SignupForm.prototype.displayISPMessage=function(t){var e='<div id="alert-message-email-validation">';e+=this.getISPWarningContent(),e+="</div>",this.showPopup(e)},e.SignupForm.prototype.showPopup=function(t){this.$container.html(t),this.$container.show()},e.SignupForm.prototype.getURLParameter=function(t){var e=(location.search.match(new RegExp("[?|&]"+t+"=(.+?)(&|$)"))||[,!1])[1];return e?decodeURIComponent(e):e},e.SignupForm.prototype.escapeHtml=function(t){var e={"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#39;","/":"&#x2F;"};return String(t).replace(/[&<>"'\/]/g,function(t){return e[t]})},e.SignupForm.prototype.getCloseButton=function(){return'<span class="close-button">X</span>'},e.SignupForm.prototype.closePopup=function(){this.$container.hide(),n("#alert-message-email-validation").remove()},e.SignupForm.prototype.reportSignupIntent=function(){t.push({event:"non-application-event",eventObject:"External Signup Screen","object-noun":"Form - Signup","object-string":"Sign up with an Email","object-type":"button","object-verb":"Click"})}}(jQuery,dataLayer,Smartsheet),function(i){i(document).ready(function(){i("html").attr("lang");var t,e=i(".js-signup-email-verification").once("signup");if(e.length)for(t=0;t<e.length;t++)new Smartsheet.SignupForm(i(e[t]),!0,!1);if((e=i(".js-signup").once("signup")).length)for(t=0;t<e.length;t++)new Smartsheet.SignupForm(i(e[t]),!1,!1);i("a.google-signup").once(function(){i(this).querystring(i.deparam.querystring())}),i("a.smartsheet-login").once(function(){i(this).querystring(i.deparam.querystring())})})}(jQuery);;
