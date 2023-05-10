using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblPortfolioReviewRequest
    {
        public int RequestId { get; set; }
        public string? RequestUserid { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? RequestType { get; set; }
        public string? RequestPan { get; set; }
        public string? RequestEmail { get; set; }
        public string? RequestMobile { get; set; }
        public string? Status { get; set; }
        public string? RequestPdf { get; set; }
        public string? Stinvtype { get; set; }
        public int? RequestSubtype { get; set; }
        public string? PdfPassword { get; set; }

        public virtual TblInvesmentType? RequestTypeNavigation { get; set; }
    }
}
