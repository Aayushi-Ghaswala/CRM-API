namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.LI_GI_Module
{
    public class UpdateInsuranceClientDto
    {
        public int Id { get; set; }
        public string? InsPlan { get; set; }
        public string? InsPolicy { get; set; }
        public DateTime? InsDuedate { get; set; }
        public decimal? PremiumAmount { get; set; }
        public int InsUserid { get; set; }
        public int InvType { get; set; }
        public int InvSubtype { get; set; }
        public int InsAmount { get; set; }
        public string InsNewpolicy { get; set; }
        public int InsPlantypeId { get; set; }
        public int Companyid { get; set; }
        public DateTime? InsStartdate { get; set; }
        public int InsTerm { get; set; }
        public string? InsFrequency { get; set; }
        public DateTime? InsPremiumRmdDate { get; set; }
        public bool IsSendForReview { get; set; }
        public bool IsKathrough { get; set; }
        public bool IsEmailReminder { get; set; }
        public bool IsNotification { get; set; }
        public bool IsSmsReminder { get; set; }
    }
}
