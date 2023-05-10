using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMfSchemeMaster
    {
        public int SchemeId { get; set; }
        public string? SchemeCode { get; set; }
        public string? Isingrowth { get; set; }
        public string? IsinReinvestment { get; set; }
        public string? SchemeName { get; set; }
        public string? SchemeCategorytype { get; set; }
        public string? NetAssetValue { get; set; }
        public DateTime? SchemeDate { get; set; }
    }
}
