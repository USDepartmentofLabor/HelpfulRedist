using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Mail;
using System.Text;
using System.Configuration;


namespace FeedbackScript.Admin.Models
{
 
    //Model class for displaying 10 Most Rated, Highest Rated and Lowest Rated DOL Pages on Toptenpages.aspx page
    public class TopPages
    {
        public string Agency { get; set; }
        public string URL{ get; set; }
        public int VolumeFeedBack { get; set; }
        public double Pfeedback { get; set; }
        public double  Nfeedback { get; set; }
        public int  Mean { get; set; }
        public int NumOfPosComments { get; set; }
        public int NumOfNegComments { get; set; }
        public string UnitName { get; set; }
    }

    //Model class for displaying Comments on ViewComments.aspx page
    public class CommentsList
    {
        public DateTime  utcDate { get; set; }
        public string PositiveComments { get; set; }
        public string NegativeComments { get; set; }
    }

}