namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module
{
    public class AddFasttrackBenefitsDto
    {
        public string Product { get; set; }
        public decimal Basic { get; set; }
        public decimal Silver { get; set; }
        public decimal Gold { get; set; }
        public decimal Platinum { get; set; }
        public decimal Diamond { get; set; }
        public int InvTypeId { get; set; }
        public bool IsParentAllocation { get; set; }
    }
}