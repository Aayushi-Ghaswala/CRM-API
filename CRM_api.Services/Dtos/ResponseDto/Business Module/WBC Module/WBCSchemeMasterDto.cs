namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module
{
    public class WBCSchemeMasterDto
    {
        public int Id { get; set; }
        public int? WbcTypeId { get; set; }
        public int? ParticularsId { get; set; }
        public int? ParticularsSubTypeId { get; set; }
        public bool? IsRedeemable { get; set; }
        public string? Business { get; set; }
        public int? GoldPoint { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public virtual WbcTypeDto? TblWbcTypeMaster { get; set; }
        public virtual SubInvestmentTypeDto? TblSubInvesmentType { get; set; }
        public virtual SubSubInvestmentTypeDto? TblSubsubInvType { get; set; }
    }
}
