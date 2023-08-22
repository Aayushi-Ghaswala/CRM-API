namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Investment_Module
{
    public class SubInvestmentTypeDto
    {
        public int Id { get; set; }
        public string? SubInvestmentType { get; set; }
        public int? InvesmentTypeId { get; set; }
        public bool IsActive { get; set; }
        public InvestmentTypeDto InvesmentType { get; set; }
    }
}
