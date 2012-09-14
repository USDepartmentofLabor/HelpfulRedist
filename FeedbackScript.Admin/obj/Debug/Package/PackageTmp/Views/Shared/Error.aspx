<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Feedback Script Admin Interface - Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Sorry, an error occurred while processing your request, Please contact System Administrator.<br />
    </h2>    
        <% string mErrorInfo = ConfigurationManager.AppSettings["ErrorInfo"];
           if (mErrorInfo == "1"){%>
               <div  style="background-color:#FAFAEF;width:961px;text-align:left;" >
               <% Response.Write(ViewData["ErrorMessage"]);%>
               </div>
               <%}%>
</asp:Content>
