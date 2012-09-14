<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FeedbackScript.Admin.Models.PaginationInfo>"%>
<% 
if (Model.TotalCount > Model.ResultPerPage)
{
    //string mPageOf = string.Empty;
    string mPageOf = "Results ";
    string mTableWidth = string.Empty;
    int pagePerPageGroup = 5;
    int currentPageGroupStartPage = (((Model.CurrentPage - 1) / pagePerPageGroup) * pagePerPageGroup) + 1;
    
    int currentPageGroupStartPage1 = ((Model.ResultPerPage * Model.CurrentPage) - (Model.ResultPerPage - 1));

    mPageOf = mPageOf + currentPageGroupStartPage1.ToString() + " - " + (Model.CurrentPage == Model.TotalPages ? Model.TotalCount :Model.ResultPerPage * Model.CurrentPage).ToString() + " of " + Model.TotalCount.ToString();

    if (Page.Request.AppRelativeCurrentExecutionFilePath.Contains("Report"))
        mTableWidth = "961px";
    else
        mTableWidth = "961px";

    Response.Write("<table border='0' width='" + mTableWidth + "'><tr><td>" + mPageOf.ToString() + "</td>");
    Response.Write("<td width=380 align=right>");
    
    if (currentPageGroupStartPage > pagePerPageGroup && Model.CurrentPage != currentPageGroupStartPage)
    {%> <%= Html.ActionLink("<<", ViewContext.RouteData.Values["action"].ToString(), new { Page = (Model.CurrentPage - pagePerPageGroup), SortBy = Model.SortBy, Order = Model.Order, txtDateFrom = Model.txtDateFrom,txtDateTo = Model.txtDateTo,SelectedAgency = Model.SelectedAgency,txtURLcontains = Model.urlContains,URL = Model.URL, SelectedUnit = Model.UnitName })%> <% }
    
    if (Model.CurrentPage > 1)
    { %><%= Html.ActionLink("<", ViewContext.RouteData.Values["action"].ToString(), new { Page = Model.CurrentPage - 1, SortBy = Model.SortBy, Order = Model.Order, txtDateFrom = Model.txtDateFrom, txtDateTo = Model.txtDateTo, SelectedAgency = Model.SelectedAgency, txtURLcontains = Model.urlContains, URL = Model.URL, SelectedUnit = Model.UnitName })%> <% }

    for (int counter = currentPageGroupStartPage; counter < currentPageGroupStartPage + pagePerPageGroup && counter  <= Model.TotalPages; counter++)
    {
        if (counter > currentPageGroupStartPage)
        {%> | <%}
        
        if (counter == Model.CurrentPage)
        {%> <%= counter %> <%}
        else
        {%> <%= Html.ActionLink(counter.ToString(), ViewContext.RouteData.Values["action"].ToString(), new { Page = counter, SortBy = Model.SortBy, Order = Model.Order, txtDateFrom = Model.txtDateFrom, txtDateTo = Model.txtDateTo, SelectedAgency = Model.SelectedAgency, txtURLcontains = Model.urlContains, URL = Model.URL, SelectedUnit = Model.UnitName })%> <% }
    }

    if (Model.TotalPages > 1 &&  Model.CurrentPage < Model.TotalPages)
    { %> <%= Html.ActionLink(">", ViewContext.RouteData.Values["action"].ToString(), new { Page = Model.CurrentPage + 1, SortBy = Model.SortBy, Order = Model.Order, txtDateFrom = Model.txtDateFrom, txtDateTo = Model.txtDateTo, SelectedAgency = Model.SelectedAgency, txtURLcontains = Model.urlContains, URL = Model.URL, SelectedUnit = Model.UnitName })%> <% }

    if (currentPageGroupStartPage + pagePerPageGroup <= Model.TotalPages && Model.CurrentPage != currentPageGroupStartPage + pagePerPageGroup -1)
    { %> <%= Html.ActionLink(">>", ViewContext.RouteData.Values["action"].ToString(), new { Page = (pagePerPageGroup + currentPageGroupStartPage), SortBy = Model.SortBy, Order = Model.Order, txtDateFrom = Model.txtDateFrom, txtDateTo = Model.txtDateTo, SelectedAgency = Model.SelectedAgency, txtURLcontains = Model.urlContains, URL = Model.URL, SelectedUnit = Model.UnitName })%> <% }

    Response.Write("</td></tr></table>");    
} %>    