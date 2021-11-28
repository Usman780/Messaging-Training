/*! Picturefill - v2.1.0 - 2014-10-07
* http://scottjehl.github.io/picturefill
* Copyright (c) 2014 https://github.com/scottjehl/picturefill/blob/master/Authors.txt; Licensed MIT */
window.matchMedia||(window.matchMedia=function(){"use strict";var a=window.styleMedia||window.media;if(!a){var b=document.createElement("style"),c=document.getElementsByTagName("script")[0],d=null;b.type="text/css",b.id="matchmediajs-test",c.parentNode.insertBefore(b,c),d="getComputedStyle"in window&&window.getComputedStyle(b,null)||b.currentStyle,a={matchMedium:function(a){var c="@media "+a+"{ #matchmediajs-test { width: 1px; } }";return b.styleSheet?b.styleSheet.cssText=c:b.textContent=c,"1px"===d.width}}}return function(b){return{matches:a.matchMedium(b||"all"),media:b||"all"}}}()),function(a,b,c){"use strict";function d(a){var b,c,d,e,g,h=a||{};b=h.elements||f.getAllElements();for(var i=0,j=b.length;j>i;i++)if(c=b[i],d=c.parentNode,e=void 0,g=void 0,"IMG"===c.nodeName.toUpperCase()&&(c[f.ns]||(c[f.ns]={}),h.reevaluate||!c[f.ns].evaluated)){if("PICTURE"===d.nodeName.toUpperCase()){if(f.removeVideoShim(d),e=f.getMatch(c,d),e===!1)continue}else e=void 0;("PICTURE"===d.nodeName.toUpperCase()||c.srcset&&!f.srcsetSupported||!f.sizesSupported&&c.srcset&&c.srcset.indexOf("w")>-1)&&f.dodgeSrcset(c),e?(g=f.processSourceSet(e),f.applyBestCandidate(g,c)):(g=f.processSourceSet(c),(void 0===c.srcset||c[f.ns].srcset)&&f.applyBestCandidate(g,c)),c[f.ns].evaluated=!0}}function e(){function c(){var b;a._picturefillWorking||(a._picturefillWorking=!0,a.clearTimeout(b),b=a.setTimeout(function(){d({reevaluate:!0}),a._picturefillWorking=!1},60))}d();var e=setInterval(function(){return d(),/^loaded|^i|^c/.test(b.readyState)?void clearInterval(e):void 0},250);a.addEventListener?a.addEventListener("resize",c,!1):a.attachEvent&&a.attachEvent("onresize",c)}if(a.HTMLPictureElement)return void(a.picturefill=function(){});b.createElement("picture");var f={};f.ns="picturefill",function(){f.srcsetSupported="srcset"in c,f.sizesSupported="sizes"in c}(),f.trim=function(a){return a.trim?a.trim():a.replace(/^\s+|\s+$/g,"")},f.endsWith=function(a,b){return a.endsWith?a.endsWith(b):-1!==a.indexOf(b,a.length-b.length)},f.restrictsMixedContent=function(){return"https:"===a.location.protocol},f.matchesMedia=function(b){return a.matchMedia&&a.matchMedia(b).matches},f.getDpr=function(){return a.devicePixelRatio||1},f.getWidthFromLength=function(a){a=a&&a.indexOf("%")>-1==!1&&(parseFloat(a)>0||a.indexOf("calc(")>-1)?a:"100vw",a=a.replace("vw","%"),f.lengthEl||(f.lengthEl=b.createElement("div"),f.lengthEl.style.cssText="border:0;display:block;font-size:1em;left:0;margin:0;padding:0;position:absolute;visibility:hidden"),f.lengthEl.style.width=a,b.body.appendChild(f.lengthEl),f.lengthEl.className="helper-from-picturefill-js",f.lengthEl.offsetWidth<=0&&(f.lengthEl.style.width=b.documentElement.offsetWidth+"px");var c=f.lengthEl.offsetWidth;return b.body.removeChild(f.lengthEl),c},f.types={},f.types["image/jpeg"]=!0,f.types["image/gif"]=!0,f.types["image/png"]=!0,f.types["image/svg+xml"]=b.implementation.hasFeature("http://www.w3.org/TR/SVG11/feature#Image","1.1"),f.types["image/webp"]=function(){var a="image/webp";c.onerror=function(){f.types[a]=!1,d()},c.onload=function(){f.types[a]=1===c.width,d()},c.src="data:image/webp;base64,UklGRh4AAABXRUJQVlA4TBEAAAAvAAAAAAfQ//73v/+BiOh/AAA="},f.verifyTypeSupport=function(a){var b=a.getAttribute("type");return null===b||""===b?!0:"function"==typeof f.types[b]?(f.types[b](),"pending"):f.types[b]},f.parseSize=function(a){var b=/(\([^)]+\))?\s*(.+)/g.exec(a);return{media:b&&b[1],length:b&&b[2]}},f.findWidthFromSourceSize=function(a){for(var b,c=f.trim(a).split(/\s*,\s*/),d=0,e=c.length;e>d;d++){var g=c[d],h=f.parseSize(g),i=h.length,j=h.media;if(i&&(!j||f.matchesMedia(j))){b=i;break}}return f.getWidthFromLength(b)},f.parseSrcset=function(a){for(var b=[];""!==a;){a=a.replace(/^\s+/g,"");var c,d=a.search(/\s/g),e=null;if(-1!==d){c=a.slice(0,d);var f=c.slice(-1);if((","===f||""===c)&&(c=c.replace(/,+$/,""),e=""),a=a.slice(d+1),null===e){var g=a.indexOf(",");-1!==g?(e=a.slice(0,g),a=a.slice(g+1)):(e=a,a="")}}else c=a,a="";(c||e)&&b.push({url:c,descriptor:e})}return b},f.parseDescriptor=function(a,b){var c,d=b||"100vw",e=a&&a.replace(/(^\s+|\s+$)/g,""),g=f.findWidthFromSourceSize(d);if(e)for(var h=e.split(" "),i=h.length-1;i>=0;i--){var j=h[i],k=j&&j.slice(j.length-1);if("h"!==k&&"w"!==k||f.sizesSupported){if("x"===k){var l=j&&parseFloat(j,10);c=l&&!isNaN(l)?l:1}}else c=parseFloat(parseInt(j,10)/g)}return c||1},f.getCandidatesFromSourceSet=function(a,b){for(var c=f.parseSrcset(a),d=[],e=0,g=c.length;g>e;e++){var h=c[e];d.push({url:h.url,resolution:f.parseDescriptor(h.descriptor,b)})}return d},f.dodgeSrcset=function(a){a.srcset&&(a[f.ns].srcset=a.srcset,a.removeAttribute("srcset"))},f.processSourceSet=function(a){var b=a.getAttribute("srcset"),c=a.getAttribute("sizes"),d=[];return"IMG"===a.nodeName.toUpperCase()&&a[f.ns]&&a[f.ns].srcset&&(b=a[f.ns].srcset),b&&(d=f.getCandidatesFromSourceSet(b,c)),d},f.applyBestCandidate=function(a,b){var c,d,e;a.sort(f.ascendingSort),d=a.length,e=a[d-1];for(var g=0;d>g;g++)if(c=a[g],c.resolution>=f.getDpr()){e=c;break}if(e&&!f.endsWith(b.src,e.url))if(f.restrictsMixedContent()&&"http:"===e.url.substr(0,"http:".length).toLowerCase())void 0!==typeof console&&console.warn("Blocked mixed content image "+e.url);else{b.src=e.url,b.currentSrc=b.src;var h=b.style||{},i="webkitBackfaceVisibility"in h,j=h.zoom;i&&(h.zoom=".999",i=b.offsetWidth,h.zoom=j)}},f.ascendingSort=function(a,b){return a.resolution-b.resolution},f.removeVideoShim=function(a){var b=a.getElementsByTagName("video");if(b.length){for(var c=b[0],d=c.getElementsByTagName("source");d.length;)a.insertBefore(d[0],c);c.parentNode.removeChild(c)}},f.getAllElements=function(){for(var a=[],c=b.getElementsByTagName("img"),d=0,e=c.length;e>d;d++){var g=c[d];("PICTURE"===g.parentNode.nodeName.toUpperCase()||null!==g.getAttribute("srcset")||g[f.ns]&&null!==g[f.ns].srcset)&&a.push(g)}return a},f.getMatch=function(a,b){for(var c,d=b.childNodes,e=0,g=d.length;g>e;e++){var h=d[e];if(1===h.nodeType){if(h===a)return c;if("SOURCE"===h.nodeName.toUpperCase()){null!==h.getAttribute("src")&&void 0!==typeof console&&console.warn("The `src` attribute is invalid on `picture` `source` element; instead, use `srcset`.");var i=h.getAttribute("media");if(h.getAttribute("srcset")&&(!i||f.matchesMedia(i))){var j=f.verifyTypeSupport(h);if(j===!0){c=h;break}if("pending"===j)return!1}}}}return c},e(),d._=f,"object"==typeof module&&"object"==typeof module.exports?module.exports=d:"function"==typeof define&&define.amd?define(function(){return d}):"object"==typeof a&&(a.picturefill=d)}(this,this.document,new this.Image);

