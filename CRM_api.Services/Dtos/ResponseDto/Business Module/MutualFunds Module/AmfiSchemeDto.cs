namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module
{
    public class AmfiSchemeDto
    {
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
    }
}
