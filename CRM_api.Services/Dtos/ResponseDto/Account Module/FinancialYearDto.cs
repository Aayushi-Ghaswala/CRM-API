namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class FinancialYearDto
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
