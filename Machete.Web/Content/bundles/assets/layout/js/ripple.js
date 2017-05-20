$(function() {     

    var ink, d, x, y;
    $(document.body).off('mousedown.ripple','.ripplelink,.ui-button:enabled')
            .on('mousedown.ripple','.ripplelink,.ui-button:enabled', null, function(e) {
        var element = $(this);
        
        if(element.find(".ink").length === 0){
            if(element.hasClass('ripplelink'))
                element.children('span').after("<span class='ink'></span>");
            else
                element.append("<span class='ink'></span>");
        }
             
        ink = $(this).find(".ink");
        ink.removeClass("ripple-animate");
                 
        if(!ink.height() && !ink.width()){
            d = Math.max($(this).outerWidth(), $(this).outerHeight());
            ink.css({height: d, width: d});
        }
         
        x = e.pageX - $(this).offset().left - ink.width()/2;
        y = e.pageY - $(this).offset().top - ink.height()/2;
         
        ink.css({top: y+'px', left: x+'px', 'pointer-events': 'none'}).addClass("ripple-animate");
    });
});