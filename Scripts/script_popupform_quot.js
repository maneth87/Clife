/* 
	author: istockphp.com
*/
jQuery(function($) {
	
    $("a.topopup").click(function () {
			loading(); // loading
			setTimeout(function(){ // then show popup, deley in .5 second
				loadPopup(); // function show popup 
			}, 500); // .5 second
	return false;
    });

    $("#btnprint").click(function () {
        loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup(); // function show popup 
        }, 500); // .5 second
        return false;
    });
    
    $("#btnnotfound").click(function () {
        loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopupNotFound(); // function show popup 
            disablePopup();// function hide popup 
        }, 500); // .5 second
        return false;
    });
  
    /* event for close the popup */
    $("div.closenotfound").hover(
                    function () {
                        $('span.ecs_tooltipnotfound').show();
                        popupStatusNotfound = 1;
                    },
                    function () {
                        $('span.ecs_tooltipnotfound').hide();
                        popupStatusNotfound = 1;
                    }
                );

    $("div.closenotfound").click(function () {        
        disablePopupNotFound();
        disablePopup();
        popupStatusNotfound = 1;        
        location.reload();
       
    });

	/* event for close the popup */
	$("div.close").hover(
					function() {
						$('span.ecs_tooltip').show();
					},
					function () {
    					$('span.ecs_tooltip').hide();
  					}
				);
	
	$("div.close").click(function() {
	    disablePopup();  // function close pop up	  
	    location.reload();
	});
	
	$(this).keyup(function(event) {
		if (event.which == 27) { // 27 is 'Ecs' in the keyboard
		    disablePopup();  // function close pop up
		    disablePopupNotFound();
		    location.reload();
		   
		}  	
	});
	
	$("div#backgroundPopup").click(function() {
	    disablePopup();  // function close pop up
	    disablePopupNotFound();
	});
	
	$('a.livebox').click(function() {
		alert('Hello World!');
	return false;
	});
	

	 /************** start: functions. **************/
	function loading() {
		$("div.loader").show();  
	}
	function closeloading() {
		$("div.loader").fadeOut('normal');  
	}
	
	var popupStatus = 0; // set value
	
	function loadPopup() { 
		if(popupStatus == 0) { // if value is 0, show popup
			//closeloading(); // fadeout loading
			$("#toPopup").fadeIn(0500); // fadein popup div
			$("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
			$("#backgroundPopup").fadeIn(0001); 
			popupStatus = 1; // and set value to 1
		}	
	}	
	function disablePopup() {
		if(popupStatus == 1) { // if value is 1, close popup
			$("#toPopup").fadeOut("normal");  
			$("#backgroundPopup").fadeOut("normal");  
			popupStatus = 0;  // and set value to 0			
			location.reload();
			 
		}
	}

    ///Function for Topup Notfound
	var popupStatusNotfound = 0; // set value

	function loadPopupNotFound() {
	    if (popupStatusNotfound == 0) { // if value is 0, show popup
	        //closeloading(); // fadeout loading
	        $("#topuprecordnotfound").fadeIn(0500); // fadein popup div
	        $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
	        $("#backgroundPopup").fadeIn(0001);
	        popupStatusNotfound = 1; // and set value to 1
	    }
	}
	function disablePopupNotFound() {
	    if (popupStatusNotfound == 1) { // if value is 1, close popup
	        $("#topuprecordnotfound").fadeOut("normal");
	        $("#backgroundPopup").fadeOut("normal");	        
	        popupStatusNotfound = 0;  // and set value to 0
	     
	        location.reload();
	    }
	}
	/************** end: functions. **************/
}); // jQuery End