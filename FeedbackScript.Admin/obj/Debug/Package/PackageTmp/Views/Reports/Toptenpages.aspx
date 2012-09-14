<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FeedbackScript.Admin.Models.TopPages>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Feedback Script - Rate This Page Feedback Results: 10 Most Rated, Highest Rated and Lowest Rated DOL Pages
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<a name="top"></a>
  <% using (Html.BeginForm())
     {
         string tDateFrom = string.Empty;
         string tDateTo = string.Empty;
         if (ViewData["txtDateFrom"] != null)
         {
             tDateFrom = ViewData["txtDateFrom"].ToString();
             tDateTo = ViewData["txtDateTo"].ToString();
         }
         %>
<h1>Rate This Page Feedback Results: 10 Most Rated, Highest Rated and Lowest Rated DOL Pages</h1>
<a name="top"></a>
<div  style="background-color:#FAFAEF;width:961px;text-align:left;height:100px" >
       <h2>Date range (mm/dd/yyyy):</h2>
		<div id="FromDateDiv">
            <%string ErrorMessage = Html.ValidationMessage("FromDateValidation").ToString();%>
                <%if (ErrorMessage.Contains("Invalid Date Range"))
                  { %><label class="errorMessage1"><%=ErrorMessage%></label>
                <%}%>
			<div><label class="labelFeedback" for="txtDateFrom">From:</label></div>
				<input type="text" id="txtDateFrom" value="<%=tDateFrom%>" name="txtDateFrom" class="DateFromText" title="Please enter a valid From Date" onkeypress="return NoSubmission()" onfocus="ClearDate();"/>
				<input type="image" alt="Select From date" value="aaa"  src="../../Images/calendar.gif" id="dateImgFrom" class="subscribeButton" onkeypress="return NoSubmission()"/>
                <div id="calcontainerFrom"></div>
	    </div>
	
        <div id="ToDateDiv">
        <% ErrorMessage = Html.ValidationMessage("ToDateValidation").ToString();%>
                <%if (ErrorMessage.Contains("Invalid Date Range"))
                  { %><label class="errorMessage1"><%=ErrorMessage%></label>
                <%}%>
			<div><label class="labelFeedback" for="txtDateTo">To:</label></div>
			<div>
				<input type="text" class="DateTo" value="<%=tDateTo %>" id="txtDateTo" name="txtDateTo" title="Please enter valid To Date"  onkeypress="return NoSubmission()" onfocus="ClearDate();"/>
				<input type="image" alt="Select To date" value="bbb" src="../../Images/calendar.gif" id="dateImgTo"  class="searchButton" onkeypress="return NoSubmission()" />
                <div id="calcontainerTo"></div>
			</div>  
        </div>

        <div>
            <input class="updateRankingButton" type="submit"  value="Update Ranking" id="updateranking" name="updateranking" onfocus="ClearDate();" />

        </div>
    </div>
    

<br />
<% if (Model != null)
   {
       ErrorMessage = Html.ValidationMessage("RecordCount").ToString();%>
            <% if (ErrorMessage.Contains("No results found for the specified search criteria"))
               { %>
            <label class="errorMessage1"><%=ErrorMessage%></label>
            <%}
               else
               {%><br />
<div id="Div1">
 <input type="submit" value="Export to Excel" id="Submit1" class="button" name="Export"/>
</div>

<br />

<h2>10 Most Rated Pages</h2>


    <table class="data" width="961px"  style="table-layout:fixed;WORD-BREAK:BREAK-ALL;">
    <col width="40px"/>
    <col width="791px"/>
    <col width="130px"/>
        <tr>
            <th align="center">Rank</th>
            <th align="center">Page URL</th>
            <th align="center">Volume of Feedback</th>
        </tr>

     <%int i = 0;
       var col = "";
       int rank = 0;
      %>
    <% 
       List<FeedbackScript.Admin.Models.TopPages> tten = Model.ToList();

       foreach (var item in tten.Where(t => t.VolumeFeedBack >= t.Mean).OrderByDescending(t => t.VolumeFeedBack).Take(10).ToList())
       {
           if (i == 1)
           {
               col = "background-color:#FAFAEF";
               i = 0;
           }
           else
           {
               col = "background-color:Normal";
               i = 1;
           }
           rank = rank + 1;
           %>
    
        <tr style="<%=col%>">
            <td>
                <%= rank%>
            </td>
            <td>
                <%= Html.Encode(item.URL)%>
            </td>
            <td>
                <%= Html.Encode(item.VolumeFeedBack)%>
            </td>
        </tr>
    <% } %>

    </table>
    <br />
    <br />
    
    <h2>10 Highest Rated Pages</h2>
    <table class="data" width="961px"  style="table-layout:fixed;WORD-BREAK:BREAK-ALL;">
    <col width="40px"/>
    <col width="756px"/>
    <col width="72px"/>
    <col width="88px"/>
        <tr>
            <th align="center">Rank</th>
            <th align="right">Page URL</th>
            <th align="center">Volume of Feedback</th>
            <th align="center">% of Positive Feedback</th>
        </tr>

     <%int j = 0;
       var colj = "";
       int rankj = 0;
      %>
    <% 

       foreach (var item1 in tten.Where(t => t.VolumeFeedBack >= t.Mean).OrderByDescending(t => t.Pfeedback).Take(10).ToList())
       {
           if (j == 1)
           {
               colj = "background-color:#FAFAEF";
               j = 0;
           }
           else
           {
               colj = "background-color:Normal";
               j = 1;
           }
           rankj = rankj + 1;
           %>
    
        <tr style="<%=colj%>">
            <td>
                <%= rankj%>
            </td>
            <td>
                <%= Html.Encode(item1.URL)%>
            </td>
            <td>
                <%= Html.Encode(item1.VolumeFeedBack)%>
            </td>
            <td>
                <%= Html.Encode(item1.Pfeedback)%>
            </td>
        </tr>
    <% } %>

    </table>

    <br /><br/>
    <h2>10 Lowest Rated Pages</h2>
    <table class="data" width="961px"  style="table-layout:fixed;WORD-BREAK:BREAK-ALL;" summary="This table provides the summary of Agency wise feedback report for selected date Range">
    <col width="40px"/>
    <col width="751px"/>
    <col width="72px"/>
    <col width="88px"/>
        <tr>
            <th align="center">Rank</th>
            <th align="center">Page URL</th>
            <th align="center">Volume of Feedback</th>
            <th align="center">% of Negative Feedback</th>
        </tr>

     <%int k = 0;
       var colk = "";
       int rankk = 0;
      %>
    <% 
       foreach (var item in tten.Where(t => t.VolumeFeedBack >= t.Mean).OrderByDescending(t => t.Nfeedback).Take(10).ToList())
       {
           if (k == 1)
           {
               colk = "background-color:#FAFAEF";
               k = 0;
           }
           else
           {
               colk = "background-color:Normal";
               k = 1;
           }
           rankk = rankk + 1;
           %>
    
        <tr style="<%=colk%>">
            <td scope="row">
                <%= rankk%>
            </td>
            <td>
                <%= Html.Encode(item.URL)%>
            </td>
            <td>
                <%= Html.Encode(item.VolumeFeedBack)%>
            </td>
            <td>
                <%= Html.Encode(item.Nfeedback)%>
            </td>
        </tr>
    <% } %>

    </table>
    	
    <div> 
        <a href="#top"><img alt="Back to top" src="../Images/backtop.gif" style="vertical-align:middle;border-color:White;"/>Back to Top</a>
    </div>


<%}

   }
     }%>

</asp:Content>