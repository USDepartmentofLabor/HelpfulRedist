using System;

namespace FeedbackScript.Admin.Models
{
    //Model class for grid pagination
    public class PaginationInfo
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int ResultPerPage { get; set; }
        public string SortBy { get; set; }
        public string Order { get; set; }
        public string urlContains { get; set; }
        public string SelectedAgency{ get; set; }
        public string txtDateFrom {get;set;}
        public string txtDateTo {get; set;}
        public string URL { get; set; }
        public string UnitName { get; set; }
        public int TotalPages
        {
            get { return Convert.ToInt32(Decimal.Ceiling((Decimal)TotalCount / (Decimal)ResultPerPage)); }
        }
    }
}
