<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FeedbackScript.Admin.Models.TopPages>>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Feedback Script - Rate This Page Feedback Results: Feedback Trends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<a name="top"></a>
  <% using (Html.BeginForm())
     {
         string tDateFrom = string.Empty;
         string tDateTo = string.Empty;
         string uContains = string.Empty;
         string sAgency = string.Empty;
         string sUnit = string.Empty;
         string sUnitName = string.Empty;
         if (ViewData["txtDateFrom"] != null)
         {
             tDateFrom = ViewData["txtDateFrom"].ToString();
             tDateTo = ViewData["txtDateTo"].ToString();
         }
         if (ViewData["urlContains"] != "")
         {
             uContains = ViewData["urlContains"].ToString();
         }
         else
         {
             uContains = "Enter Text";
         }
         SelectList ss = (SelectList)ViewData["AgencyList"];
         sAgency = ss.SelectedValue.ToString();
         SelectList mm = (SelectList)ViewData["UnitList"];
         sUnit = mm.SelectedValue.ToString();
         if (sUnit == "D")
             sUnitName = "Days";
         else if (sUnit == "W")
             sUnitName = "Weeks";
         else if (sUnit == "M")
             sUnitName = "Months";
         else if (sUnit == "Q")
             sUnitName = "Quarters";
         else if (sUnit == "Y")
             sUnitName = "Years";
                 
        %>
<h1>Rate This Page Feedback Results: Feedback Trends</h1>

<div  style="background-color:#FAFAEF;width:961px;text-align:left;height:75px" >
        <h2>Date range (mm/dd/yyyy):</h2>
		<div id="FromDateDiv">
			<div>
                <label class="labelFeedback" for="txtDateFrom">From:</label><%string ErrorMessage = Html.ValidationMessage("FromDateValidation").ToString();%>
                <%if (ErrorMessage.Contains("Invalid Date Range"))
                  { %><label class="errorMessage1"><%=ErrorMessage%></label>
                <%}%>
            </div>
				<input type="text" id="txtDateFrom" value="<%=tDateFrom%>" name="txtDateFrom" class="DateFromText" title="Please enter a valid From Date" onkeypress="return NoSubmission()"  onfocus="ClearDate();"/>
				<input type="image" alt="Select From date" value="aaa"  src="../../Images/calendar.gif" id="dateImgFrom" class="subscribeButton" />
                <div id="calcontainerFrom"></div>
	    </div>
	
        <div id="ToDateDiv">
        
			<div>
                <label class="labelFeedback" for="txtDateTo">To:</label><% ErrorMessage = Html.ValidationMessage("ToDateValidation").ToString();%>
                <%if (ErrorMessage.Contains("Invalid Date Range"))
                  { %><label class="errorMessage1"><%=ErrorMessage%></label>
                <%}%>
            </div>
			<div>
				<input type="text" class="DateTo" value="<%=tDateTo %>" id="txtDateTo" name="txtDateTo" title="Please enter valid To Date" onkeypress="return NoSubmission()"  onfocus="ClearDate();"/>
				<input type="image" alt="Select To date"value="bbb" src="../../Images/calendar.gif" id="dateImgTo"  class="searchButton" />
                <div id="calcontainerTo"></div>
			</div>
        </div>

         <div id="Unit">
			<div><label class="SelectionPanelHeader" for="SelectedUnit">Units:</label></div>
			<div>
                <%= Html.DropDownList("SelectedUnit", (SelectList)ViewData["UnitList"], new { @class = "UnitList", Title = "", onfocus="ClearDate();"})%>
                <div id="Div7"></div>
			</div>
        </div>

</div>


<div  style="background-color:#FAFAEF;width:961px;text-align:left;height:75px" >
        <div id="URLContainsDiv">
			<div><label class="SelectionPanelHeader" for="txtURLContains">URL Contains:</label></div>
				<input type="text" id="txtURLContains" value="<%=uContains%>" name="txtURLContains" class="DateFromText" title="Please enter a valid From Date"  onkeypress="return NoSubmission()"
                onblur="if (this.value == '') {this.value = 'Enter Text';}" 
                onfocus="if (this.value == 'Enter Text') {this.value = '';}"/>
                <div id="Div3"></div>
	    </div>
	
        <div id="AgencyDiv">
			<div><label class="SelectionPanelHeader" for="SelectedAgency">Agency:</label></div>
			<div>
                <%= Html.DropDownList("SelectedAgency", (SelectList)ViewData["AgencyList"], new { @class = "SelectedAgency", Title = "" })%>
                <div id="Div5"></div>
			</div>
        </div>

        <div>
            <input class="updateRankingButton" type="submit"  value="Update Trends" id="Submit2" name="updateranking"/>
            <input class="updateRankingButton" type="reset"  value="Reset" id="Submit3" name="resetit" onclick="document.location.href =' <% = Url.Action("Trends")  %>'" />
        </div>
</div>
    
<%
if (Model != null)
{
    ErrorMessage = Html.ValidationMessage("RecordCount").ToString();%>
            <% if (ErrorMessage.Contains("No results found for the specified search criteria"))
               { %>
            <label class="errorMessage1"><%=ErrorMessage%></label>
            <%}
               else
               {%>
<br />
<div id="Div1">
 <input type="submit" value="Export to Excel" id="Submit1" class="button" name="Export"/>
</div>
<br />
<h2>Chart : Feedback Trends for the combination of the URL containing 'Text' and Agency 'Text'</h2>
    <div id="visualization" style="width: 800px; height: 400px;left:10px;top:0px"></div>

<h2>Table : Feedback Trends for the combination of the URL containing 'Text' and Agency 'Text'</h2>
<br /><br />
    <% Html.RenderPartial("PaginationControl", ViewData["paging"]); %>

    <table class="data" width="961px" summary="This table provides the Trend result for selected Unit, Date Range, Agency, URL contains" >
        <tr >
            <% string SortBy = ViewData["SortBy"].ToString();
               string Order = ViewData["Order"].ToString();
               
            %>
            <th>
                <% if (SortBy == "UnitName" && Order == "Asc")
                   { %>
                <%= Html.ActionLink(sUnitName, "Trends", new { SortBy = "UnitName", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%><img id="Img4" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink(sUnitName, "Trends", new { SortBy = "UnitName", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%>
                    <%if (SortBy == "UnitName")
                      {%>
                    <img id="Img6" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%}%> 
                  

            </th>
            <th>
                <% if (SortBy == "VolumeOfFeedback" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("Volume of Feedback", "Trends", new { SortBy = "VolumeOfFeedback", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%><img id="Img2" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("Volume of Feedback", "Trends", new { SortBy = "VolumeOfFeedback", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%>
                <%if (SortBy == "VolumeOfFeedback")
                  {%>
                    <img id="Img1" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>                 
            </th>
            <th>
                <% if (SortBy == "NumOfPosComments" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("No. of Positive Feedback Comments", "Trends", new { SortBy = "NumOfPosComments", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%><img id="Img9" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("No. of Positive Feedback Comments", "Trends", new { SortBy = "NumOfPosComments", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%>
                <%if (SortBy == "NumOfPosComments")
                  {%>
                    <img id="Img10" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>
            </th>
            <th>
                <% if (SortBy == "NumOfNegComments" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("No. of Negative Feedback Comments", "Trends", new { SortBy = "NumOfNegComments", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%><img id="Img11" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("No. of Negative Feedback Comments", "Trends", new { SortBy = "NumOfNegComments", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency, SelectedUnit = sUnit })%>
                <%if (SortBy == "NumOfNegComments")
                  {%>
                    <img id="Img12" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
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
                <%= Html.Encode(item.UnitName)%>
            </td>
            <td>
                <%= Html.Encode(item.VolumeFeedBack)%>
            </td>
            <td>
                <%= Html.Encode(item.NumOfPosComments)%>
            </td>
             <td>
                <%= Html.Encode(item.NumOfNegComments)%>
            </td>
        </tr>
    <% } %>

    </table>
    <hr align="left" width="961px"/>
     <% Html.RenderPartial("PaginationControl", ViewData["paging"]); %>
    <br />
    <br />
    
    <div> 
        <a href="#top"><img alt="Back to top" src="../Images/backtop.gif" style="vertical-align:middle;border-color:White;"/>Back to Top</a>
    </div>


<%}
}
     } %>

</asp:Content>
