using CRM_api.DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module
{
    public class InsuranceClientDto
    {
        public int Id { get; set; }
        public string? InsPlantype { get; set; }
        public string? InsPlan { get; set; }
        public string? InsPolicy { get; set; }
        public string? InsUsername { get; set; }
        public DateTime? InsDuedate { get; set; }
        public decimal? PremiumAmount { get; set; }
        public string? InsEmail { get; set; }
        public string? InsPan { get; set; }
        public string? InsMobile { get; set; }
        public virtual UserMasterDto? TblUserMaster { get; set; }
        public virtual InvestmentTypeDto? TblInvesmentType { get; set; }
        public virtual SubInvestmentTypeDto? TblSubInvesmentType { get; set; }
        public int? InsAmount { get; set; }
        public string? InsNewpolicy { get; set; }
        public virtual InsuranceTypeMasterDto? TblInsuranceTypeMaster { get; set; }
        public virtual InsuranceCompanyListDto? TblInsuranceCompanylist { get; set; }
        public DateTime? InsStartdate { get; set; }
        public int? InsTerm { get; set; }
        public string? InsFrequency { get; set; }
        public DateTime? InsPremiumRmdDate { get; set; }
        public bool? IsSendForReview { get; set; }
        public bool? IsKathrough { get; set; }
        public bool? IsEmailReminder { get; set; }
        public bool? IsNotification { get; set; }
        public bool? IsSmsReminder { get; set; } 
    }
}
