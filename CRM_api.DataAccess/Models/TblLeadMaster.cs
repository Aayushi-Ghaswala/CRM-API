using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public class TblLeadMaster
    {
        public int Id { get; set; }
        public int? AssignedTo { get; set; }
        public int? ReferredBy { get; set; }
        public int CampaignId { get; set; }
        public int StatusId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string InterestedIn { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public virtual TblUserMaster AssignUser { get; set; }
        [ForeignKey(nameof(ReferredBy))]
        public virtual TblUserMaster ReferredUser { get; set; }
        [ForeignKey(nameof(CampaignId))]
        public virtual TblCampaignMaster CampaignMaster { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual TblStatusMaster StatusMaster { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual TblCityMaster CityMaster { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual TblStateMaster StateMaster { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual TblCountryMaster CountryMaster { get; set; }        
    }
}