;
if(typeof Drupal!=="undefined"&&typeof jQuery!=="undefined"){(function($){Drupal.behaviors.picture={attach:function(context){("HTMLPictureElement" in window)||window.picturefill($(context));if(context==="#cboxLoadedContent"&&$(context).find("picture").length){$.colorbox.resize();$("img",context).once("colorbox-lazy-load",function(){$(this).load(function(){this.style.maxHeight=$(window).height()+"px";this.style.maxWidth=$(window).width()+"px";$.colorbox.resize({innerHeight:this.height,innerWidth:this.width});this.style.maxHeight=null;this.style.maxWidth=null;});});}}};})(jQuery);};
/**
 * @file
 * Handles ajax functionalities for Ajax Links API module.
 */
(function ($) {
  var ajaxLoading = false;
  Drupal.behaviors.ajaxLinksApi = {
    attach: function () {
      var trigger = Drupal.settings.ajax_links_api.trigger,
        negativeTrigger = Drupal.settings.ajax_links_api.negative_triggers;

      // match the elements from the positive selector
      var $elements = $(trigger);

      // remove elements if the negative trigger is specified
      if (negativeTrigger) {
        $elements = $elements.not(negativeTrigger);
      }

      // add the click handler
      $elements.click(function(e) {
        e.preventDefault();
        var selector;
        if(!ajaxLoading) {
          ajaxLoading = true;
          var url = $(this).attr("href");
          var id = $(this).attr("rel");
          if(id) {
            selector = $(this).attr("rel");
          } else {
            selector = Drupal.settings.ajax_links_api.selector;
          }
          ajaxBefore(selector);
          ajaxLink(selector, url);          
        }
      });
    }
  };
  function  ajaxLink(selector, url) {
    $.ajax({
      url: url,
      type: "GET",
      data: "ajax=1",
      success: function (data) {        
        ajaxAfter(selector, url, data, window, document);
        Drupal.attachBehaviors(selector);
      },
      error: function (xhr) {
        var data = xhr.response.replace("?ajax=1", "");
        ajaxAfter(selector, url, data, window, document);
      }
    });
  }
  function ajaxBefore(selector){
    // Preserve the height of the current content to avoid the entire page
    // collapsing.
    $(selector).css('height', $(selector).height() + 'px');

    // Replace the content with a throbber.
    $(selector).html("<div class='ajax-links-api-loading'></div>");
  }
  function ajaxAfter(selector, url, data, window, document){    
    // Reset the height of the container.
    $(selector).css('height', '');

    // Replace the contents of the container with the data.
    $(selector).html(data);

    // Update active class.
    $('a.active').removeClass('active').parents('li').removeClass('active-trail');
    $('a').filter(function() {
      return $(this).attr('href')== url
    }).addClass('active').parents('li').addClass('active-trail');

    // Change Url if option is selected and for html5 compatible browsers.
    var html5 = Drupal.settings.ajax_links_api.html5;
    if(html5 == 1 && window.history.replaceState) {
      // get title of loaded content.
      var matches = data.match("<title>(.*?)</title>");
      if (matches) {
        // Decode any HTML entities.
        var title = $('<div/>').html(matches[1]).text();
        // Since title is not changing with window.history.pushState(),
        // manually change title. Possible bug with browsers.
        document.title = title;
      }
      // store current url.
      window.history.replaceState({page : 0} , document.title, window.location.href);
      // Change url.
      window.history.pushState({page : 1} , title, url);
      window.onpopstate = function (e) {
        window.history.go(0);
      };
    }

    // Views Pager.
    // Please check http://drupal.org/node/1907376 for details.
    var viewsPager = Drupal.settings.ajax_links_api.vpager;
    if(viewsPager == 1) {
      $(selector + " .view .pager a").each(function(){
        var href = $(this).attr('href');
        href = href.replace("?ajax=1", "");
        href = href.replace("&ajax=1", "");
        $(this).attr('href', href);
      });
    }

    // Form Validation.
    // Plese check http://drupal.org/node/1760414 for details.
    var formAction = $(selector + " form").attr('action');
    if (formAction) {
      formAction = formAction.replace("?ajax=1", "");
      $("form").attr('action', formAction);
    }    
  }
})(jQuery);
;
(function ($) {
  Drupal.behaviors.gssAutocomplete = {
    attach: function(context, settings) {
      if (settings.gss.key == undefined) {
        return;
      }

      $('.block-search .form-item-search-block-form input.form-text, .gss .form-item-keys input.form-text, .block-search .form-search input.form-text')
        .focus(function () {
          this.select();
        })
        .mouseup(function (e) {
          e.preventDefault();
        })
        .autocomplete({
          position: {
            my: "left top",
            at: "left bottom",
            offset: "0, 5",
            collision: "none"
          },
          source: function (request, response) {
            $.ajax({
              url: "http://clients1.google.com/complete/search?q=" + request.term + "&hl=en&client=partner&source=gcsc&partnerid=" + settings.gss.key + "&ds=cse&nocache=" + Math.random().toString(),
              dataType: "jsonp",
              success: function (data) {
                response($.map(data[1], function (item) {
                  return {
                    label: item[0],
                    value: item[0]
                  };
                }));
              }
            });
          },
          autoFill: true,
          minChars: 0,
          select: function (event, ui) {
            $(this).closest('input').val(ui.item.value);
            $(this).closest('form').trigger('submit');
          }
        });
    }
  };
}(jQuery));
;
(function($) {
  Drupal.behaviors.marketo_ma = {
    attach: function(context, settings) {
      if (typeof settings.marketo_ma !== 'undefined' && settings.marketo_ma.track) {
        jQuery.ajax({
          url: document.location.protocol + settings.marketo_ma.library,
          dataType: 'script',
          cache: true,
          success: function() {
            Munchkin.init(settings.marketo_ma.key);
            if (typeof settings.marketo_ma.actions !== 'undefined') {
              jQuery.each(settings.marketo_ma.actions, function() {
                Drupal.behaviors.marketo_ma.marketoMunchkinFunction(this.action, this.data, this.hash);
              });
            }
          }
        });
      }
    },
    marketoMunchkinFunction: function(leadType, data, hash) {
      mktoMunchkinFunction(leadType, data, hash);
    }
  };

})(jQuery);
;
var Drupal = Drupal || {};
var dataLayer = dataLayer || {};
var Smartsheet = Smartsheet || {};

