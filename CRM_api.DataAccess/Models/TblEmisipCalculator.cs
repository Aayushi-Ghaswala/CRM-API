using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblEmisipCalculator
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? LoanAmount { get; set; }
        public decimal? LoanInterestRate { get; set; }
        public int? NoOfYear { get; set; }
        public decimal? InterestRateOnInvestment { get; set; }
    }
}
