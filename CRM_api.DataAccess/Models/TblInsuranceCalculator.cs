using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblInsuranceCalculator
    {
        public int Insurancesid { get; set; }
        public string? Name { get; set; }
        public DateTime? InsDate { get; set; }
        public string? Gender { get; set; }
        public int? Annualincome { get; set; }
        public int? Existinglifecover { get; set; }
        public int? Totalloandue { get; set; }
        public int? Totalsaving { get; set; }
        public DateTime? Dob { get; set; }
        public int? HomeLoanDue { get; set; }
    }
}