(function ($, Drupal, dataLayer, Smartsheet) {

  // ensures the optimizely object is defined globally using
  window['optimizely'] = window['optimizely'] || [];

  Drupal.behaviors = Drupal.behaviors || {};

  /**
   * Adds GA dataLayer tracking to webforms that use webform_ajax.
   */
  Drupal.behaviors.smartsheetWebformTracking = {};

  Drupal.behaviors.smartsheetWebformTracking.attach = function (context, settings) {
    var $context = $(context);

    $context.find('form.webform-client-form').once('datalayer-tracking', function () {
      var $wrapper = getWrapper(this);
      var formId = getFormId($wrapper);
      if (formId) {
        // Registers a new form impression every time the form is loaded.
        dataLayer.push({
            'event': 'marketo-form-impression',
            'marketo-form-id' : formId
          }
        );
        // Adds a change handler to report field completion.
        $wrapper.find('input').change(function (e) {
          dataLayer.push({
            'event': 'marketo-form-field-complete',
            'marketo-form-field': formId
          });
        });
      }
    });

    $(context).find('.webform-confirmation').once('datalayer-tracking', function () {
      var $wrapper = getWrapper(this);
      var formId = getFormId($wrapper);
      if (formId) {
        // Reports successful form submission.
        dataLayer.push({
          'event': 'marketo-form-submit-success',
          'marketo-form-id': formId
        });
        // sends a form_success custom tracking event to Optimizely for the given event name.
         window.optimizely.push(["trackEvent", "form_success"]);
      }
    });

    $(context).find('.messages.error').once('datalayer-tracking', function () {
      var $wrapper = getWrapper(this);
      var formId = getFormId($wrapper);


      if (formId) {
        var error = $(this).not('element-invisible').text();

        // Reports a validation error on form submission.
        dataLayer.push({
          'event': 'marketo-form-submit-failed',
          'marketo-form-id': formId,
          'validation-error': error
        });

        // sends a form_fail custom tracking event to Optimizely for the given event name.
        // Collect field errors
        $('.datalayer-tracking-processed ul li').each(function (index, value) {
          //otimizely dimension value has a 20 char string limit
          var fieldError = $(this).html().substring(0, 20);
          window.optimizely.push(['setDimensionValue', 'failed_contact_fields', fieldError]);
          window.optimizely.push(["trackEvent", "form_fail"]);
        });

      }

    });

    /**
     * Returns the webform ajax wrapper for the given element, if any.
     *
     * @param {HTMLElement} elem
     * @returns jQuery
     */
    function getWrapper(elem) {
      return $(elem).parents('div').filter(function(){
        return (this.id && this.id.match(/webform-ajax-wrapper-[0-9]+$/));
      });
    }

    /**
     * Returns the webform node ID corresponding to a webform ajax wrapper.
     *
     * @param {jQuery} $wrapper
     * @returns {string|bool}
     *   The form ID as a string, or false if none.
     */
    function getFormId($wrapper) {
      if (!$wrapper || !$wrapper.length || !$wrapper[0].id) {
        return false;
      }
      return $wrapper.attr('id').match(/[0-9]+$/)[0];
    }

  };

})(jQuery, Drupal, dataLayer, Smartsheet);
;
var Drupal = Drupal || {};
var Smartsheet = Smartsheet || {};

