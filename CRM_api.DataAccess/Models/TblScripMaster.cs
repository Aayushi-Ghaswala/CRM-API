using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblScripMaster
    {
        public int Scripid { get; set; }
        public string? Scripsymbol { get; set; }
        public string? Scripname { get; set; }
        public string? Isin { get; set; }
        public string? Exchange { get; set; }
        public decimal? Ltp { get; set; }
        public DateTime? Ltt { get; set; }
    }
}
