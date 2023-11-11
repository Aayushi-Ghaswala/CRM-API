namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module
{
    public class AmfiNavDto
    {
        public string? SchemeCode { get; set; }
        public string? Isin { get; set; }
        public string? SchemeName { get; set; }
        public string? NetAssetValue { get; set; }
        public DateTime? Date { get; set; }
    }
}
