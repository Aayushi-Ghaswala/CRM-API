using System.ComponentModel;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.LI_GI_Module
{
    public class AddInsuranceClientDto
    {
        public string? InsPlan { get; set; }
        public string? InsPolicy { get; set; }
        public DateTime? InsDuedate { get; set; }
        public decimal? PremiumAmount { get; set; }
        public int InsUserid { get; set; }
        public int InvSubtype { get; set; }
        public int InsAmount { get; set; }
        public string? InsNewpolicy { get; set; }
        public int InsPlantypeId { get; set; }
        public int Companyid { get; set; }
        public DateTime? InsStartdate { get; set; }
        public int? InsTerm { get; set; }
        public string? InsFrequency { get; set; }
        public DateTime? InsPremiumRmdDate { get; set; }
        [DefaultValue(false)]
        public bool IsSendForReview { get; set; }
        [DefaultValue(false)]
        public bool IsKathrough { get; set; }
        [DefaultValue(false)]
        public bool IsEmailReminder { get; set; }
        [DefaultValue(false)]
        public bool IsNotification { get; set; }
        [DefaultValue(false)]
        public bool IsSmsReminder { get; set; }
    }
}
