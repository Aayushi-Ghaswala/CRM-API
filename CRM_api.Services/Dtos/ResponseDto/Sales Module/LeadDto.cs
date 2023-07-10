using CRM_api.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class LeadDto
    {
        public int Id { get; set; }
        public TblUserMaster AssignUser { get; set; }
        public TblUserMaster ReferredUser { get; set; }
        public TblCampaignMaster CampaignMaster { get; set; }
        public TblStatusMaster StatusMaster { get; set; }
        public TblCityMaster CityMaster { get; set; }
        public TblStateMaster StateMaster { get; set; }
        public TblCountryMaster CountryMaster { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public TblInvesmentType TblInvesmentType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
