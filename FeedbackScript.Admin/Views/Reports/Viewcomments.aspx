<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FeedbackScript.Admin.Models.CommentsList>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Feedback Script - Rate This Page Feedback Results: View Comments Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<A NAME="top"></A>
  <% using (Html.BeginForm())
     {
         string tDateFrom = string.Empty ;
         string tDateTo = string.Empty;
         string uContains = string.Empty;
         string sAgency = string.Empty;
         string sURL = string.Empty;
         if (ViewData["txtDateFrom"] != null)
         {
             tDateFrom = ViewData["txtDateFrom"].ToString();
             tDateTo = ViewData["txtDateTo"].ToString();
         }
         if (ViewData["URL"] != null)
         {
             sURL= ViewData["URL"].ToString();
         }
         if (ViewData["urlContains"] != null)
         {
             uContains = ViewData["urlContains"].ToString();
         }
         if (ViewData["SelectedAgency"] != null)
         {
             sAgency = ViewData["SelectedAgency"].ToString();
         }
        %>
<h1>Rate This Page Feedback Results: View Comments Page</h1>
<br />

<h2>From : <%=tDateFrom%></h2>
<h2>To   : <%=tDateTo%></h2>
<h2>URL  : <%=sURL%></h2>
<br />
<div id="Div1">
 <input type="submit" value="Export to Excel" id="Submit1" class="button" name="Export"/>
</div>

<br />

    <% Html.RenderPartial("PaginationControl", ViewData["paging"]); %>

    <table class="data" width="961px" >
    <col width="81px"/>
    <col width="440px"/>
    <col width="440"/>
        <tr >
            <% string SortBy = ViewData["SortBy"].ToString(); 
               string Order = ViewData["Order"].ToString(); 
            %>
            <th>
                <% if (SortBy == "utcDate" && Order == "Asc")
                   { %>
                <%= Html.ActionLink("Date", "Viewcomments", new { SortBy = "utcDate", Order = "Dis", txtDateFrom = tDateFrom, txtDateTo=tDateTo,URL=sURL})%><img id="Img4" runat="server" alt="Sort Descending" src="~/images/downarrow.gif" />
                <% }
                   else
                   { %>
                <%= Html.ActionLink("Date", "Viewcomments", new { SortBy = "utcDate", Order = "Asc", txtDateFrom = tDateFrom, txtDateTo = tDateTo,URL = sURL })%>
                    <%if (SortBy == "utcDate")
                      {%>
                    <img id="Img6" runat="server" alt="Sort Ascending" src="~/images/uparrow.gif" />
                    <%} %>
                <%}%> 
                  

            </th>
            <th>Positive Feedback Comments</th>
            <th>Negative Feedback Comments</th>
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
            <td  scope="row" style="vertical-align:top; margin-top:0; ">
                <%= Html.Encode(item.utcDate.ToShortDateString()) %>
            </td>
            <td>
                <%= Html.Encode(item.PositiveComments)%>
            </td>
            <td>
                <%= Html.Encode(item.NegativeComments)%>
            </td>
        </tr>
    <% } %>

    </table>
    <hr align="left" width="961px"/>
     <% Html.RenderPartial("PaginationControl", ViewData["paging"]); %>
    <br />
    <br />
    
    <div> 
        < <%=Html.ActionLink("Back to Feedback Results- Search Page", "Search", new { txtDateFrom = tDateFrom, txtDateTo = tDateTo, txtURLContains = uContains, SelectedAgency = sAgency })%>
    </div>

<%} %>

</asp:Content>
