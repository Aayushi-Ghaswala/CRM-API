namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Investment_Module
{
    public class UpdateSubInvestmentTypeDto
    {
        public int Id { get; set; }
        public string InvestmentType { get; set; }
        public int InvesmentTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
