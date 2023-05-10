using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMgaindetail
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? MgainAccountnum { get; set; }
        public string? MgainModeholder { get; set; }
        public string? Mgain1stholder { get; set; }
        public string? Mgain1stholderpan { get; set; }
        public decimal? MgainInvamt { get; set; }
        public string? MgainType { get; set; }
        public string? MgainProjectname { get; set; }
        public string? MgainPlotno { get; set; }
        public decimal? MgainAllocatedsqft { get; set; }
        public decimal? MgainAllocatedsqftamt { get; set; }
        public decimal? MgainTotalsqft { get; set; }
        public decimal? MgainTotalplotamt { get; set; }
        public string? MgainAggre { get; set; }
        public DateTime? MgainRedemdate { get; set; }
        public decimal? MgainRedemamt { get; set; }
        public int? MgainUserid { get; set; }
        public bool? MgainIsactive { get; set; }
    }
}
