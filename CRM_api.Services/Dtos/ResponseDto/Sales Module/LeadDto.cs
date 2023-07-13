using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class LeadDto
    {
        public int Id { get; set; }
        public UserNameDto? AssignUser { get; set; }
        public UserNameDto? ReferredUser { get; set; }
        public CampaignDto? CampaignMaster { get; set; }
        public StatusDto? StatusMaster { get; set; }
        public CityMasterDto? CityMaster { get; set; }
        public StateMasterDto? StateMaster { get; set; }
        public CountryMasterDto? CountryMaster { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public List<InvesmentTypeDto>? TblInvesmentTypes { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
