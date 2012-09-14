using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Helpful.Models;

namespace Helpful.Controllers
{
    public class DataController : Controller
    {
        // Fields
        trackingDataContext tdc = new trackingDataContext();

        // Methods
        private string GetAgencyCode(string dolurl)
        {
            try
            {
                string str = string.Empty;
                Uri uri = new Uri(dolurl);
                if (uri.Host.ToLower().EndsWith(".gov"))
                {
                    if ((((uri.Host.ToLower().EndsWith("msha.dol.gov") || uri.Host.ToLower().EndsWith("oalj.dol.gov")) || (uri.Host.ToLower().EndsWith("autocommunities.gov") || uri.Host.ToLower().EndsWith("doors.dol.gov"))) || ((uri.Host.ToLower().EndsWith("efast.dol.gov") || uri.Host.ToLower().EndsWith("jobcorps.gov")) || (uri.Host.ToLower().EndsWith("mynextmove.dol.gov") || uri.Host.ToLower().EndsWith("saversummit.dol.gov")))) || (((uri.Host.ToLower().EndsWith("savingmatters.dol.gov") || uri.Host.ToLower().EndsWith("youthrules.dol.gov")) || (uri.Host.ToLower().EndsWith("bls.gov") || uri.Host.ToLower().EndsWith("doleta.gov"))) || (((uri.Host.ToLower().EndsWith("msha.gov") || uri.Host.ToLower().EndsWith("osha.gov")) || (uri.Host.ToLower().EndsWith("oalj.dol.gov") || uri.Host.ToLower().EndsWith("jobcorps.gov"))) || (uri.Host.ToLower().EndsWith("pbgc.gov") || uri.Host.ToLower().EndsWith("oig.dol.gov")))))
                    {
                        str = uri.Host.Replace("gov", "").Replace("dol", "").Replace(".", "").Replace("www", "");
                    }
                    else if (uri.Segments.Count<string>() == 1)
                    {
                        str = "DOL.Gov";
                    }
                    else if ((uri.Segments.Count<string>() == 2) & uri.Segments[1].Contains<char>('.'))
                    {
                        str = "DOL.Gov";
                    }
                    else
                    {
                        str = uri.Segments[1].Replace("/", "");
                    }
                }
                return str.ToUpper();
            }
            catch
            {
                return "DOL.Gov";
            }
        }

        private string GetCorrectAgencyName(string mAgencyCode)
        {
            if (mAgencyCode == "jobcorps")
            {
                mAgencyCode = "ojc";
            }
            if (mAgencyCode == "doleta")
            {
                mAgencyCode = "eta";
            }
            if (mAgencyCode == "asp")
            {
                mAgencyCode = "oasp";
            }
            if (mAgencyCode == "_sec")
            {
                mAgencyCode = "osec";
            }
            if (mAgencyCode == "appeals")
            {
                mAgencyCode = "ecab";
            }
            if (mAgencyCode == "dol")
            {
                mAgencyCode = "dol directory";
            }
            return mAgencyCode;
        }

        public ActionResult Index()
        {
            return base.View();
        }

        public void Log(string dolurl, string question, string suggestion, string posttype)
        {
            base.Response.AddHeader("Access-Control-Allow-Origin", "*");
            base.Response.AddHeader("Access-Control-Allow-Methods", "POST,OPTIONS");
            base.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
            base.Response.AddHeader("Access-Control-Allow-Credentials", "false");
            base.Response.AddHeader("Access-Control-Max-Age", "60");
            try
            {

                if ((((((base.Request.QueryString.AllKeys.Contains<string>("dolurl") & base.Request.QueryString.AllKeys.Contains<string>("question")) & base.Request.QueryString.AllKeys.Contains<string>("posttype")) && (dolurl.Trim() != "")) && ((question.ToUpper() == "YES") || (question.ToUpper() == "NO"))) && (!(posttype.ToUpper() != "C") || !(posttype.ToUpper() != "Q"))) && (((dolurl != null) && (question != null)) && (suggestion != null)))
                {
                    bool flag = false;
                    string agencyCode = this.GetAgencyCode(dolurl);
                    if (question.ToUpper() == "YES")
                    {
                        flag = true;
                    }
                    if (posttype.ToUpper() == "Q")
                    {
                        Response entity = new Response
                        {
                            Url = dolurl,
                            Positive = new bool?(flag),
                            UtcDate = DateTime.Now,
                            Agency = agencyCode
                        };
                        tdc.Responses.InsertOnSubmit(entity);
                        tdc.SubmitChanges();
                    }
                    else if (posttype.ToUpper() == "C")
                    {
                        Comment comment = new Comment
                        {
                            Url = dolurl,
                            Positive = new bool?(flag),
                            Comment1 = suggestion,
                            UtcDate = DateTime.Now,
                            Agency = agencyCode
                        };
                        tdc.Comments.InsertOnSubmit(comment);
                        tdc.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
