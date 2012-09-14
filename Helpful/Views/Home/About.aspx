<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About Us
</asp:Content>
<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
<script type='text/javascript' src='http://www.google.com/jsapi'></script>
<link runat="server" href="http://doldev.opadev.dol.gov/w_helpful/Content/ui-lightness/jquery-ui-1.8.4.custom.css" rel="stylesheet" type="text/css" />
<script src="http://doldev.opadev.dol.gov/w_helpful/Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
<script src="http://doldev.opadev.dol.gov/w_helpful/Scripts/jquery-ui-1.8.4.custom.min.js" type="text/javascript"></script>
<h1>Url Feedback Report</h1>
    <script type='text/javascript'>
        
      google.load("visualization", "1", {packages:["corechart"]});
      $(document).ready(function (){ 
            DisplayData("<%= Server.UrlPathEncode( ViewData["url"].ToString() ) %>"); 
            
            $("#suggest").autocomplete({
                source: function(request, response) {
			        $.ajax({
				        url: "<%= Url.Action("Urls","Suggest") %>",
				        dataType: "json",
				        data: { urlPart: request.term },
				        success: function(data) {
				            //alert(data);
					        response($.map(data, function(item) {
					            var i = item.Url.replace("http://", "").substring(0,30);
						        return {
							        label: "Yes: " + item.Yes + " No: " + item.No + " " + i, 
							        value: item.Url
						        }
					        }))
				        }
			        })
			    },
		        select: function(event, ui) {
		            //ui.item ? ("Selected: " + ui.item.label) : "Nothing selected, input was " + this.value);
		            //DisplayData(ui.item.value);
		            location.search = "?Url=" + ui.item.value;
		            //this.value = "";
	            },
                minLength: 2
            });
      });
      
      function DisplayData(showUrl){
          $.getJSON("<%= Url.Action("Summery","Data") %>", { url: showUrl },
                function(data){
                    var data1 = new google.visualization.DataTable();
                    data1.addColumn('string', 'Task');
                    data1.addColumn('number', 'Number of Responses');
                    data1.addRows(5);
                    data1.setValue(0, 0, 'Yes');
                    data1.setValue(0, 1, data.Yes);
                    data1.setValue(1, 0, 'No');
                    data1.setValue(1, 1, data.No);

                    var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
                    chart.draw(data1, {width: 430, height: 240, title: 'User Responses'});

                    var commentDisplay = "";
                    for(comment in data.Comments)
                    {
                        commentDisplay += "<li>" + htmlEncode(data.Comments[comment].Comment) + "</li>";
                    }
                    $('#comments').empty().append(commentDisplay);
                    $('#url').empty().append("Showing URL: " + showUrl);
                });
        }

        function htmlEncode(value){ 
          return $('<div/>').text(value).html(); 
        } 
    </script>
    <style type="text/css">
		.ui-autocomplete-loading { background: white no-repeat right; }
		#city { width: 25em; }
	</style>
    <div class="ui-widget"><input type="text" id="suggest" />
    <div id="url"></div></div>
    <div id="chart_div"></div>
    Comments:
    <ul id="comments"></ul>
</asp:Content>
