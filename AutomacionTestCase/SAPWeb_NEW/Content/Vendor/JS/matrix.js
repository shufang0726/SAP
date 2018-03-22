
$(document).ready(function(){

    displayMenu();
	

	// === Sidebar navigation === //
	
	$('.submenu > a').click(function(e)
	{
		e.preventDefault();
		var submenu = $(this).siblings('ul');
		var li = $(this).parents('li');
		var submenus = $('#sidebar li.submenu ul');
		var submenus_parents = $('#sidebar li.submenu');
		if(li.hasClass('open'))
		{
			if(($(window).width() > 768) || ($(window).width() < 479)) {
				submenu.slideUp();
			} else {
				submenu.fadeOut(250);
			}
			li.removeClass('open');
		} else 
		{
			if(($(window).width() > 768) || ($(window).width() < 479)) {
				submenus.slideUp();			
				submenu.slideDown();
			} else {
				submenus.fadeOut(250);			
				submenu.fadeIn(250);
			}
			submenus_parents.removeClass('open');		
			li.addClass('open');
		}
		
	});
	
	var ul = $('#sidebar > ul');
	
	$('.mainmenu > a').click(function (e)
	{
		//e.preventDefault();
		var li = $(this).parents('li');
		var sidemenus_siblings = $('#sidebar li');
		if (sidemenus_siblings.hasClass('active')){
		    sidemenus_siblings.removeClass('active');
		}
		var submenus_parents = $('#sidebar li.submenu');
		if (submenus_parents.hasClass('open'))
		{
		    submenus_parents.removeClass('open');
		    var submenus = $('#sidebar li.submenu ul');
		    if (($(window).width() > 768) || ($(window).width() < 479)) {
		        submenus.slideUp();
		    } else {
		        submenus.fadeOut(250);
		    }
		}
		li.addClass('active');
		var bread = $(this);
		displayBreadcrumb(bread);
		return true;
	});

	$('.submenu > ul > li > a').click(function (e) {
	    var bread = $(this);
	    displayBreadcrumb(bread);
	    return true;
	});

	$('.sublevel > a').click(function (e)
	{
		//e.preventDefault();
		var li = $(this).parents('li');
		var sidemenus_siblings = $('#sidebar li');
		if (sidemenus_siblings.hasClass('active')){
		    sidemenus_siblings.removeClass('active');
		}
		var submenus_parents = $('#sidebar li.submenu');
		
		li.addClass('active');
		return true;
	});
	// === Resize window related === //
	$(window).resize(function()
	{
		if($(window).width() > 479)
		{
			ul.css({'display':'block'});	
			$('#content-header .btn-group').css({width:'auto'});		
		}
		if($(window).width() < 479)
		{
			ul.css({'display':'none'});
			fix_position();
		}
		if($(window).width() > 768)
		{
			$('#user-nav > ul').css({width:'auto',margin:'0'});
            $('#content-header .btn-group').css({width:'auto'});
		}
	});
	
	if($(window).width() < 468)
	{
		ul.css({'display':'none'});
		fix_position();
	}
	
	if($(window).width() > 479)
	{
	   $('#content-header .btn-group').css({width:'auto'});
		ul.css({'display':'block'});
	}
	
	// === Tooltips === //
	//$('.tip').tooltip();	
	//$('.tip-left').tooltip({ placement: 'left' });	
	//$('.tip-right').tooltip({ placement: 'right' });	
	//$('.tip-top').tooltip({ placement: 'top' });	
	//$('.tip-bottom').tooltip({ placement: 'bottom' });	
	
	// === Search input typeahead === //
	//$('#search input[type=text]').typeahead({
	//	source: ['Dashboard','Form elements','Common Elements','Validation','Wizard','Buttons','Icons','Interface elements','Support','Calendar','Gallery','Reports','Charts','Graphs','Widgets'],
	//	items: 4
	//});
	
	// === Fixes the position of buttons group in content header and top user navigation === //
	function fix_position()
	{
		var uwidth = $('#user-nav > ul').width();
		$('#user-nav > ul').css({width:uwidth,'margin-left':'-' + uwidth / 2 + 'px'});
        
        var cwidth = $('#content-header .btn-group').width();
        $('#content-header .btn-group').css({width:cwidth,'margin-left':'-' + uwidth / 2 + 'px'});
	}
	
	// === Style switcher === //
	$('#style-switcher i').click(function()
	{
		if($(this).hasClass('open'))
		{
			$(this).parent().animate({marginRight:'-=190'});
			$(this).removeClass('open');
		} else 
		{
			$(this).parent().animate({marginRight:'+=190'});
			$(this).addClass('open');
		}
		$(this).toggleClass('icon-arrow-left');
		$(this).toggleClass('icon-arrow-right');
	});
	
	$('#style-switcher a').click(function()
	{
		var style = $(this).attr('href').replace('#','');
		$('.skin-color').attr('href','css/maruti.'+style+'.css');
		$(this).siblings('a').css({'border-color':'transparent'});
		$(this).css({'border-color':'#aaaaaa'});
	});
	
	$('.lightbox_trigger').click(function(e) {
		
		e.preventDefault();
		
		var image_href = $(this).attr("href");
		
		if ($('#lightbox').length > 0) {
			
			$('#imgbox').html('<img src="' + image_href + '" /><p><i class="icon-remove icon-white"></i></p>');
		   	
			$('#lightbox').slideDown(500);
		}
		
		else { 
			var lightbox = 
			'<div id="lightbox" style="display:none;">' +
				'<div id="imgbox"><img src="' + image_href +'" />' + 
					'<p><i class="icon-remove icon-white"></i></p>' +
				'</div>' +	
			'</div>';
				
			$('body').append(lightbox);
			$('#lightbox').slideDown(500);
		}
		
	});
	

	$('#lightbox').on('click', function() { 
		$('#lightbox').hide(200);
	});
	
});

function displayMenu() {
    var hash = window.location.hash;
    var routeURL = hash.split("/")[1];
    var menu = "main";
    var submenu = "";
    switch (routeURL) {
        case "team":
        case "teamList":
            menu= "team";
            break;
        case "company":
        case "companyList":
            menu = "company";
            break;
        case "usermanagement":
        case "userList":
            menu = "usermanagement";
            break;
        case "management":
            menu = "management";
            break;
        case "matrix":
            menu = "matrix";
            break;
        case "report":
            break;
        default:
            menu = "main";
    }
    if (menu != "main") {
        var sidemenus_siblings = $('#sidebar li');
        if (sidemenus_siblings.hasClass('active')) {
            sidemenus_siblings.removeClass('active');
        }
        //var submenus_parents = $('#sidebar li.submenu');
        //if (submenus_parents.hasClass('open')) {
        //    submenus_parents.removeClass('open');
        //    var submenus = $('#sidebar li.submenu ul');
        //    if (($(window).width() > 768) || ($(window).width() < 479)) {
        //        submenus.slideUp();
        //    } else {
        //        submenus.fadeOut(250);
        //    }
        //}
        $("a[ui-sref$= " + menu + "]").parent().addClass("active");
    }
    
}

function displayBreadcrumb(bread) {
    $('#breadcrumb').find('.current').remove();
    var menuName = bread.find('span').context.innerText;
	$('#menuHome').after("<span class='current'>" + menuName + "</span>");
}
