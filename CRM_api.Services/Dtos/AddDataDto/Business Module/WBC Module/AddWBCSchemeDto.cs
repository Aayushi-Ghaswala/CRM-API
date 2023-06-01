namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module
{
    public class AddWBCSchemeDto
    {
        public int? WbcTypeId { get; set; }
        public int? ParticularsId { get; set; }
        public int? ParticularsSubTypeId { get; set; }
        public bool? IsRedeemable { get; set; }
        public string? Business { get; set; }
        public int? Benefits { get; set; }
        public int? NoOfContactsAllowed { get; set; }
        public int? GoldPoint { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
