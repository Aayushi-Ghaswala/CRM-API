namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module
{
    public class GoldPointDto
    {
        public int GpId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Credit { get; set; } = 0;
        public int? Debit { get; set; } = 0;
        public string Username { get; set; }
        public string? Type { get; set; }
        public string? PointCategory { get; set; }
        public int? Vendorid { get; set; }
    }
}
