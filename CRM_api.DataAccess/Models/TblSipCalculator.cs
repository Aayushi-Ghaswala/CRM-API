using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblSipCalculator
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Sipamount { get; set; }
        public int? NoOfYear { get; set; }
        public decimal? ExpectedReturnPer { get; set; }
    }
}
