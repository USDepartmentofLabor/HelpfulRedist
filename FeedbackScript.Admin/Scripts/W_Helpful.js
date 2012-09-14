var W_Helpful = (function () {
    //console.log("Define: W_Helpful");
    var TRACKER_URL = 'http://doldev.opadev.dol.gov/w_helpful/Data.mvc/Log';
    var Display_ = '<div id="W_Helpful" style="flaot:right">'
		+ '<input type="hidden" id="ResponseYesNo" name="ResponseYesNo" value=""/>'

		+ '<div id="W_Helpful_question_Div"><div class="W_Helpful_message">'
		+ '<img src="/images/page-rating-star.gif" alt="Feedback Star" id="W_Helpful_Star" />&nbsp'
		+ '<a  href="javascript:void(0)" id="W_Helpful_question_Link" title="Click here to give your feedback about this page">Was this page helpful?</a></div></div>'

                + '<div id="W_Helpful_yesORno"  style="display:none;">'

                + '<div id="W_Helpful_yes"><center><input type="checkbox" id="W_Helpful_accurate-yes" style="vertical-align:middle"/>'
		+ '<label style="font-size:12px;margin-top:-10px" for="W_Helpful_accurate-yes">Yes</label>&nbsp&nbsp</center></div>'

                + '<div id="W_Helpful_no"><center><input type="checkbox" id="W_Helpful_accurate-no" style="vertical-align:middle"/>'
		+ '<label style="font-size:12px;margin-top:-10px" for="W_Helpful_accurate-no">No</label>&nbsp&nbsp</center></div>'
               
		+ '</div>'

                + '<form>'
                + '<div id="W_Helpful_comment" style="display:none;">'
                + '<div class="W_Helpful_message" id="W_Helpful_message"><p><center>How can we make better? <br />(250 Character max.)</center></p></div><br/><textarea id="W_Helpful_helpful-input"></textarea>'
                + '<br/><br/><center><input id="W_Helpful_comment-submit" type="button" value="Submit"/>&nbsp&nbsp<input id="W_Helpful_comment-skip" type="button" value="Skip"/></center>'
                + '<br/><center><hr style="width=150px"/></center><center><label style=font-size:12px>We are collecting this info under OMB clearance number xxxx</label></center></p>'
                + '</div>'
                + '</form>'

		+ '<div id="W_Helpful_confirm" style="display:none;">'
                + '<label style=font-size:12px><center><strong>Thank you for your feedback!</strong></center><br/>'
		+ '<center>Please <a href="http://www.dol.gov/dol/contact/" style="text-decoration:underline">Contact US</a> if you have any other comments or questions!</center></label><br/>'
		+ '<div id="W_Helpful_Close-Confirmation"><center><label style=font-size:12px>x <a href="javascript:document.url" id="w_Helpful_Close" style="text-decoration:underline">Close</a></label></center></div>'
                + '</div>'

                + '</div>';
    var Display_UI = function (prependTo, after) {
        if (after) {
            $(prependTo).prepend(Display_);
        } else {
            $(prependTo).append(Display_);
        }
        $('#W_Helpful').show();
    }

    return {
        init: function (prependTo) {
            Display_UI(prependTo);

		$('#W_Helpful_helpful-input').live('keyup', function(){

			var limit = 250
			var text = $('#W_Helpful_helpful-input').val();
			var chars = text.length;

			if(chars > limit){
				var new_text = text.substr(0, limit);
				$('#W_Helpful_helpful-input').val(new_text);
			}
		});


            $('#W_Helpful_Close-Confirmation').live('click', function () {
		$('#W_Helpful_question_Div').removeAttr("style");
		var cssObj1 = {
     			'cursor':'pointer',
			'width':'170px',
			'font-size':'12px'
		}
		$('#W_Helpful_question_Div').css(cssObj1);

                $('#W_Helpful_helpful-input').val('');
                $('#W_Helpful_accurate-yes').removeAttr('checked');
                $('#W_Helpful_accurate-no').removeAttr('checked');
                W_Helpful.Hide('#W_Helpful_confirm');
            });

            $('#W_Helpful_comment-skip').live('click', function () {
		W_Helpful.Show('#W_Helpful_confirm');
                W_Helpful.Hide('#W_Helpful_comment');
		setTimeout(function() { $('#w_Helpful_Close').focus(); }, 1000);
            });

		$("#W_Helpful_question_Link").bind( "click keydown", function (evt) {
     		//begin = document.cookie.indexOf("ThisPageHasBeenReviewed=");
                //if (begin == -1) {
		//event.preventDefault();
		var charCode = (evt.which) ? evt.which : evt.keyCode
		//alert(charCode)
		if (charCode == '13' || charCode  == '0' || charCode == '1') 
		{
	                document.cookie = "ThisPageHasBeenReviewed=Yes";
			var cssObj3 = {
				'height':'20px',
     				'cursor':'pointer',
				'width':'170px',
				'font-size':'12px',
				'border': '1px solid #D8D5D4',
				'background-color':'#FAF8CC'
			}
	
			$('#W_Helpful_question_Div').css(cssObj3); 

	                W_Helpful.Show('#W_Helpful_yesORno');

	                //$('#W_Helpful_accurate-yes').focus();
			setTimeout(function() { $('#W_Helpful_accurate-yes').focus(); }, 1000);
		}
		
               //}
            });


            $('#W_Helpful_accurate-yes').bind( "click keydown", function (evt) {
		var charCode = (evt.which) ? evt.which : evt.keyCode
		if (charCode == '13' || charCode  == '0' || charCode == '1') 
		{
	                //console.log("onClick:", this.id);
	                $('#ResponseYesNo').val('yes');
	                W_Helpful.Hide('#W_Helpful_yesORno');
	                $("#W_Helpful_message").html("<center><label style=font-size:12px><strong>Great, can we make it even better?</strong><br />(250 Character max.)</label></center>");
	                //$('#W_Helpful_message').replaceWith('<p><center>Great, can we make it even better?<br />(250 Character max.)</center></p>');
	                W_Helpful.TakeResponse('yes', '', 'q');
        	        //W_Helpful.Hide('#W_Helpful_question_Div');
	                W_Helpful.Show('#W_Helpful_comment');
			//$('#W_Helpful_helpful-input').focus();
			//$('#W_Helpful_helpful-input').focus();
			setTimeout(function() { $('#W_Helpful_helpful-input').focus(); }, 1000);
		}

            });


            $('#W_Helpful_accurate-no').bind( "click keydown", function (evt) {
		var charCode = (evt.which) ? evt.which : evt.keyCode
		if (charCode == '13' || charCode  == '0' || charCode == '1') 
		{
                	$("#W_Helpful_message").html("<center><label style=font-size:12px><strong>How can we make it better?</strong> <br />(250 Character max.)</label></center>");
	                $('#ResponseYesNo').val('no');
	                //console.log("onClick:", this.id);
	                W_Helpful.Hide('#W_Helpful_yesORno');
	                W_Helpful.TakeResponse('no', '', 'q');
	                //W_Helpful.Hide('#W_Helpful_question_Div');
	                W_Helpful.Show('#W_Helpful_comment');
	                //$('#W_Helpful_helpful-input').focus();
			setTimeout(function() { $('#W_Helpful_helpful-input').focus(); }, 1000);
		}
            });


            $('#W_Helpful_comment-submit').live('click', function () {
                //console.log("onClick:", this.id);
		if (jQuery.trim($('#W_Helpful_helpful-input').val()) != '')
	                W_Helpful.TakeResponse($('#ResponseYesNo').val(), $('#W_Helpful_helpful-input').val(), 'c');
                W_Helpful.Hide('#W_Helpful_comment');
                W_Helpful.Show('#W_Helpful_confirm');
		//$('#w_Helpful_Close').focus();
		setTimeout(function() { $('#w_Helpful_Close').focus(); }, 1000);

		

            });
        },
        TakeResponse: function (quest, commt, PostType) {
            var logTo = TRACKER_URL;
            //console.log("W_Helpful.TRACKER_URL:", W_Helpful.TRACKER_URL);
            //console.log("TRACKER_URL:", TRACKER_URL);
            //console.log("T URL:", logTo);
            var pageUrl = document.URL;
            //console.log("Page URL:", pageUrl);
            //Eric discussion for cross site
            if (typeof XDomainRequest != 'undefined') {
                //alert("?dolurl=" + pageUrl + "&question=" + quest + "&suggestion=" + commt + "&posttype=" + PostType);
                var xdr = new XDomainRequest();
                xdr.open("GET", logTo + "?question=" + quest + "&suggestion=" + commt + "&posttype=" + PostType + "&dolurl=" + pageUrl);
                xdr.send();
                xdr.onload = function () {
                    //alert('test');
                }
            } else {
                $.getJSON(logTo, { dolurl: pageUrl, question: quest, suggestion: commt, posttype: PostType });
            }
        },
        Hide: function (selector) {
            $(selector).slideUp('slow');
        },

        Show: function (selector) {
            $(selector).slideDown('slow');
        }
    }
})();