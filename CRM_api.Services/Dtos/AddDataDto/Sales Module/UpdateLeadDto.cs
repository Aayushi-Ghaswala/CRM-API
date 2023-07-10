using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class UpdateLeadDto
    {
        public int Id { get; set; }
        public int AssignedBy { get; set; }
        public int ReferredBy { get; set; }
        public int CampaignId { get; set; }
        public int StatusId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int InterestedIn { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
