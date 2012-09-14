<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FeedbackScript.Admin.Models.TopPages>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Feedback Script - Rate This Page Feedback Results: Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<A NAME="top"></A>
  <% using (Html.BeginForm())
     {
         string tDateFrom = string.Empty;
         string tDateTo = string.Empty;
         string uContains = string.Empty;
         string sAgency = string.Empty;
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
         if (ss != null)
            sAgency = ss.SelectedValue.ToString();
        %>
<h1>Rate This Page Feedback Results: Summary by Agency</h1>

<div  style="background-color:#FAFAEF;width:961px;text-align:left;height:75px" >
        <h2>Date range (mm/dd/yyyy):</h2>
		<div id="FromDateDiv">
    		<div>
            <label class="labelFeedback" for="txtDateFrom">From:</label><%string ErrorMessage = Html.ValidationMessage("FromDateValidation").ToString();%>
                <%if (ErrorMessage.Contains("Invalid Date Range"))
                  { %><label class="errorMessage1"><%=ErrorMessage%></label>
                <%}%>  
            </div>
				<input type="text" id="txtDateFrom" value="<%=tDateFrom%>" name="txtDateFrom" class="DateFromText" title="Please enter a valid From Date" onkeypress="return NoSubmission()" onfocus="ClearDate();"/>
				<input type="image" alt="Select From date" value="aaa"  src="../../Images/calendar.gif" id="dateImgFrom" class="subscribeButton" />
                <div id="calcontainerFrom"></div>
	    </div>
	
        <div id="ToDateDiv">
			<div>
                <label class="labelFeedback" for="txtDateTo">To:</label> <% ErrorMessage = Html.ValidationMessage("ToDateValidation").ToString();%>
                <%if (ErrorMessage.Contains("Invalid Date Range"))
                  { %><label class="errorMessage1"><%=ErrorMessage%></label>
                <%}%>
             </div>
			<div>
				<input type="text" class="DateTo" value="<%=tDateTo %>" id="txtDateTo" name="txtDateTo" title="Please enter valid To Date" onkeypress="return NoSubmission()" onfocus="ClearDate();"/>
				<input type="image" alt="Select To date" value="bbb" src="../../Images/calendar.gif" id="dateImgTo" alt="Find it in DOL" class="searchButton" />
                <div id="calcontainerTo"></div>
			</div>
        </div>

</div>
<div  style="background-color:#FAFAEF;width:961px;text-align:left;height:75px" >
        <div id="URLContainsDiv">
			<div><label class="SelectionPanelHeader" for="txtURLContains">URL Contains:</label></div>
				<input type="text" id="txtURLContains" value="<%=uContains%>" name="txtURLContains" class="DateFromText" title="Please enter URL contains text" onkeypress="return NoSubmission()" 
                onblur="if (this.value == '') {this.value = 'Enter Text';}" 
                onfocus="if (this.value == 'Enter Text') {this.value = '';};ClearDate();"/>
                <div id="Div3"></div>
	    </div>
	
        <div id="AgencyDiv">
			<div><label class="SelectionPanelHeader" for="SelectedAgency">Agency:</label></div>
			<div>
                <%if (ViewData["AgencyList"] != null)
                  {%>
                    <%= Html.DropDownList("SelectedAgency", (SelectList)ViewData["AgencyList"], new { @class = "DateTo", Title = "", Visible = false, onfocus = "ClearDate();" })%>
                <%}
                  else
                  {%>
                    <%= Html.DropDownList("SelectedAgency", null, new { @class = "DateTo", width = "40px", Title = "", Visible = false, onfocus = "ClearDate();" })%>
                    <%} %>
                <div id="Div5"></div>
			</div>
        </div>

        <div>
            <input class="updateRankingButton" type="submit"  value="Search" id="Submit2" name="updateranking"/>
            <input class="updateRankingButton" type="button"  value="Reset" id="Submit3" name="resetit" onclick="document.location.href =' <% = Url.Action("Search")  %>'" />
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
               {%>
<br />

<div id="Div1">
 <input type="submit" value="Export to Excel" id="Submit1" class="button" name="Export"/>
</div>
<br />
    <% Html.RenderPartial("PaginationControl", ViewData["paging"]); %>

    <table class="data" width="961px"  style="table-layout:fixed;WORD-BREAK:BREAK-ALL;" summary="This table provides the summary of Feedback reports vased ob selected date Range, URL contains and selected Agency">
    <col width="300px"/>
    <col width="80px"/>
    <col width="50px"/>
    <col width="53px"/>
    <col width="73px"/>
    <col width="75px"/>
    <col width="55px"/>
    

        <tr >
            <% string SortBy = ViewData["SortBy"].ToString();
               string Order = ViewData["Order"].ToString(); 
            %>
            <th>
                <% if (SortBy == "URLLink" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("URL", "Search", new { SortBy = "URLLink", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%><img id="Img4" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("URL", "Search", new { SortBy = "URLLink", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%>
                    <%if (SortBy == "URLLink")
                      {%>
                    <img id="Img6" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%}%> 
                  

            </th>
            <th>
                <% if (SortBy == "AgencyName" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("Agency", "Search", new { SortBy = "AgencyName", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%><img id="Img7" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("Agency", "Search", new { SortBy = "AgencyName", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%>
                    <%if (SortBy == "AgencyName")
                      {%>
                    <img id="Img8" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%}%> 
            </th>
            <th>
                <% if (SortBy == "VolumeOfFeedback" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("Volume of Feedback", "Search", new { SortBy = "VolumeOfFeedback", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%><img id="Img2" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("Volume of Feedback", "Search", new { SortBy = "VolumeOfFeedback", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%>
                <%if (SortBy == "VolumeOfFeedback")
                  {%>
                    <img id="Img1" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>                 
            </th>
            <th>
                <% if (SortBy == "PositiveFeedback" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("% Positive Feedback", "Search", new { SortBy = "PositiveFeedback", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%><img id="Img5" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("% Positive Feedback", "Search", new { SortBy = "PositiveFeedback", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%>
                <%if (SortBy == "PositiveFeedback")
                  {%>
                    <img id="Img3" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>
            </th>
            <th>
                <% if (SortBy == "NumOfPosComments" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("No. of Positive Feedback Comments", "Search", new { SortBy = "NumOfPosComments", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%><img id="Img9" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("No. of Positive Feedback Comments", "Search", new { SortBy = "NumOfPosComments", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%>
                <%if (SortBy == "NumOfPosComments")
                  {%>
                    <img id="Img10" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>
            </th>
            <th>
                <% if (SortBy == "NumOfNegComments" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("No. of Negative Feedback       Comments", "Search", new { SortBy = "NumOfNegComments", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%><img id="Img11" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("No. of Negative Feedback       Comments", "Search", new { SortBy = "NumOfNegComments", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlContains = uContains, SelectedAgency = sAgency })%>
                <%if (SortBy == "NumOfNegComments")
                  {%>
                    <img id="Img12" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%} %>
            </th>
            <th>
                View<br /> Comments
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
                <%= Html.Encode(item.URL)%>
            </td>
            <td>
                <%= Html.Encode(item.Agency)%>
            </td>
            <td>
                <%= Html.Encode(item.VolumeFeedBack)%>
            </td>
            <td>
                <%= Html.Encode(item.Pfeedback)%>
            </td>
            <td>
                <%= Html.Encode(item.NumOfPosComments)%>
            </td>
             <td>
                <%= Html.Encode(item.NumOfNegComments)%>
            </td>
 
            <%
               
if (item.NumOfNegComments >= 1 | item.NumOfPosComments >= 1)
{ %>
                <td>
                    <%= Html.ActionLink("View", "ViewComments", new { URL = item.URL, txtDateFrom = tDateFrom, txtDateTo = tDateTo, urlcontains = uContains, SelectedAgency = sAgency })%>
                </td>
            <%}%>
            <%else
{%>
                 <td>&nbsp</td>
                 <%} %>
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