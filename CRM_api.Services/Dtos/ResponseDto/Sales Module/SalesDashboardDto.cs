namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class SalesDashboardDto
    {
        public string? Type { get; set; }
        public int? Week { get; set; } = 0;
        public int? Month { get; set; } = 0;
        public int? Quarter { get; set; } = 0;
    }
}
