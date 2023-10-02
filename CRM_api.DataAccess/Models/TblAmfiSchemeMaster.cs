namespace CRM_api.DataAccess.Models
{
    public partial class TblAmfiSchemeMaster
    {
        public int Id { get; set; }
        public string? Amc { get; set; }
        public string? SchemeCode { get; set; }
        public string? SchemeName { get; set; }
        public string? SchemeType { get; set; }
        public string? SchemeCategory { get; set; }
        public string? SchemeNavname { get; set; }
        public string? SchemeMinAmt { get; set; }
        public DateTime? LaunchDate { get; set; }
        public DateTime? ClosureDate { get; set; }
        public string? Isin { get; set; }

        public void Update(string? amc, string? schemeName, string? schemeType, string? schemeCategory, string? schemeNavName, string? schemeMinAmt, DateTime? launchDate, DateTime? closureDate, string? isin)
        {
            Amc = amc;
            SchemeName = schemeName;
            SchemeType = schemeType;
            SchemeCategory = schemeCategory;
            SchemeNavname = schemeNavName;
            SchemeMinAmt = schemeMinAmt;
            LaunchDate = launchDate;
            ClosureDate = closureDate;
            Isin = isin;
        }
    }
}
