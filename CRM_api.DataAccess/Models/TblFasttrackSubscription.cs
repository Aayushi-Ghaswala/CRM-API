using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFasttrackSubscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? MobileNo { get; set; }
        public DateTime? Date { get; set; }
        public double? Amount { get; set; }
        public double? Tax { get; set; }
    }
}
