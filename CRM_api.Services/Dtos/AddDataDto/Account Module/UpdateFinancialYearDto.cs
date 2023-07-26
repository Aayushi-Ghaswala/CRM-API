namespace CRM_api.Services.Dtos.AddDataDto.Account_Module
{
    public class UpdateFinancialYearDto
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
    }
}
