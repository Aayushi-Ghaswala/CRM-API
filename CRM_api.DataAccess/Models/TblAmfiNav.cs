namespace CRM_api.DataAccess.Models
{
    public partial class TblAmfiNav
    {
        public int Id { get; set; }
        public string? SchemeCode { get; set; }
        public string? Isin { get; set; }
        public string? SchemeName { get; set; }
        public string? NetAssetValue { get; set; }
        public DateTime? Date { get; set; }
    }
}
