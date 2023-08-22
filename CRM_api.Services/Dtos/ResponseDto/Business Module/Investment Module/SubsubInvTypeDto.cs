namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Investment_Module
{
    public class SubsubInvTypeDto
    {
        public int Id { get; set; }
        public string? SubsubInvType { get; set; }
        public int? SubInvTypeId { get; set; }
        public bool IsActive { get; set; }
        public SubInvestmentTypeDto TblSubInvesmentType { get; set; }
    }
}
