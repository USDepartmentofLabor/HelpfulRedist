using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeedbackScript.Admin.Models;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace FeedbackScript.Admin.Controllers
{
    /*public class FeedbackLog : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.Controller.GetType().Name;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string userName = filterContext.HttpContext.User.Identity.Name;
            string parameterInfo = string.Empty;
            StringBuilder CompleteParam = new StringBuilder();

            if (filterContext.ActionParameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in filterContext.ActionParameters)
                {
                    parameterInfo = string.Format("Parameter name: {0} – Parameter value: {1}", parameter.Key, parameter.Value == null ? "null" : parameter.Value);
                    CompleteParam.Append(parameterInfo + Environment.NewLine);
                }
            }
            
        }
    }*/
    public class ReportsController : Controller
    {
        //Check and return valid date
        private bool isDate(string inputdate)
        {
            DateTime dt;
            DateTime.TryParse(inputdate, out dt);
            if (dt >= Convert.ToDateTime("1/1/1753") & dt <= Convert.ToDateTime("12/31/9999"))
                return DateTime.TryParse(inputdate, out dt);
            else
            {
                return false;
            }
            
        }

        //Replace the characters <>'[]\ with empty string. Normally used in comments text.
        private string ReplaceBad(string strTemp)
        {
            return strTemp.Replace("<","").Replace(">","").Replace("'","").Replace("[","").Replace("]","").Replace("\"","");
        }

        //Return the 10 Most Rated, Highest Rated and Lowest Rated DOL Pages for last 30 days (default) on TopTenPages.aspx page 
        //If the From or To date is empty, return the "Invalid Date Range" message. 
        //If "Export to Excel" option is selected, call the ExportToExcelTopTenPages function for generating and returning an excel document. 
        [LogRequest]
        [ValidateInput(false)]
        public ActionResult Toptenpages(string txtDateFrom, string txtDateTo, string Export)
        {
            try
            {
                ViewData["FromDate"] = DateTime.Today.Subtract(TimeSpan.FromDays(365)).ToShortDateString();
                ViewData["ToDate"] = DateTime.Today.ToShortDateString();
                string strClientUserName = Environment.UserName.ToString().Trim();
                if (txtDateFrom == null | txtDateTo == null)
                {
                    txtDateTo = System.DateTime.Today.ToShortDateString();
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                }

                ViewData["txtDateFrom"] = txtDateFrom;
                ViewData["txtDateTo"] = txtDateTo;

                bool mFromDateStatus = isDate(txtDateFrom);
                bool mToDateStatus = isDate(txtDateTo);

                if (mFromDateStatus == false)
                {
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                    ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                }
                if (mToDateStatus == false)
                {
                    ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                    txtDateTo = System.DateTime.Today.ToShortDateString(); 
                }
                ViewData["txtDateFrom"] = txtDateFrom;
                ViewData["txtDateTo"] = txtDateTo;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    if (Convert.ToDateTime(txtDateFrom) > Convert.ToDateTime(txtDateTo))
                    {
                        ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                        ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                        return View();
                    }
                }
                ReportsDomain _reportsDomain = new ReportsDomain(ConfigurationManager.ConnectionStrings["FeedbackScriptConnectionString"].ConnectionString);

                var orderedResultsPerPage = _reportsDomain.GetTopTenList(txtDateFrom, txtDateTo);

               

                bool m = isDate(txtDateFrom);
                if (Export != null)
                {
                    ExportToExcelTopTenPages(orderedResultsPerPage.ToList(),txtDateFrom.ToString(),txtDateTo.ToString());
                }

                if (_reportsDomain.GetTopTenList(txtDateFrom, txtDateTo).Count() <= 0)
                    ModelState.AddModelError("RecordCount", "No results found for the specified search criteria");
                

                return View(orderedResultsPerPage);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message + ex.StackTrace;
                return View("Error");
            }
        }

        //Generate an excel document for the selected date range
        private void ExportToExcelTopTenPages(List<FeedbackScript.Admin.Models.TopPages> list, string dateFrom, string dateTo)
        {
            //the list is tahe rows that are checked and need to be exported
            StringWriter sw = new StringWriter();

            //I don't believe any of this syntax is right, but if they have Excel export to excel and if not export to csv  "|" delimeted
            sw.WriteLine("Date Range :");
            sw.WriteLine("From:," + dateFrom + ",To:," + dateTo);
            sw.WriteLine();
            sw.WriteLine(",10 Most DOL Rated Pages");
            sw.WriteLine("Rank,Page URL,Volume of Feedback");
            sw.WriteLine();
            int rank = new int();
            string mUrl = string.Empty;
            foreach (var item in list.Where(t => t.VolumeFeedBack >= t.Mean).OrderByDescending(t => t.VolumeFeedBack).Take(10).ToList())
            {
                mUrl = "=HYPERLINK(\"" + item.URL + "\")";
                sw.WriteLine((++rank) +  "," + mUrl + "," + item.VolumeFeedBack);

            }
            
            sw.WriteLine();
            sw.WriteLine(",10 Highest Rate pages");
            sw.WriteLine();
            sw.WriteLine("Rank,Page URL,Volume of Feedback, % of Positive Feedback");
            rank = new int();
            foreach (var item in list.Where(t => t.VolumeFeedBack >= t.Mean).OrderByDescending(t => t.Pfeedback).Take(10).ToList())
            {
                mUrl = "=HYPERLINK(\"" + item.URL + "\")";
                sw.WriteLine((++rank) + "," + mUrl + "," + item.VolumeFeedBack + "," + item.Pfeedback);

            }

            sw.WriteLine();
            sw.WriteLine(",10 Lowest Rated pages");
            sw.WriteLine();
            sw.WriteLine("Rank,Page URL,Volume of Feedback,% of Negative Feedback");
            rank = new int();

            foreach (var item in list.Where(t => t.VolumeFeedBack >= t.Mean).OrderByDescending(t => t.Nfeedback).Take(10).ToList())
            {
                mUrl = "=HYPERLINK(\"" + item.URL + "\")";
                sw.WriteLine((++rank) + "," + mUrl + "," + item.VolumeFeedBack + "," + item.Nfeedback);

            }

            Response.AddHeader("Content-Disposition", "attachment; filename=Feedback-Top-Ten-Pages.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.Write(sw);
            Response.End();

        }

        //Return the summary by agency for Rate This Page Feedback Results for last 30 days (default) on Summary.aspx page
        //If the From or To date is empty, return the "Invalid Date Range" message. 
        //If "Export to Excel" option is selected, call the ExportToExcelSummaryPage function for generating and returning an excel document. 
        [LogRequest]
        [ValidateInput(false)]
        public ActionResult Summary(string txtDateFrom, string txtDateTo, string Export,string SortBy, string Order)
        {
            try
            {
                if (txtDateFrom == null | txtDateTo == null)
                {
                    txtDateTo = System.DateTime.Today.ToShortDateString();
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                }
                
                bool mFromDateStatus = isDate(txtDateFrom);
                bool mToDateStatus = isDate(txtDateTo);

                if (mFromDateStatus == false)
                {
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                    ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                }
                if (mToDateStatus == false)
                {
                    txtDateTo = System.DateTime.Today.ToShortDateString();
                    ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                }

                ViewData["txtDateFrom"] = txtDateFrom;
                ViewData["txtDateTo"] = txtDateTo;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    if (Convert.ToDateTime(txtDateFrom) > Convert.ToDateTime(txtDateTo))
                    {
                        ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                        ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                        return View();
                    }
                }

                ReportsDomain _reportsDomain = new ReportsDomain(ConfigurationManager.ConnectionStrings["FeedbackScriptConnectionString"].ConnectionString);

                var orderedResultsPerPage = _reportsDomain.GetSummary(txtDateFrom, txtDateTo);

                if (Export == string.Empty | Export == null)
                {
                    if (SortBy == "" || SortBy == null) { SortBy = "AgencyName"; }
                    if (Order == "" || Order == null) { Order = "Asc"; }

                    switch (SortBy)
                    {
                        case "VolumeOfFeedback":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSummary(txtDateFrom, txtDateTo).OrderBy(a => a.VolumeFeedBack) : _reportsDomain.GetSummary(txtDateFrom, txtDateTo).OrderByDescending(a => a.VolumeFeedBack);
                            break;
                        case "PositiveFeedback":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSummary(txtDateFrom, txtDateTo).OrderBy(a => a.Pfeedback) : _reportsDomain.GetSummary(txtDateFrom, txtDateTo).OrderByDescending(a => a.Pfeedback);
                            break;
                        case "AgencyName":
                        default:
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSummary(txtDateFrom, txtDateTo).OrderBy(a => a.Agency) : _reportsDomain.GetSummary(txtDateFrom, txtDateTo).OrderByDescending(a => a.Agency);
                            break;
                    }

                    ViewData["SortBy"] = SortBy;
                    ViewData["Order"] = Order;
                   

                    if (_reportsDomain.GetSummary(txtDateFrom, txtDateTo).Count() <= 0)
                        ModelState.AddModelError("RecordCount", "No results found for the specified search criteria");

                    return View(orderedResultsPerPage);
                }
                else
                {
                    ExportToExcelSummaryPage(orderedResultsPerPage.ToList(), txtDateFrom.ToString(), txtDateTo.ToString());
                    return View(orderedResultsPerPage);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message + ex.StackTrace;
                return View("Error");
            }
        }

        //Generate an excel document for the selected date range
        private void ExportToExcelSummaryPage(List<FeedbackScript.Admin.Models.TopPages> list, string dateFrom, string dateTo)
        {
            //the list is tahe rows that are checked and need to be exported
            StringWriter sw = new StringWriter();

            //I don't believe any of this syntax is right, but if they have Excel export to excel and if not export to csv  "|" delimeted
            sw.WriteLine("Date Range :");
            sw.WriteLine("From:," + dateFrom + ",To:," + dateTo);
            sw.WriteLine();
            sw.WriteLine("Summary by Pages");
            sw.WriteLine("Agency,Volume of Feedback,% of Positive Feedback");
            sw.WriteLine();
            foreach (var item in list.OrderBy(t=>t.Agency))
            {
                sw.WriteLine(item.Agency + "," + item.VolumeFeedBack+ "," + item.Pfeedback);

            }

            Response.AddHeader("Content-Disposition", "attachment; filename=Feedback-Summary-by-Agency.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.Write(sw);
            Response.End();

        }

        //Return the search results for Rate This Page Feedback Results for last 30 days (default) on Search.aspx page
        //If the From or To date is empty, return the "Invalid Date Range" message.
        //If the selected Agency is empy, use DOL.GOV (default)
        //"Enter Text" is nothing but an empty value. This is displayed for user display only
        //If "Export to Excel" option is selected, call the ExportToExcelSearchPage function for generating and returning an excel document. 
        [LogRequest]
        [ValidateInput(false)]
        public ActionResult Search(string txtDateFrom, string txtDateTo, string txtURLContains, string SelectedAgency, string Export, int? Page, string SortBy, string Order)
        {
            try
            {
                if (txtURLContains == "Enter Text")
                    txtURLContains = "";
                if (SelectedAgency == null)
                {
                    SelectedAgency = "DOL.Gov";
                }
                if (txtURLContains == null)
                {
                    txtURLContains = "";
                }

                int resultsPerPage = 10;

                if (txtDateFrom == null | txtDateTo == null)
                {
                    txtDateTo = System.DateTime.Today.ToShortDateString();
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                }

                ReportsDomain _reportsDomain = new ReportsDomain(ConfigurationManager.ConnectionStrings["FeedbackScriptConnectionString"].ConnectionString);
                
                bool mFromDateStatus = isDate(txtDateFrom);
                bool mToDateStatus = isDate(txtDateTo);

                if (mFromDateStatus == false)
                {
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                    ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                }
                if (mToDateStatus == false)
                {
                    txtDateTo = System.DateTime.Today.ToShortDateString();
                    ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                }

                txtURLContains = ReplaceBad(txtURLContains);
                ViewData["txtDateFrom"] = txtDateFrom;
                ViewData["txtDateTo"] = txtDateTo;
                ViewData["urlContains"] = txtURLContains;
                
                if (!ModelState.IsValid)
                {
                    ViewData["AgencyList"] = new SelectList(new[]
                                {
                                    new {Agency="Empty List",Val="Empty List"},
                                },
                                "Agency", "Agency", "Empty List");
                    return View();
                }
                else
                {
                    if (Convert.ToDateTime(txtDateFrom) > Convert.ToDateTime(txtDateTo))
                    {
                        ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                        ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                        ViewData["AgencyList"] = new SelectList(new[]
                                {
                                    new {Agency="Empty List",Val="Empty List"},
                                },
                               "Agency", "Agency", "Empty List");
                        return View();
                    }
                }

                
                var orderedResultsPerPage = _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency);

                if (Export == string.Empty | Export == null)
                {
                    if (!Page.HasValue || Page.Value < 1) { Page = 1; }
                    if (SortBy == "" || SortBy == null) { SortBy = "AgencyName"; }
                    if (Order == "" || Order == null) { Order = "Asc"; }

                    PaginationInfo pi = new PaginationInfo { CurrentPage = Page.Value, ResultPerPage = resultsPerPage, TotalCount = _reportsDomain.GetSearch(txtDateFrom,txtDateTo,txtURLContains,SelectedAgency).Count(), SortBy = SortBy, Order = Order, txtDateFrom = txtDateFrom,txtDateTo = txtDateTo,SelectedAgency = SelectedAgency,urlContains = txtURLContains};
                    ViewData["paging"] = pi;

                    switch (SortBy)
                    {
                        case "URLLinks":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderBy(a => a.URL) : _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderByDescending(a => a.URL);
                            break;
                        case "AgencyName":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderBy(a => a.Agency) : _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderByDescending(a => a.Agency);
                            break;
                        case "PositiveFeedback":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderBy(a => a.Pfeedback) : _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderByDescending(a => a.Pfeedback);
                            break;
                        case "NumOfPosComments":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderBy(a => a.NumOfPosComments) : _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderByDescending(a => a.NumOfPosComments);
                            break;
                        case "NumOfNegComments":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderBy(a => a.NumOfNegComments) : _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderByDescending(a => a.NumOfNegComments);
                            break;
                        case "VolumeOfFeedback":
                        default:
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderBy(a => a.VolumeFeedBack) : _reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).OrderByDescending(a => a.VolumeFeedBack);
                            break;
                    }

                    
                    ViewData["SortBy"] = SortBy;
                    ViewData["Order"] = Order;
                    ViewData["AgencyList"] = new SelectList(_reportsDomain.GetAgencyList(txtDateFrom,txtDateTo).ToList(), "Agency", "Agency", SelectedAgency);
                    

                    if (_reportsDomain.GetSearch(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency).Count() <= 0)
                        ModelState.AddModelError("RecordCount", "No results found for the specified search criteria");

                    return View(orderedResultsPerPage.Skip(resultsPerPage * (Page.Value - 1)).Take(resultsPerPage));
                }
                else
                {
                    //ExportToExcelSearchPage(orderedResultsPerPage.ToList(), txtDateFrom.ToString(), txtDateTo.ToString());
                    ExportToExcelSearchPage(orderedResultsPerPage.ToList(), txtDateFrom.ToString(), txtDateTo.ToString(), txtURLContains, SelectedAgency);
                    return View(orderedResultsPerPage);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message + ex.StackTrace;
                return View("Error");
            }
        }

        //Return the view comments results for the comments entered during the Rate This Page Feedback Results for a selected URL on ViewComments.aspx page
        //This page is called on click of View link in View Comments column.
        //If "Export to Excel" option is selected, call the ExportToExcelCommentsPage function for generating and returning an excel document. 
        [LogRequest]
        [ValidateInput(false)]
        public ActionResult ViewComments(string txtDateFrom, string txtDateTo, string URL, int? Page, string SortBy, string Order, string Export, string urlContains, string SelectedAgency)
        {
            try
            {
                int resultsPerPage = 10;

                ReportsDomain _reportsDomain = new ReportsDomain(ConfigurationManager.ConnectionStrings["FeedbackScriptConnectionString"].ConnectionString);

                URL= ReplaceBad(URL);
                txtDateFrom = ReplaceBad(txtDateFrom);
                txtDateTo = ReplaceBad(txtDateTo);

                var orderedResultsPerPage = _reportsDomain.GetCommentsList(txtDateFrom, txtDateTo, URL);

                if (Export == string.Empty | Export == null)
                {
                    if (!Page.HasValue || Page.Value < 1) { Page = 1; }
                    if (SortBy == "" || SortBy == null) { SortBy = "utcDate"; }
                    if (Order == "" || Order == null) { Order = "Asc"; }

                    PaginationInfo pi = new PaginationInfo { CurrentPage = Page.Value, ResultPerPage = resultsPerPage, TotalCount = _reportsDomain.GetCommentsList(txtDateFrom, txtDateTo, URL).Count(), SortBy = SortBy, Order = Order, txtDateFrom = txtDateFrom, txtDateTo = txtDateTo, URL = URL};
                    ViewData["paging"] = pi;

                    switch (SortBy)
                    {
                        case "utcDate":
                        default:
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetCommentsList(txtDateFrom, txtDateTo, URL).OrderBy(a => a.utcDate) : _reportsDomain.GetCommentsList(txtDateFrom, txtDateTo, URL).OrderByDescending(a => a.utcDate);
                            break;
                    }

                    var CountViewPage = _reportsDomain.GetCommentsList(txtDateFrom, txtDateTo, URL);
                    if (CountViewPage.Count() == 0)
                    {
                        txtDateFrom = "";
                        txtDateTo = "";
                        URL = "";
                    }

                    ViewData["SortBy"] = SortBy;
                    ViewData["Order"] = Order;
                    ViewData["txtDateFrom"] = txtDateFrom;
                    ViewData["txtDateTo"] = txtDateTo;
                    ViewData["URL"] = URL;
                    ViewData["urlContains"] = urlContains;
                    ViewData["SelectedAgency"] = SelectedAgency;

                    

                        
                    return View(orderedResultsPerPage.Skip(resultsPerPage * (Page.Value - 1)).Take(resultsPerPage));
                }
                else
                {
                    ExportToExcelCommentsPage(orderedResultsPerPage.ToList(), txtDateFrom.ToString(), txtDateTo.ToString(),txtDateFrom,txtDateTo,URL);
                    return View(orderedResultsPerPage);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message + ex.StackTrace;
                return View("Error");
            }
        }

        //Generate an excel document for the selected date range, and URL
        private void ExportToExcelCommentsPage(List<FeedbackScript.Admin.Models.CommentsList> list, string dateFrom, string dateTo, string txtDateFrom, string txtDateTo, string URL)
        {
            //the list is tahe rows that are checked and need to be exported
            StringWriter sw = new StringWriter();

            //I don't believe any of this syntax is right, but if they have Excel export to excel and if not export to csv  "|" delimeted
            sw.WriteLine("Date Range :");
            sw.WriteLine("From:," + dateFrom + ",To:," + dateTo);
            sw.WriteLine();
            sw.WriteLine(",Page URL: ," + "=HYPERLINK(\"" + URL + "\")");
            sw.WriteLine();
            sw.WriteLine("Date,Positive Feedback Comments,Negative Feedback Comments");
            sw.WriteLine();
            foreach (var item in list.OrderBy(t => t.utcDate))
            {
                sw.WriteLine(Convert.ToDateTime(item.utcDate).ToShortDateString() + ",\"" + item.PositiveComments + "\",\"" + item.NegativeComments + "\"");
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=Feedback-Comments-by-url.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.Write(sw);
            Response.End();

        }

        //Return the Feedback Trend results and Graph for a selected Date range, URL, Agency and Unit on Trends.aspx page
        //The Graph data is generated using GetGraphData function, which is feeded to google.visualization.LineChart component in Site.Master
        //If "Export to Excel" option is selected, call the ExportToExcelTrendPage function for generating and returning an excel document. 
        [LogRequest]
        [ValidateInput(false)]
        public ActionResult Trends(string txtDateFrom, string txtDateTo, string txtURLContains, string SelectedAgency, string SelectedUnit, string Export, int? Page, string SortBy, string Order)
        {
            try
            {
                if (txtURLContains == "Enter Text")
                    txtURLContains = "";
                string sUnit = string.Empty;
                if (SelectedUnit == null)
                    SelectedUnit = "D";
                

                var list = new SelectList(new []
                                {
                                    new {ID="D",Name="Days"},
                                    new{ID="W",Name="Weeks"},
                                    new{ID="M",Name="Months"},
                                    new{ID="Q",Name="Quarters"},
                                    new{ID="Y",Name="Years"},
                                },
                "ID","Name",SelectedUnit);

                ViewData["UnitList"] = list;      
                if (SelectedAgency == null)
                {
                    SelectedAgency = "DOL.Gov";
                }
                if (txtURLContains == null)
                {
                    txtURLContains = "";
                }

                int resultsPerPage = 10;

                if (txtDateFrom == null | txtDateTo == null)
                {
                    txtDateTo = System.DateTime.Today.ToShortDateString();
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                }

                
                ReportsDomain _reportsDomain = new ReportsDomain(ConfigurationManager.ConnectionStrings["FeedbackScriptConnectionString"].ConnectionString);

                bool mFromDateStatus = isDate(txtDateFrom);
                bool mToDateStatus = isDate(txtDateTo);

                if (mFromDateStatus == false)
                {
                    txtDateFrom = System.DateTime.Today.Subtract(TimeSpan.FromDays(30)).ToShortDateString();
                    ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                }
                if (mToDateStatus == false)
                {
                    txtDateTo = System.DateTime.Today.ToShortDateString();
                    ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                }

                txtURLContains = ReplaceBad (txtURLContains);
                ViewData["txtDateFrom"] = txtDateFrom;
                ViewData["txtDateTo"] = txtDateTo;
                ViewData["urlContains"] = txtURLContains;

                if (!ModelState.IsValid)
                {
                    ViewData["AgencyList"] = new SelectList(new[]
                                {
                                    new {Agency="Empty List",Val="Empty List"},
                                },
                               "Agency", "Agency", "Empty List");
                    return View();
                }
                else
                {
                    if (Convert.ToDateTime(txtDateFrom) > Convert.ToDateTime(txtDateTo))
                    {
                        ModelState.AddModelError("FromDateValidation", "Invalid Date Range");
                        ModelState.AddModelError("ToDateValidation", "Invalid Date Range");
                        ViewData["AgencyList"] = new SelectList(new[]
                                {
                                    new {Agency="Empty List",Val="Empty List"},
                                },
                                "Agency", "Agency", "Empty List");
                        return View();
                    }
                }

                var orderedResultsPerPage = _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,'D');

                if (Export == string.Empty | Export == null)
                {
                    if (!Page.HasValue || Page.Value < 1) { Page = 1; }
                    if (SortBy == "" || SortBy == null) { SortBy = "UnitName"; }
                    if (Order == "" || Order == null) { Order = "Asc"; }
                    char mEmpty = new char();
                    PaginationInfo pi = new PaginationInfo { CurrentPage = Page.Value, ResultPerPage = resultsPerPage, TotalCount = _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,mEmpty).Count(), SortBy = SortBy, Order = Order, txtDateFrom = txtDateFrom, txtDateTo = txtDateTo, SelectedAgency = SelectedAgency, urlContains = txtURLContains, UnitName = SelectedUnit};
                    ViewData["paging"] = pi;

                    switch (SortBy)
                    {
                        case "VolumeOfFeedback":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,mEmpty).OrderBy(a => a.VolumeFeedBack) : _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,mEmpty).OrderByDescending(a => a.VolumeFeedBack);
                            break;
                        case "NumOfPosComments":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit, mEmpty).OrderBy(a => a.NumOfPosComments) : _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit, mEmpty).OrderByDescending(a => a.NumOfPosComments);
                            break;
                        case "NumOfNegComments":
                            orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit, mEmpty).OrderBy(a => a.NumOfNegComments) : _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit, mEmpty).OrderByDescending(a => a.NumOfNegComments);
                            break;
                        case "UnitName":
                        default:
                            if (SelectedUnit == "M")
                                orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,'A') : _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,'D');
                            else
                                orderedResultsPerPage = Order == "Asc" ? _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit, mEmpty).OrderBy(a=>a.UnitName) : _reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit, mEmpty).OrderByDescending(a=>a.UnitName);   
                            break;
                    }


                    ViewData["SortBy"] = SortBy;
                    ViewData["Order"] = Order;
                    ViewData["AgencyList"] = new SelectList(_reportsDomain.GetAgencyList(txtDateFrom,txtDateTo).ToList(), "Agency", "Agency", SelectedAgency);

                    if (_reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,mEmpty).Count() <= 0)
                    {
                        ModelState.AddModelError("RecordCount", "No results found for the specified search criteria");
                    }
                    else
                    {
                        int MaxRecSizeVolFeedBack = new int();
                        ViewData["graphData"] = GetGraphData(_reportsDomain.GetTrend(txtDateFrom, txtDateTo, txtURLContains, SelectedAgency, SelectedUnit,'A').ToList(), out MaxRecSizeVolFeedBack);
                        ViewData["MaxRecSizeVolFeedBack"] = MaxRecSizeVolFeedBack;
                    }

                    return View(orderedResultsPerPage.Skip(resultsPerPage * (Page.Value - 1)).Take(resultsPerPage));
                }
                else
                {
                    ExportToExcelTrendPage(orderedResultsPerPage.ToList(), txtDateFrom.ToString(), txtDateTo.ToString(),SelectedUnit,txtURLContains,SelectedAgency);
                    return View(orderedResultsPerPage);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message + ex.StackTrace;
                return View("Error");
            }
        }

        //Generate Graph data to feed google.visualization.LineChart component in Site.Master
        private string GetGraphData(List<FeedbackScript.Admin.Models.TopPages> TrendResult, out int MaxRecSizeVolFeedBack)
        {
            string gData = "data.addColumn('string', 'x');" + Environment.NewLine;
            gData = gData + "data.addColumn('number', 'Volume of Feedback');" + Environment.NewLine;
            gData = gData + "data.addColumn('number', 'Volume of Positive Feedback');" + Environment.NewLine;
            gData = gData + "data.addColumn('number', 'Volume of Negative Feedback');" + Environment.NewLine;
            foreach (var item in TrendResult.ToList()) //.OrderBy(t=>t.UnitName))
            {
                gData = gData + "data.addRow(['" + item.UnitName+ "', " + item.VolumeFeedBack + "," + item.NumOfPosComments + "," + item.NumOfNegComments + "]);" + Environment.NewLine;
            }
            MaxRecSizeVolFeedBack = (from feed in TrendResult select feed.VolumeFeedBack).Max();
            return gData;
        }

        //Generate an excel document for the selected date range, agency, unit and URL
        private void ExportToExcelTrendPage(List<FeedbackScript.Admin.Models.TopPages> list, string dateFrom, string dateTo, string UnitName, string URLContains, string AgencyName)
        {

            if (UnitName== "D")
                UnitName= "Days";
            else if (UnitName== "W")
                UnitName= "Weeks";
            else if (UnitName== "M")
                UnitName= "Months";
            else if (UnitName== "Q")
                UnitName= "Quarters";
            else if (UnitName== "Y")
                UnitName= "Years";

            StringWriter sw = new StringWriter();

            sw.WriteLine("Date Range :");
            sw.WriteLine("From:," + dateFrom + ",To:," + dateTo);
            sw.WriteLine();
            sw.WriteLine("URL Contains:," + URLContains + ",Units:," + UnitName);
            sw.WriteLine();
            sw.WriteLine("Agency Name:," + AgencyName);
            sw.WriteLine();
            sw.WriteLine("Trends report");
            sw.WriteLine("" + UnitName + ",Total Volume of Feedback,Volume of Positive Feedback,Volume of Negative Feedback");
            sw.WriteLine();
            foreach (var item in list)
            {
                sw.WriteLine(item.UnitName + ',' + item.VolumeFeedBack + "," + item.NumOfPosComments + "," + item.NumOfNegComments);

            }

            Response.AddHeader("Content-Disposition", "attachment; filename=Trend-Report.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.Write(sw);
            Response.End();

        }

        //Generate an excel document for the selected date range, URL, and Agency
        private void ExportToExcelSearchPage(List<FeedbackScript.Admin.Models.TopPages> list, string dateFrom, string dateTo, string URLContains, string AgencyName)
        {

            StringWriter sw = new StringWriter();

            sw.WriteLine("Date Range :");
            sw.WriteLine("From:," + dateFrom + ",To:," + dateTo);
            sw.WriteLine();
            sw.WriteLine("Agency Name:," + AgencyName);
            sw.WriteLine();
            sw.WriteLine("Search report");
            sw.WriteLine("URL,Agency,Volume of Feedback,% of Positive Feedback,No of Positive Feedback Comments, No of Negative Feedback Comments, View");
            sw.WriteLine();
            foreach (var item in list.OrderBy(t => t.Agency))
            {
                string mView = string.Empty;
                if (item.NumOfNegComments >= 1 | item.NumOfPosComments >= 1)
                {
                    //mView = "=HYPERLINK(\"" + Request.Url.AbsoluteUri.Replace("Search","ViewComments") + "?URL=" + item.URL + "&txtDateFrom=" + dateFrom + "&txtDateTo=" + dateTo + "&urlcontains=" + URLContains + "&SelectedAgency=" + AgencyName + "\")";
                    //Request.Url.AbsolutePath /Reports/Search
                    mView = "=HYPERLINK(\"" + Request.Url.Scheme + "://" + Request.Url.Authority + Request.Url.AbsolutePath.Replace("Search","ViewComments?")  + "URL=" + item.URL + "&txtDateFrom=" + dateFrom + "&txtDateTo=" + dateTo + "&urlcontains=" + URLContains + "&SelectedAgency=" + AgencyName + "\")";
                }
                sw.WriteLine(item.URL+ ',' + item.Agency+ "," + item.VolumeFeedBack + "," + item.Pfeedback + "," + item.NumOfPosComments + "," + item.NumOfNegComments +"," + mView);

            }

            Response.AddHeader("Content-Disposition", "attachment; filename=Trend-Report.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.Write(sw);
            Response.End();

        }
    }
}
