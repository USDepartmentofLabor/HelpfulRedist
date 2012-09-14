using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using gov.dol.opa;
using System.Web.Mvc;
using System.Configuration;

namespace FeedbackScript.Admin.Models
{
    public class ReportsDomain
    {
        protected string connectionKey;
        protected FeedbackreportsDataContext dataContext;
        public string errorMessage = String.Empty;

        //Create database connection object
        public ReportsDomain(string ConnectionKey)
        {
            connectionKey = ConnectionKey;
            //dataContext = new FeedbackreportsDataContext(getConnectionString());
            dataContext = new FeedbackreportsDataContext(connectionKey); //"Data Source=FPDBMS14v; Initial Catalog=Feedbackscript; User ID=FeedbackAdmin; Password=!Password!;Persist Security Info=false");

        }

        //Return the connection string
        //Important: Currently it is not in use
        protected string getConnectionString()
        {
            try
            {
                string connectionString = "";
                ConnectionManager cm = new ConnectionManager();
                cm.setEncryptionKeyString("This is some key that no one knows.");
                cm.Open();
                //connectionString = cm.getConnectionString();
                return connectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Return the Top 10 Most Rated, Highest Rated and Lowest Rated DOL Pages by using spGetTopPages stored procedure
        public IEnumerable<TopPages> GetTopTenList(string txtDateFrom, string txtDateTo)
        {
            try
            {
               var results = dataContext.spGetTopPages(Convert.ToDateTime(txtDateFrom), Convert.ToDateTime(txtDateTo));
               IEnumerable<TopPages> res = from resp in results
                                           //where resp.VolumeOfFeedback >= resp.Mean 
                                           orderby resp.VolumeOfFeedback descending 
                                            select new TopPages
                                            {
                                                Agency = resp.Agency,
                                                URL = resp.URL,
                                                VolumeFeedBack = Convert.ToInt32(resp.VolumeOfFeedback),
                                                Pfeedback = Convert.ToDouble(resp.Positive),
                                                Nfeedback = Convert.ToDouble(resp.Negative),
                                                Mean = Convert.ToInt32(resp.Mean)
                                            };
                
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Return the summary by agency for Rate This Page Feedback Results for a date range by using spSummaryPages stored procedure
        public IEnumerable<TopPages> GetSummary(string txtDateFrom, string txtDateTo)
        {
            try
            {
                var results = dataContext.spSummaryPages(Convert.ToDateTime(txtDateFrom), Convert.ToDateTime(txtDateTo));
                IEnumerable<TopPages> res = from resp in results
                                            //where resp.VolumeOfFeedback >= resp.Mean 
                                            orderby resp.VolumeOfFeedback descending
                                            select new TopPages
                                            {
                                                Agency = resp.Agency,
                                                VolumeFeedBack = Convert.ToInt32(resp.VolumeOfFeedback),
                                                Pfeedback = Convert.ToDouble(resp.Positive),
                                            };

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Return the search results for Rate This Page Feedback Results for a date range, URL and Agency by using spGetSearchPages stored procedure
        public IEnumerable<TopPages> GetSearch(string txtDateFrom, string txtDateTo, string urlContains, string SelectedAgency)
        {
            try
            {
                var results = dataContext.spGetSearchPages(Convert.ToDateTime(txtDateFrom), Convert.ToDateTime(txtDateTo),urlContains,SelectedAgency);
                IEnumerable<TopPages> res = from resp in results
                                            orderby resp.VolumeOfFeedback descending
                                            select new TopPages
                                            {
                                                URL = resp.URL,
                                                Agency = resp.Agency,
                                                VolumeFeedBack = Convert.ToInt32(resp.VolumeOfFeedback),
                                                Pfeedback = Convert.ToDouble(resp.Positive),
                                                NumOfPosComments = Convert.ToInt32(resp.PositiveNoOfComments),
                                                NumOfNegComments = Convert.ToInt32(resp.NegativeNoOfComments)
                                            };

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Return Agencies list for a date range by using spGetAgencyList stored procedure
        public IEnumerable<TopPages> GetAgencyList(string txtDateFrom, string txtDateTo)
        {
            var results = dataContext.spGetAgencyList(Convert.ToDateTime(txtDateFrom), Convert.ToDateTime(txtDateTo));
            IEnumerable<TopPages> res = from resp in results
                                   select new TopPages
                                   {
                                       Agency = resp.Agency
                                   };
            return res;

        }

        //Return Comments list for a date range and URL by using spGetComments stored procedure
        public IEnumerable<CommentsList> GetCommentsList(string txtDateFrom, string txtDateTo, string URL)
        {
            try
            {
                var results = dataContext.spGetComments(URL,Convert.ToDateTime(txtDateFrom), Convert.ToDateTime(txtDateTo));
                IEnumerable<CommentsList> res = from resp in results
                                            
                                            orderby resp.utcDate descending
                                            select new CommentsList
                                            {
                                                utcDate = Convert.ToDateTime(resp.utcDate),
                                                PositiveComments = resp.PostiveComments,
                                                NegativeComments = resp.NegativeComments
                                            };

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Return Feedback Trend results by using spGetTrendResults stored procedure
        public IEnumerable<TopPages> GetTrend(string txtDateFrom, string txtDateTo, string urlContains, string SelectedAgency, string SelectedUnit, char CustomOrder)
        {
            try
            {
                var results = dataContext.spGetTrendResults(Convert.ToDateTime(txtDateFrom), Convert.ToDateTime(txtDateTo), urlContains, SelectedAgency, Convert.ToChar(SelectedUnit), CustomOrder);
                IEnumerable<TopPages> res = from resp in results
                                            //orderby resp.Unit descending
                                            select new TopPages
                                            {
                                                UnitName = resp.Unit,
                                                VolumeFeedBack = Convert.ToInt32(resp.VolumeOfFeedback),
                                                NumOfPosComments = Convert.ToInt32(resp.PositiveNoOfComments),
                                                NumOfNegComments = Convert.ToInt32(resp.NegativeNoOfComments)
                                            };

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

