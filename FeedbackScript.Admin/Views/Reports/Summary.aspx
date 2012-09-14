<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FeedbackScript.Admin.Models.TopPages>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Feedback Script - Rate This Page Feedback Results: Summary by Agency
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<A NAME="top">
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
<h1>Rate This Page Feedback Results: Summary by Agency</h1>
<A NAME="top">
<div  style="background-color:#FAFAEF;width:961px;text-align:left;height:100px" >
<%string ErrorMessage = Html.ValidationMessage("FromDateValidation").ToString();%>
    <%if (ErrorMessage.Contains("Invalid Date Range"))
      { %><span class="errorMessage1"><%=ErrorMessage%></span>
    <%}
      else
      {
        ErrorMessage = Html.ValidationMessage("ToDateValidation").ToString();%>
        <%if (ErrorMessage.Contains("Invalid Date Range"))
          { %><span class="errorMessage1"><%=ErrorMessage%></span>
        <%}
      }%>
<h2>Date range (mm/dd/yyyy):</h2>
		<div id="FromDateDiv">
			<div><label class="labelFeedback" for="txtDateFrom">From:</label></div>
            <div>
				<input type="text" id="txtDateFrom" value="<%= Html.Encode(tDateFrom) %>" name="txtDateFrom" class="DateFromText"  onkeypress="return NoSubmission()" onfocus="ClearDate();"/>
                <div id="calcontainerFrom"></div>
            </div>
	    </div>
	
        <div id="ToDateDiv">
			<div><label class="labelFeedback" for="txtDateTo">To:</label></div>
			<div>
				<input type="text" class="DateTo" value="<%= Html.Encode(tDateTo) %>" id="txtDateTo" name="txtDateTo"  onkeypress="return NoSubmission()" onfocus="ClearDate();"/>
                <div id="calcontainerTo"></div>
			</div>
        </div>

        <div>
            <input class="updateRankingButton" type="submit"  value="Update Summary" id="updateranking" name="updateranking"/>
        </div>
    </div>

<br />

 <%if (Model != null)
   {
       ErrorMessage = Html.ValidationMessage("RecordCount").ToString();%>
            <% if (ErrorMessage.Contains("No results found for the specified search criteria"))
               { %>
            <span class="errorMessage1"><%=ErrorMessage%></span><br />
            <%}
               else
               {%><br />
<div id="Div1">
 <input type="submit" value="Export to Excel" id="Submit1" class="button" name="Export"/>
</div>

<br />

    <table class="data" width="961px" summary="This table provides the summary of Top ten pages for selected date Range">
        <tr >
            <% string SortBy = ViewData["SortBy"].ToString();
               string Order = ViewData["Order"].ToString(); 
            %>
            <th>
                <% if (SortBy == "AgencyName" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("Agency", "Summary", new { SortBy = "AgencyName", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo })%><img id="Img4" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("Agency", "Summary", new { SortBy = "AgencyName", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo })%>
                    <%if (SortBy == "AgencyName")
                      {%>
                    <img id="Img6" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%}%> 
                  

            </th>
            <th class="dataright">
                <% if (SortBy == "VolumeOfFeedback" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("Volume of Feedback", "Summary", new { SortBy = "VolumeOfFeedback", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo })%><img id="Img2" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("Volume of Feedback", "Summary", new { SortBy = "VolumeOfFeedback", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo })%>
                <%if (SortBy == "VolumeOfFeedback")
                  {%>
                    <img id="Img1" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>                 
            </th>
            <th class="dataright">
                <% if (SortBy == "PositiveFeedback" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("% of Positive Feedback", "Summary", new { SortBy = "PositiveFeedback", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo })%><img id="Img5" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("% of Positive Feedback", "Summary", new { SortBy = "PositiveFeedback", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo })%>
                <%if (SortBy == "PositiveFeedback")
                  {%>
                    <img id="Img3" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>
            </th>
        </tr>

     <%int i = 0;
       var col = "";
       int rank = 0;
      %>
    <% 

       foreach (var item in Model)
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
            <td  scope="row">
                <%= Html.ActionLink(item.Agency, "Search", new { txtDateFrom = tDateFrom, txtDateTo = tDateTo, txtURLContains = "", SelectedAgency = item.Agency })%>
            </td>
            <td class="dataright">
                <%= Html.Encode(item.VolumeFeedBack)%>
            </td>
            <td class="dataright">
                <%= string.Format("{0:0.00}", Convert.ToDecimal(Html.Encode(item.Pfeedback)))%>
            </td>
        </tr>
    <% } %>

    </table>
    <br />
    <br />
    
    <div> 
        <a href="#top"><img alt="Back to top" src="/Images/backtop.gif" style="vertical-align:middle;border-color:White;"/>Back to Top</a>
    </div>


<%}
   }
     } %>

</asp:Content>