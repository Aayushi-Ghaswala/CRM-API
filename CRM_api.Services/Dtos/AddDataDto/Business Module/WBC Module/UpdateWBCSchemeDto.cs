namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module
{
    public class UpdateWBCSchemeDto
    {
        public int Id { get; set; }
        public int? WbcTypeId { get; set; }
        public int? ParticularsId { get; set; }
        public int? ParticularsSubTypeId { get; set; }
        public bool? IsRedeemable { get; set; }
        public string? Business { get; set; }
        public int? GoldPoint { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime ToDate { get; set; } = new DateTime(9999, 12, 31);
        public int? On_the_spot_GP { get; set; }
        public bool IsParentAllocation { get; set; }
    }
}