(function ($, Drupal, Smartsheet) {

  /**
   * Drupal behaviors integration for the Smartsheet.videoTracking object.
   */
  Drupal.behaviors = Drupal.behaviors || {};
  Drupal.behaviors.smartsheetVideoTracking = {};
  Drupal.behaviors.smartsheetVideoTracking.attach = function (context, settings) {

    var $iframes = $('iframe.media-youtube-player').once('ga-video-tracking');
    if ($iframes.length) {
      // Load the YouTube iframe API JS, if it isn't already loaded.
      var tag = document.createElement('script');
      tag.src = "https://www.youtube.com/iframe_api";
      var firstScriptTag = document.getElementsByTagName('script')[0];
      firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

      // Create a new player object for each iframe.
      $iframes.each(function () {
        new Smartsheet.VideoTracking(this);
      });
    }
  };

  /**
   * Handles YouTube video tracking via Google Analytics.
   *
   * @param {HTMLElement} iframe
   *   The iframe of the YouTube player.
   */
  Smartsheet.VideoTracking = function (iframe) {
    this.iframe = iframe;
    this.player = this.createPlayer();
  };

  /**
   * Instantiates a YT Player object for the current iframe.
   *
   * @returns {YT.Player}
   */
  Smartsheet.VideoTracking.prototype.createPlayer = function () {
    if (Smartsheet.VideoTracking.apiReady) {
      this.player = new YT.Player(this.iframe, {
        events: {
          onReady: this.handleReady.bind(this),
          onStateChange: window.onPlayerStateChange
        }
      });
      return this.player;
    }
    else {
      // If the YouTube iFrame API isn't ready yet, try every two seconds to
      // run this same method again.
      window.setTimeout(this.createPlayer.bind(this), 2000);
    }
  };

  /**
   * Callback for the YouTube iframe API's onReady event.
   */
  Smartsheet.VideoTracking.prototype.handleReady = function () {
    window.registerYouTubePlayer(this.player);
  };

  /**
   * Global function required for Analytics Pros integration.
   */
  window.onPlayerStateChange = function (e) {};

  /**
   * Callback executed by the YouTube iframe API when the API is available.
   */
  window.onYouTubeIframeAPIReady = function () {
    Smartsheet.VideoTracking.apiReady = true;
  };


})(jQuery, Drupal, Smartsheet);
;
