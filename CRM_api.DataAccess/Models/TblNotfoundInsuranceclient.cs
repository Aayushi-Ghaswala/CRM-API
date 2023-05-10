using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblNotfoundInsuranceclient
    {
        public int Id { get; set; }
        public string? InsPlantype { get; set; }
        public string? InsPlan { get; set; }
        public string? InsPolicy { get; set; }
        public string? InsUsername { get; set; }
        public DateTime? InsDuedate { get; set; }
        public decimal? InsAmount { get; set; }
        public string? InsEmail { get; set; }
        public string? InsPan { get; set; }
        public string? InsMobile { get; set; }
        public int? InsSumAssuredInsured { get; set; }
        public string? InsNewpolicy { get; set; }
    }
}
