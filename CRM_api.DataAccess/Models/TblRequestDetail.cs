using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblRequestDetail
    {
        public int InvestmentId { get; set; }
        public int? RequestId { get; set; }
        public int? InvestmentTypeId { get; set; }
        public int? InvestmentSubtypeId { get; set; }
        public decimal? LoanInvAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public int? PaymentTenure { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public decimal? RateOfInterest { get; set; }
        public decimal? PaymentLength { get; set; }
        public string? FinancialSector { get; set; }
    }
}
